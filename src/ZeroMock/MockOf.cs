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
                ParseBinaryExpression(mock, be);
            }
        }

        return mock.Object;
    }

    private static void ParseBinaryExpression<T>(Mock<T> mock, BinaryExpression be) where T : class
    {
        if (be.NodeType == ExpressionType.Equal)
        {
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
            else if (left.NodeType == ExpressionType.Call && left is MethodCallExpression mce)
            {
                var result = new SetupResult(new ArgumentMatcher(new List<Condition>()));
                result.Returns(right.Value);
                PatchedObjectTracker.AddSetup(mock.Object, mce.Method, new SetupResultAccessor(result));
            }
        }
        else if (be.NodeType == ExpressionType.AndAlso)
        {
            var left = be.Left as BinaryExpression;
            var right = be.Right as BinaryExpression;
            ParseBinaryExpression(mock, left);
            ParseBinaryExpression(mock, right);
        }
        else
        {
            throw new InvalidOperationException($"{be.NodeType} not supported. Must be == or &&");
        }
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