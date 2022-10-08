namespace ZeroMock;

internal class SetupResultAccessor<T> : ISetupResultAccessor
{
    private readonly SetupResult<T> _setupResult;

    public SetupResultAccessor(SetupResult<T> setupResult)
    {
        _setupResult = setupResult;
    }

    public Func<dynamic[], dynamic>? GetReturnValue => _setupResult.GetReturn;

    public int InvocationAmount => _setupResult.InvocationAmount;
    public Action<dynamic[]>? Callback => _setupResult.GetCallback;

    public bool Match(object[] args) => _setupResult.Match(args);
}
