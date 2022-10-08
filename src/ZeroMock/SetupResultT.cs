namespace ZeroMock;

public class SetupResult<T>
{
    private readonly ArgumentMatcher _condition;
    internal int InvocationAmount { get; private set; }
    internal Func<dynamic[], dynamic>? GetReturn { get; private set; }
    internal Action<dynamic[]>? GetCallback { get; private set; }

    internal SetupResult(ArgumentMatcher condition)
    {
        _condition = condition;
    }

    public SetupResult<T> Returns(Delegate result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result.Method.Invoke(result.Target, args);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1>(Func<T1, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2>(Func<T1, T2, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3>(Func<T1, T2, T3, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4>(Func<T1, T2, T3, T4, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3], args[4]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3], args[4], args[5]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> result)
    {
        Func<dynamic[], dynamic> returnFunc = (args) => result(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Returns(T result) => Returns(() => result);

    public SetupResult<T> Returns(Func<T> result)
    {
        Func<object[], dynamic> returnFunc = _ => result()!;
        this.GetReturn = returnFunc;
        return this;
    }

    public SetupResult<T> Callback(Action action)
    {
        GetCallback = _ => action();
        return this;
    }

    public SetupResult<T> Callback(Delegate action)
    {
        GetCallback = (args) => action.Method.Invoke(action.Target, args);
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
