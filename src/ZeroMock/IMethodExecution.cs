namespace ZeroMock;

internal interface IMethodExecution
{
    int InvocationAmount { get; }
    Func<object> Invoke { get; }
}