using System.Reflection;

namespace ZeroMock.Core;

internal static class BingingFlagsHelper
{
    /// <summary>
    /// BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
    /// </summary>
    public const BindingFlags InstanceAll = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
}
