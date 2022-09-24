namespace ZeroMock.Core;

internal static class InstanceFactory
{
    /// <summary>
    /// Create an instance by calling the first constructor with default values
    /// </summary>
    public static T CreateNew<T>() where T : class
    {
        var ctor = typeof(T).GetConstructors(BingingFlagsHelper.InstanceAll).First();

        var types = ctor.GetParameters().Select(e => e.ParameterType).Select(e => GetDefaultValue(e)).ToArray();

        var result = Activator.CreateInstance(typeof(T), BingingFlagsHelper.InstanceAll, null, types, null);

        if (result == null)
        {
            throw new InvalidOperationException($"Could not create instance of type: {typeof(T).Name}");
        }

        return (T)result;
    }

    private static object GetDefaultValue(Type t)
    {
        if (t.IsValueType)
            return Activator.CreateInstance(t)!;

        return null!;
    }
}