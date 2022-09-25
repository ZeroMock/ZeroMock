using System.Reflection;

namespace ZeroMock;

public class SetupResult<T>
{
    private readonly MethodInfo _methodInfo;
    private readonly object _obj;

    public SetupResult(MethodInfo methodInfo, object obj)
    {
        _methodInfo = methodInfo;
        _obj = obj;
    }

    public SetupResult<T> Returns(T result) => Returns(() => result);

    public SetupResult<T> Returns(Func<T> result)
    {
        PatchedObjectTracker.AddMethod(_methodInfo, _obj, () => result());
        return this;
    }

    public SetupResult<T> Callback(Action action)
    {
        PatchedObjectTracker.AddCallback(_obj, action);
        return this;
    }
}
