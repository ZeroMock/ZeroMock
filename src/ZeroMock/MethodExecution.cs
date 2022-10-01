namespace ZeroMock;

internal class ArgumentMatcher
{
    private readonly List<Condition> _matchers;

    public ArgumentMatcher(List<Condition> matchers)
    {
        _matchers = matchers;
    }

    public bool Match(object[] args)
    {
        if (args.Count() != _matchers.Count)
        {
            return false;
        }

        if (args.Count() == 0)
        {
            return true;
        }

        for (int i = 0; i < _matchers.Count; i++)
        {
            if (!_matchers[i].Match(args[i]))
            {
                return false;
            }
        }

        return true;
    }
}

internal class SetupResultAccessor<T> : ISetupResultAccessor
{
    private readonly SetupResult<T> _setupResult;

    public SetupResultAccessor(SetupResult<T> setupResult)
    {
        _setupResult = setupResult;
    }

    public Func<object>? GetReturnValue => _setupResult.GetReturn;

    public int InvocationAmount => _setupResult.InvocationAmount;
    public Action? Callback => _setupResult.GetCallback;

    public bool Match(object[] args) => _setupResult.Match(args);
}

internal class SetupResultAccessor : ISetupResultAccessor
{
    private readonly SetupResult _setupResult;

    internal SetupResultAccessor(SetupResult setupResult)
    {
        _setupResult = setupResult;
    }

    public Func<object>? GetReturnValue => null;

    public int InvocationAmount => _setupResult.InvocationAmount;
    public Action? Callback => _setupResult.GetCallback;

    public bool Match(object[] args) => _setupResult.Match(args);
}
