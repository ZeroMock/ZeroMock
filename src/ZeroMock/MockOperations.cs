using System.Reflection;

namespace ZeroMock;

internal class MockOperations
{
    public Dictionary<MethodBase, IMethodExecution> MethodOverrides { get; } = new();

    public Action? Callback { get; set; }
}
