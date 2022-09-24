using System.Reflection;

namespace ZeroMock.Core;

internal class InstanceFactory
{
    public T CreateNew<T>() where T : class
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var ctor = typeof(T).GetConstructors(flags).First();

        var types = ctor.GetParameters().Select(e => e.ParameterType).Select(e => GetDefaultValue(e)).ToArray();

        var result = Activator.CreateInstance(typeof(T), flags, null, types, null);
        ArgumentNullException.ThrowIfNull(result);
        return (T)result;
    }

    private static object GetDefaultValue(Type t)
    {
        if (t.IsValueType)
            return Activator.CreateInstance(t)!;

        return null!;
    }
}