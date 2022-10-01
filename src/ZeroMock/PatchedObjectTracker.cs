using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeroMock;

/// <summary>
/// Track data associated with mocked objects
/// </summary>
internal static class PatchedObjectTracker
{
    /// <summary>
    /// Mocked objects, and the setup associated with each method/property
    /// </summary>
    private static readonly ConditionalWeakTable<object, Dictionary<MethodBase, MethodData>> _objects = new();

    /// <summary>
    /// Register an object being mocked
    /// </summary>
    public static void Track(object obj)
    {
        _objects.Add(obj, new());
    }

    /// <summary>
    /// Add a Setup operation to an objects method/property
    /// </summary>
    public static void AddSetup(object obj, MethodBase method, ISetupResultAccessor setupAccessor)
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

    /// <summary>
    /// Get how often a method/property was called
    /// </summary>
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

    /// <summary>
    /// Get a Setup operation if it exists
    /// </summary>
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

                // TODO Should this be last or default?
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
