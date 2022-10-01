using System.Diagnostics;
using System.Reflection;
using HarmonyLib;

namespace ZeroMock;

public class PatchFailedException : Exception
{
    public PatchFailedException(string message, Exception e) : base(message, e)
    {
    }
}

internal static class Patcher
{
    private static readonly HashSet<Type> _seenTypes = new HashSet<Type>();
    private static readonly MethodInfo _prefixVoidMethodInfo = typeof(Patcher).GetMethod(nameof(ActionHook), BindingFlags.Static | BindingFlags.Public)!;
    private static readonly MethodInfo _prefixReturnMethodInfo = typeof(Patcher).GetMethod(nameof(FuncHook), BindingFlags.Static | BindingFlags.Public)!;
    private static readonly HarmonyMethod _prefixVoidPatch = new HarmonyMethod(_prefixVoidMethodInfo);
    private static readonly HarmonyMethod _prefixReturnPatch = new HarmonyMethod(_prefixReturnMethodInfo);
    internal static readonly Dictionary<MethodInfo, Func<object>> MethodResults = new Dictionary<MethodInfo, Func<object>>();
    internal static Action<string>? Log;

    /// <summary>
    /// Do not execute the original implemenation
    /// </summary>
    const bool Skip = false;
    private static readonly Harmony _harmony = new Harmony("com.ZermMock.patch");

    public static void SetupHooks<T>() where T : class
    {
        if (_seenTypes.Contains(typeof(T)))
        {
            return;
        }

        var ctor = typeof(T).GetConstructors(BingingFlagsHelper.InstanceAll).First();
        var methods = typeof(T).GetMethods(BingingFlagsHelper.InstanceAll | BindingFlags.DeclaredOnly);

        foreach (var method in methods)
        {
            if (method.IsGenericMethod)
            {
                Log?.Invoke($"Not patching generic method: {method.DeclaringType.Name}.{method.Name}");
            }
            else
            {
                Patch(method);
            }
        }

        Patch(ctor);

        _seenTypes.Add(typeof(T));
    }

    public static void Patch(MethodBase original)
    {
        try
        {
            Log?.Invoke($"Patching {original.DeclaringType.Name}.{original.Name}.");
            if (original is MethodInfo mi)
            {
                if (mi.ReturnType != typeof(void))
                {
                    _harmony.Patch(original, _prefixReturnPatch);
                    return;
                }
            }

            _harmony.Patch(original, _prefixVoidPatch);
        }
        catch (Exception e)
        {
            throw new PatchFailedException($"Could not patch {original.DeclaringType.Name}.{original.Name}. Usually this is because it has been inlined.", e);
        }
    }

#pragma warning disable IDE1006 // Naming Styles
    public static bool ActionHook(object __instance, MethodBase __originalMethod, object[] __args)
    {
        var patchState = PatchedObjectTracker.TryGetObjectMethodResults(__instance, __originalMethod, __args, out var methodResults);
        if (patchState == PatchState.Setup)
        {
            methodResults.Callback?.Invoke();

            return Skip;
        }

        if (patchState == PatchState.NotSetup)
        {
            return Skip;
        }

        if (patchState == PatchState.NotTracked)
        {
            if (IsInConstructorOfNewMockObject())
            {
                PatchedObjectTracker.Track(__instance);
                return Skip;
            }
        }

        return !Skip;
    }

    public static bool FuncHook(object __instance, MethodBase __originalMethod, object[] __args, ref object __result)
    {
        var patchState = PatchedObjectTracker.TryGetObjectMethodResults(__instance, __originalMethod, __args, out var methodResults);

        if (patchState == PatchState.Setup)
        {
            if (methodResults.GetReturnValue != null)
            {
                __result = methodResults.GetReturnValue();
            }

            methodResults.Callback?.Invoke();

            return Skip;
        }

        if (patchState == PatchState.NotSetup)
        {
            return Skip;
        }

        return !Skip;
    }

#pragma warning restore IDE1006 // Naming Styles

    public static bool IsInConstructorOfNewMockObject()
    {
        var st = new StackTrace(1, false);
        return st.GetFrames().Select(e => e.GetMethod())
            .Where(mi => mi?.Name == nameof(InstanceFactory.CreateNew) && mi.DeclaringType == typeof(InstanceFactory))
            .Any();
    }
}
