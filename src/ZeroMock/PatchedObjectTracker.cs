using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeroMock;

public class MockOperations
{
    public Dictionary<MethodBase, Func<object>> MethodOverrides { get; } = new();
    public Action? Callback { get; set; }
}

internal static class PatchedObjectTracker
{
    private static readonly ConditionalWeakTable<object, MockOperations> mockResults = new();

    public static void Track(object obj)
    {
        mockResults.Add(obj, new());
    }

    public static void AddMethod(MethodInfo methodInfo, object obj, Func<object> func)
    {
        if (mockResults.TryGetValue(obj, out var dict))
        {
            dict.MethodOverrides[methodInfo] = func;
            return;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static void AddCallback(object obj, Action callback)
    {
        if (mockResults.TryGetValue(obj, out var dict))
        {
            dict.Callback = callback;
            return;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static bool TryGetObjectMethodResults(object obj, out MockOperations mockOperations)
    {
        return mockResults.TryGetValue(obj, out mockOperations);
    }
}
