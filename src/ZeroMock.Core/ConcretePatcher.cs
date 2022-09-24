using System.Runtime.CompilerServices;

namespace ZeroMock.Core;

//private static Harmony _harmony = new Harmony("com.zeromock.patch");


internal static class PatchTracker
{
    private static readonly ConditionalWeakTable<object, RefId> _ids = new ConditionalWeakTable<object, RefId>();

    private class RefId
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public static void Track(object obj)
    {
        _ids.Add(obj, new RefId());
    }

    public static bool IsTracked(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return _ids.TryGetValue(obj, out _);
    }
}
