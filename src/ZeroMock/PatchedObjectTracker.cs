using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeroMock;

internal class MethodData
{
    public List<ISetupResultAccessor> Setups { get; } = new List<ISetupResultAccessor>();

    public List<object[]> Invocation { get; } = new List<object[]>();
}


internal static class PatchedObjectTracker
{
    private static readonly ConditionalWeakTable<object, Dictionary<MethodBase, MethodData>> _objects = new();

    public static void Track(object obj)
    {
        _objects.Add(obj, new());
    }

    public static void Add(object obj, MethodBase method, ISetupResultAccessor setupAccessor)
    {
        if (_objects.TryGetValue(obj, out var methods))
        {
            if (methods.TryGetValue(method, out var setups))
            {
                setups.Setups.Add(setupAccessor);
            }
            else
            {
                var methodData = new MethodData();
                methodData.Setups.Add(setupAccessor);
                methods[method] = methodData;
            }
            return;
        }

        throw new InvalidOperationException("Object instance not tracked");
    }

    public static int GetInvocationCount(object obj, MethodInfo methodInfo, ArgumentMatcher matcher)
    {
        if (_objects.TryGetValue(obj, out var setupResults))
        {
            if (setupResults.TryGetValue(methodInfo, out var resultAccessor))
            {
                return resultAccessor.Invocation.Count(e => matcher.Match(e) == true);
            }

            return 0;
        }

        throw new InvalidOperationException("Object instance not tracked");
        throw new NotImplementedException("GetInvocationCount not implemented");
    }

    public static PatchState TryGetObjectMethodResults
        (object obj,
        MethodBase originalMethod,
        object[] args,
       out ISetupResultAccessor setupResult)
    {
        if (_objects.TryGetValue(obj, out var objectRegistration))
        {
            if (objectRegistration.TryGetValue(originalMethod, out var setupRegistration))
            {
                setupRegistration.Invocation.Add(args);

                var match = setupRegistration.Setups.FirstOrDefault(e => e.Match(args));
                if (match != null)
                {
                    setupResult = match;
                    return PatchState.Setup;
                }
            }

            setupResult = null!;
            return PatchState.NotSetup;
        }

        setupResult = null!;
        return PatchState.NotTracked;
    }
}

internal enum PatchState
{
    NotTracked,
    NotSetup,
    Setup
}