namespace ZeroMock;

internal interface ISetupResultAccessor
{
    /// <summary>
    /// How many times the setup method has been invoked
    /// </summary>
    int InvocationAmount { get; }

    /// <summary>
    /// If the user setup a return
    /// </summary>
    Func<object>? GetReturnValue { get; }

    /// <summary>
    /// If the user setup a callback
    /// </summary>
    public Action? Callback { get; }

    /// <summary>
    /// If this setup arguments match the passed arguments
    /// </summary>
    /// <param name="args"></param>
    public bool Match(object[] args);
}