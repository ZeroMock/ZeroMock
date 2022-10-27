using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ZeroMock;
public static class Mock
{
    private static readonly ConditionalWeakTable<object, object> _mockedObjects = new();

    public static T Of<T>(Expression<Func<T, object>> predicate) where T : class
    {
        var mock = new Mock<T>();

        _mockedObjects.Add(mock.Object, mock);
        mock.SetupAllProperties();

        if (predicate.Body is UnaryExpression ue)
        {
            if (ue.Operand is BinaryExpression be)
            {
                if (ue.Operand.NodeType != ExpressionType.Equal)
                {
                    throw new InvalidOperationException($"{ue.Operand.NodeType} not supported. Must be == or &&");
                }

                var left = be.Left;
                var right = be.Right as ConstantExpression;

                if (left.NodeType == ExpressionType.MemberAccess && left is MemberExpression me)
                {
                    var member = me.Member;

                    if (member is PropertyInfo pi)
                    {
                        var result = new SetupResult(new ArgumentMatcher(new List<Condition>()));
                        result.Returns(right.Value);
                        PatchedObjectTracker.AddSetup(mock.Object, pi.GetMethod, new SetupResultAccessor(result));
                    }
                }
            }
        }

        return mock.Object;
    }

    public static T Of<T>() where T : class
    {
        var mock = new Mock<T>();

        _mockedObjects.Add(mock.Object, mock);
        mock.SetupAllProperties();

        return mock.Object;
    }

    public static Mock<T> Get<T>(T obj) where T : class
    {
        if (_mockedObjects.TryGetValue(obj, out var mock))
        {
            return (Mock<T>)mock;
        }

        throw new InvalidOperationException("Object does not have mock associated");
    }
}