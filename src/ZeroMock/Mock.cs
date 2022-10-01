using System.Linq.Expressions;
using System.Reflection;

namespace ZeroMock;

public class Mock<T> where T : class
{
    public void Verify<TReturn>(Expression<Func<T, TReturn>> expression, Times? times = null)
    {
        times ??= Times.AtLeastOnce();

        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            var conditions = GetMethodCallExpressionArguments(mce);
            var count = PatchedObjectTracker.GetInvocationCount(_object, mce.Method, new ArgumentMatcher(conditions));
            conditions.AddRange(GetMethodCallExpressionArguments(mce));

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
            var conditions = GetMethodCallExpressionArguments(mce);

            Patcher.Patch(mce.Method);
            var result = new SetupResult<TReturn>(new ArgumentMatcher(conditions));
            PatchedObjectTracker.Add(Object, mce.Method, new SetupResultAccessor<TReturn>(result));
            return result;
        }

        if (body is MemberExpression me && me.Member is PropertyInfo pi)
        {
            if (pi.SetMethod != null)
            {
                Patcher.Patch(pi.SetMethod);
            }

            Patcher.Patch(pi.GetMethod);
            var result = new SetupResult<TReturn>(new ArgumentMatcher(new List<Condition>()));
            PatchedObjectTracker.Add(Object, pi.GetMethod, new SetupResultAccessor<TReturn>(result));
            return result;
        }

        throw new InvalidOperationException("Setup only works for methods and properties");
    }

    private static List<Condition> GetMethodCallExpressionArguments(MethodCallExpression mce)
    {
        var conditions = new List<Condition>();

        foreach (var arg in mce.Arguments)
        {
            if (arg is MethodCallExpression argMce)
            {
                if (argMce.Arguments[0] is UnaryExpression ue)
                {
                    var match = new Condition(ue);
                    conditions.Add(match);
                }
            }
            else if (arg is ConstantExpression ce)
            {
                var match = new Condition(ce);
                conditions.Add(match);
            }
            else
            {
                throw new NotImplementedException($"Unhandled path for {arg.GetType()}");
            }
        }

        return conditions;
    }

    public SetupResult Setup(Expression<Action<T>> expression)
    {
        var body = expression.Body;

        var matchers = new List<Condition>();

        if (body is MethodCallExpression mce)
        {
            // TODO Generate matchers
            Patcher.Patch(mce.Method);
            var result = new SetupResult(new ArgumentMatcher(matchers));
            PatchedObjectTracker.Add(Object, mce.Method, new SetupResultAccessor(result));
            return result;
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
