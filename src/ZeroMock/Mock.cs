using System.Linq.Expressions;
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
}

public class Mock<T> where T : class
{
    public SetupResult<TReturn> Setup<TReturn>(Expression<Func<T, TReturn>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            Patcher.Patch(mce.Method);
            return new SetupResult<TReturn>(mce.Method, _object);
        }

        throw new InvalidOperationException("Setup only works for methods");
    }

    private static T Create()
    {
        Patcher.SetupHooks<T>();
        var instance = InstanceFactory.CreateNew<T>();
        PatchedObjectTracker.Track(instance);
        return instance;
    }

    private readonly T _object = Create();
    public T Object => _object;

    //public void Setup(Expression<Action<T>> expression)
    //{
    //    var t = typeof(T);
    //    Console.WriteLine(t.Name);
    //}

}