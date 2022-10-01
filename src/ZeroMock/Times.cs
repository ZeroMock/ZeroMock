namespace ZeroMock;

public class Times
{
    private static readonly Times _once = new(e => e == 1, "Once");
    public static Times Once() => _once;

    private static readonly Times _never = new(e => e == 0, "Never");
    public static Times Never() => _never;

    private static readonly Times _atLeastOnce = new(e => e >= 1, "At Least Once");
    public static Times AtLeastOnce() => _atLeastOnce;

    private static readonly Times _atMostOnce = new(e => e <= 1, "At Most Once");
    public static Times AtMostOnce() => _atMostOnce;

    public static Times Exactly(int amount) => new(e => e == amount, $"Exactly {amount}");

    public static Times AtLeast(int amount) => new(e => e >= amount, $"At Least {amount}");

    public static Times AtMost(int amount) => new(e => e <= amount, $"At Most {amount}");

    private readonly Func<int, bool> _operation;
    private readonly string _name;

    public Times(Func<int, bool> operation, string name)
    {
        _operation = operation;
        _name = name;
    }

    public bool Test(int count) => _operation(count);

    public override string ToString() => _name;
}
