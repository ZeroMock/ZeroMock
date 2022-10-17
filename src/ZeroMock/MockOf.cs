using System.Runtime.CompilerServices;

namespace ZeroMock;
public static class Mock
{
    private static readonly ConditionalWeakTable<object, object> _mockedObjects = new();

    public static T Of<T>() where T : class
    {
        var mock = new Mock<T>();

        _mockedObjects.Add(mock.Object, mock);

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