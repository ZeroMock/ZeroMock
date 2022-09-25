namespace ZeroMock;

internal class MethodExecution<T> : IMethodExecution
{
    public MethodExecution(Func<T> func)
    {
        object Invoke()
        {
            InvocationAmount++;
            return func()!;
        }

        this.Invoke = Invoke;
    }

    public Func<object> Invoke { get; }

    public int InvocationAmount { get; private set; }
}
