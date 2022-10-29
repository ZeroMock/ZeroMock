namespace ZeroMock;

public class SetupResult
{
    private readonly ArgumentMatcher _condition;

    internal int InvocationAmount { get; private set; }
    internal Action<dynamic[]>? GetCallback { get; private set; }
    internal Func<dynamic[], dynamic>? GetReturn { get; private set; }

    internal SetupResult(ArgumentMatcher condition)
    {
        _condition = condition;
    }

    public SetupResult Callback(Action action)
    {
        GetCallback = _ => action();
        return this;
    }

    public SetupResult Callback(Delegate action)
    {
        GetCallback = (args) => action.Method.Invoke(action.Target, args);
        return this;
    }

    internal SetupResult Returns(dynamic result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result;
        this.GetReturn = returnFunc;
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
