namespace ZeroMock;

public class SetupResult<T>
{
    private readonly ArgumentMatcher _condition;
    internal int InvocationAmount { get; private set; }
    internal Func<object>? GetReturn { get; private set; }
    internal Action? GetCallback { get; private set; }

    internal SetupResult(ArgumentMatcher condition)
    {
        _condition = condition;
    }

    public SetupResult<T> Returns(T result) => Returns(() => result);

    public SetupResult<T> Returns(Func<T> result)
    {
        Func<object> returnFunc = () => result();
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Callback(Action action)
    {
        GetCallback = action;
        return this;
    }

    public bool Match(object[] args)
    {
        if (_condition.Match(args))
        {
            InvocationAmount++;
            return true;
        }

        return false;
    }
}

public class SetupResult
{
    private readonly ArgumentMatcher _condition;

    internal int InvocationAmount { get; private set; }
    internal Action? GetCallback { get; private set; }

    internal SetupResult(ArgumentMatcher condition)
    {
        _condition = condition;
    }

    public SetupResult Callback(Action action)
    {
        GetCallback = action;
        return this;
    }

    public bool Match(object[] args)
    {
        if (_condition.Match(args))
        {
            InvocationAmount++;
            return true;
        }

        return false;
    }
}
