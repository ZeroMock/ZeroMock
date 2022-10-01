namespace ZeroMock;

internal interface ISetupResultAccessor
{
    int InvocationAmount { get; }
    Func<object>? GetReturnValue { get; }

    public Action? Callback { get; }

    public bool Match(object[] args);
}