namespace ZeroMock;

public class TimesOperation
{
    private readonly Func<int, bool> _operation;
    private readonly string _name;

    public TimesOperation(Func<int, bool> operation, string name)
    {
        _operation = operation;
        _name = name;
    }

    public bool Test(int count) => _operation(count);

    public override string ToString() => _name;
}
