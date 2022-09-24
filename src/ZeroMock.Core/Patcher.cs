using System.Reflection;
using HarmonyLib;

namespace ZeroMock.Core;

public class PatchFailedException : Exception
{
    public PatchFailedException(string message, Exception e) : base(message, e)
    {
    }
}

internal static class Patcher
{
    private static readonly HashSet<Type> _seenTypes = new HashSet<Type>();
    private static readonly MethodInfo _ctorPrefix = typeof(Patcher).GetMethod(nameof(CtorPrefix), BindingFlags.Static | BindingFlags.Public)!;
    private static readonly HarmonyMethod _prefixPatch = new HarmonyMethod(_ctorPrefix);

    /// <summary>
    /// Do not execute the original implemenation
    /// </summary>
    const bool Skip = false;
    private static readonly Harmony _harmony = new Harmony("com.ZermMock.patch");

    public static void Prepare<T>() where T : class
    {
        if (_seenTypes.Contains(typeof(T)))
        {
            return;
        }

        var ctor = typeof(T).GetConstructors(BingingFlagsHelper.InstanceAll).First();
        var methods = typeof(T).GetMethods(BingingFlagsHelper.InstanceAll | BindingFlags.DeclaredOnly);

        foreach (var method in methods)
        {
            Patch<T>(method, _prefixPatch);
        }

        Patch<T>(ctor, _prefixPatch);

        _seenTypes.Add(typeof(T));
    }

    private static void Patch<T>(MethodBase original, HarmonyMethod prefix)
    {
        try
        {
            _harmony.Patch(original, prefix);
        }
        catch (Exception e)
        {
            throw new PatchFailedException($"Could not patch {typeof(T).Name}.{original.Name}. Usually this is because it has been inlined.", e);
        }
    }

#pragma warning disable IDE1006 // Naming Styles
    public static bool CtorPrefix(object __instance, MethodBase __originalMethod)
    {
        return Skip;
    }

#pragma warning restore IDE1006 // Naming Styles
}
