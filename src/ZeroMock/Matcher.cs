using System.Linq.Expressions;

namespace ZeroMock;

/// <summary>
/// The parameter condition passed as argument during setup
/// </summary>
internal class Condition
{
    private readonly dynamic _func;
    public Condition(UnaryExpression ue)
    {
        var operand = (dynamic)ue.Operand;
        _func = operand.Compile();
    }

    public Condition(ConstantExpression ce)
    {
        Func<dynamic, bool> func = (e) => e == ce.Value;
        _func = func;
    }

    public Condition(dynamic func)
    {
        _func = func;
    }

    public bool Match(dynamic input)
    {
        var result = (bool)_func.Invoke(input);
        return result;
    }
}
