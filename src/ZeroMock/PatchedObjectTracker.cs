using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeroMock;

internal static class PatchedObjectTracker
{
    private static readonly ConditionalWeakTable<object, MockOperations> _mockResults = new();

    public static void Track(object obj)
    {
        _mockResults.Add(obj, new());
    }

    public static void AddMethod<T>(MethodInfo methodInfo, object obj, Func<T> func)
    {
        if (_mockResults.TryGetValue(obj, out var mockOperations))
        {
            mockOperations.MethodOverrides[methodInfo] = new MethodExecution<T>(func);
            return;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static int GetInvocationCount(MethodInfo methodInfo, object obj)
    {
        if (_mockResults.TryGetValue(obj, out var mockOperations))
        {
            if (mockOperations.MethodOverrides.TryGetValue(methodInfo, out var methodExecution))
            {
                return methodExecution.InvocationAmount;
            }

            return 0;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static void AddCallback(object obj, Action callback)
    {
        if (_mockResults.TryGetValue(obj, out var dict))
        {
            dict.Callback = callback;
            return;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static bool TryGetObjectMethodResults(object obj, out MockOperations mockOperations)
    {
        return _mockResults.TryGetValue(obj, out mockOperations);
    }
}
