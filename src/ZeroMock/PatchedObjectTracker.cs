using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeroMock;

internal static class PatchedObjectTracker
{
    private static readonly ConditionalWeakTable<object, Dictionary<MethodBase, Func<object>>> mockResults = new();

    public static void Track(object obj)
    {
        mockResults.Add(obj, new());
    }

    public static void AddMethod(MethodInfo methodInfo, object obj, Func<object> func)
    {
        if (mockResults.TryGetValue(obj, out var dict))
        {
            dict[methodInfo] = func;
            return;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static bool TryGetObjectMethodResults(object obj, out Dictionary<MethodBase, Func<object>> results)
    {
        return mockResults.TryGetValue(obj, out results);
    }

    public static bool TryGetResult(object obj, MethodBase methodBase, out Func<object> result)
    {
        if (mockResults.TryGetValue(obj, out var overrides))
        {
            return overrides.TryGetValue(methodBase, out result);
        }

        result = null;
        return false;
    }
}
