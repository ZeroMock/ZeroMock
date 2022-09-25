using System.Linq.Expressions;
using System.Reflection;

namespace ZeroMock;

public class VerificationException : Exception
{
    public VerificationException(string message) : base(message)
    {

    }
}


public class Mock<T> where T : class
{
    public void Verify<TReturn>(Expression<Func<T, TReturn>> expression, TimesOperation? times = null)
    {
        times ??= Times.AtLeastOnce();

        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            var count = PatchedObjectTracker.GetInvocationCount(mce.Method, _object);

            if (!times.Test(count))
            {
                throw new VerificationException($"Method {typeof(T)}.{mce.Method.Name} was called {count}, but expected {times}");
            }
        }
    }

    public SetupResult<TReturn> Setup<TReturn>(Expression<Func<T, TReturn>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            Patcher.Patch(mce.Method);
            return new SetupResult<TReturn>(mce.Method, _object);
        }

        if (body is MemberExpression me && me.Member is PropertyInfo pi)
        {
            if (pi.SetMethod != null)
            {
                Patcher.Patch(pi.SetMethod);
            }

            Patcher.Patch(pi.GetMethod);
            return new SetupResult<TReturn>(pi.GetMethod, _object);
        }

        throw new InvalidOperationException("Setup only works for methods and properties");
    }

    public SetupResult Setup(Expression<Action<T>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            Patcher.Patch(mce.Method);
            return new SetupResult(_object);
        }

        throw new InvalidOperationException("Setup only works for methods");
    }

    private static T Create()
    {
        Patcher.SetupHooks<T>();
        var instance = InstanceFactory.CreateNew<T>();
        return instance;
    }

    private readonly T _object = Create();
    public T Object => _object;
}