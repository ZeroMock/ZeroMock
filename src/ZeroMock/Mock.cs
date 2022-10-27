using System.Linq.Expressions;
using System.Reflection;

namespace ZeroMock;



/// <summary>
/// Mock a concrete class
/// </summary>
public class Mock<T> where T : class
{
    private HashSet<MethodInfo> _setupProperties = new();

    public Mock()
    {
        if (!IsConcreteType(typeof(T)))
        {
            throw new InvalidOperationException($"ZeroMock can only be used with concrete classes. Cannot mock {typeof(T).Name}");
        }
    }

    public void SetupAllProperties()
    {
        var methods = typeof(T).GetMethods(BingingFlagsHelper.InstanceAll | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            _setupProperties.Add(method);
        }

    }

    private static bool IsConcreteType(Type type)
    {
        return type.IsClass && !type.IsAbstract && !type.IsInterface;
    }

    /// <summary>
    /// Check if a method was called matching the condition
    /// </summary>
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

    /// <summary>
    /// Check if a method was called matching the condition
    /// </summary>
    public void Verify(Expression<Action<T>> expression, Times? times = null)
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

    /// <summary>
    /// Setup a method matching the condition
    /// </summary>
    public SetupResult<TReturn> Setup<TReturn>(Expression<Func<T, TReturn>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            var conditions = GetMethodCallExpressionArguments(mce);

            Patcher.Patch(mce.Method);
            var result = new SetupResult<TReturn>(new ArgumentMatcher(conditions));
            PatchedObjectTracker.AddSetup(Object, mce.Method, new SetupResultAccessor<TReturn>(result));
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
            PatchedObjectTracker.AddSetup(Object, pi.GetMethod, new SetupResultAccessor<TReturn>(result));
            return result;
        }

        throw new InvalidOperationException("Setup only works for methods and properties");
    }

    /// <summary>
    /// Setup a method matching the condition
    /// </summary>
    public SetupResult Setup(Expression<Action<T>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            var conditions = GetMethodCallExpressionArguments(mce);

            Patcher.Patch(mce.Method);
            var result = new SetupResult(new ArgumentMatcher(conditions));
            PatchedObjectTracker.AddSetup(Object, mce.Method, new SetupResultAccessor(result));
            return result;
        }

        throw new InvalidOperationException("Setup only works for methods");
    }

    private static List<Condition> GetMethodCallExpressionArguments(MethodCallExpression mce)
    {
        var conditions = new List<Condition>();

        foreach (var arg in mce.Arguments)
        {
            if (arg is MethodCallExpression argMce)
            {
                if (argMce.Method.DeclaringType?.Namespace == "ZeroMock" && argMce.Method.Name == "IsAny")
                {
                    Func<dynamic, bool> any = (dynamic _) => true;
                    var match = new Condition(any);
                    conditions.Add(match);
                }
                else
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
            else if (arg is MemberExpression me && me.Member.DeclaringType?.Namespace == "ZeroMock" && me.Member.Name == "IsAny")
            {
                Func<dynamic, bool> any = (dynamic _) => true;
                var match = new Condition(any);
                conditions.Add(match);
            }
            else
            {
                throw new NotImplementedException($"Unhandled path for {arg.GetType()}");
            }
        }

        return conditions;
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
