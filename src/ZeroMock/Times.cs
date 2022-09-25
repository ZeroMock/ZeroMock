namespace ZeroMock;

public static class Times
{
    private static readonly TimesOperation _once = new(e => e == 1, "Once");
    public static TimesOperation Once() => _once;

    private static readonly TimesOperation _never = new(e => e == 0, "Never");
    public static TimesOperation Never() => _never;

    private static readonly TimesOperation _atLeastOnce = new(e => e >= 1, "At Least Once");
    public static TimesOperation AtLeastOnce() => _atLeastOnce;

    private static readonly TimesOperation _atMostOnce = new(e => e <= 1, "At Most Once");
    public static TimesOperation AtMostOnce() => _atMostOnce;

    public static TimesOperation Exactly(int amount) => new(e => e == amount, $"Exactly {amount}");

    public static TimesOperation AtLeast(int amount) => new(e => e >= amount, $"At Least {amount}");

    public static TimesOperation AtMost(int amount) => new(e => e <= amount, $"At Most {amount}");
}
