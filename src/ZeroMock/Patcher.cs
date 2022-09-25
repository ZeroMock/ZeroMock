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
                Console.WriteLine("Not patching generic method");
                //var genMethod = method.MakeGenericMethod(typeof(object));
                //Patch<T>(genMethod, _prefixPatch);

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
        if (PatchedObjectTracker.TryGetObjectMethodResults(__instance, out _))
        {
            return Skip;
        }

        if (IsInConstructorOfNewMockObject())
        {
            return Skip;
        }

        return !Skip;
    }

    public static bool IsInConstructorOfNewMockObject()
    {
        const string newInstanceHint = $"{nameof(InstanceFactory)}.{nameof(InstanceFactory.CreateNew)}";
        var stackTrace = Environment.StackTrace;
        return stackTrace.Contains(newInstanceHint);
    }

    public static bool ReturnPrefix(object __instance, MethodBase __originalMethod, ref object __result)
    {
        if (PatchedObjectTracker.TryGetObjectMethodResults(__instance, out var methodResults))
        {
            if (methodResults.MethodOverrides.TryGetValue(__originalMethod, out var result))
            {
                __result = result();
            }

            methodResults.Callback?.Invoke();

            return Skip;
        }

        return !Skip;
    }

#pragma warning restore IDE1006 // Naming Styles
}
