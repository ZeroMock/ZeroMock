using System.Diagnostics;
using System.Reflection;
using HarmonyLib;

namespace ZeroMock;

/// <summary>
/// Harmony2 wrapper to inject hooks into function calls
/// </summary>
internal static class Patcher
{
    private static readonly HashSet<Type> _seenTypes = new HashSet<Type>();
    private static readonly MethodInfo _prefixVoidMethodInfo = typeof(Patcher).GetMethod(nameof(ActionHook), BindingFlags.Static | BindingFlags.Public)!;
    private static readonly MethodInfo _prefixReturnMethodInfo = typeof(Patcher).GetMethod(nameof(FuncHook), BindingFlags.Static | BindingFlags.Public)!;
    private static readonly HarmonyMethod _prefixVoidPatch = new HarmonyMethod(_prefixVoidMethodInfo);
    private static readonly HarmonyMethod _prefixReturnPatch = new HarmonyMethod(_prefixReturnMethodInfo);
    internal static readonly Dictionary<MethodInfo, Func<object>> MethodResults = new Dictionary<MethodInfo, Func<object>>();

    /// <summary>
    /// Do not execute the original implemenation
    /// </summary>
    const bool Skip = false;
    private static readonly Harmony _harmony = new Harmony("com.ZermMock.patch");

    /// <summary>
    /// Hook all non generic functions/properties/ctor to return default value and skip original implementation
    /// </summary>
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
            if (!method.IsGenericMethod)
            {
                Patch(method);

            }
        }

        Patch(ctor);

        _seenTypes.Add(typeof(T));
    }

    /// <summary>
    /// Inject hook to return default value and skip original implementation
    /// </summary>
    /// <param name="original"></param>
    /// <exception cref="PatchFailedException"></exception>
    public static void Patch(MethodBase original)
    {
        try
        {
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
                __result = methodResults.GetReturnValue(__args);
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

    /// <summary>
    /// Check if we are in the callstack of InstanceFactory.CreateNew
    /// </summary>
    private static bool IsInConstructorOfNewMockObject()
    {
        var st = new StackTrace(1, false);
        return st.GetFrames().Select(e => e.GetMethod())
            .Where(mi => mi?.Name == nameof(InstanceFactory.CreateNew) && mi.DeclaringType == typeof(InstanceFactory))
            .Any();
    }
}
