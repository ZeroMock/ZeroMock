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
    private static readonly MethodInfo _prefixVoidMethodInfo = typeof(Patcher).GetMethod(nameof(VoidPrefix), BindingFlags.Static | BindingFlags.Public)!;
    private static readonly MethodInfo _prefixReturnMethodInfo = typeof(Patcher).GetMethod(nameof(ReturnPrefix), BindingFlags.Static | BindingFlags.Public)!;
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
    public static bool VoidPrefix(object __instance)
    {
        if (PatchedObjectTracker.TryGetObjectMethodResults(__instance, out var methodResults))
        {
            methodResults.Callback?.Invoke();

            return Skip;
        }

        if (IsInConstructorOfNewMockObject())
        {
            PatchedObjectTracker.Track(__instance);
            return Skip;
        }

        return !Skip;
    }

    public static bool ReturnPrefix(object __instance, MethodBase __originalMethod, ref object __result)
    {
        if (PatchedObjectTracker.TryGetObjectMethodResults(__instance, out var methodResults))
        {
            if (methodResults.MethodOverrides.TryGetValue(__originalMethod, out var result))
            {
                __result = result.Invoke();
            }

            methodResults.Callback?.Invoke();

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
