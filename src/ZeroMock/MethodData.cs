namespace ZeroMock;

/// <summary>
/// Data to be associated with a MethodBase
/// </summary>
internal class MethodData
{
    /// <summary>
    /// Contains all .Setup operations on the method
    /// </summary>
    public List<ISetupResultAccessor> Setups { get; } = new List<ISetupResultAccessor>();

    /// <summary>
    /// Contains all the args when the method was really called
    /// </summary>
    public List<object[]> Invocation { get; } = new List<object[]>();
}
