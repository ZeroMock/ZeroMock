namespace ZeroMock;

internal enum PatchState
{
    /// <summary>
    /// Object has not been registered as mock
    /// </summary>
    NotTracked,
    /// <summary>
    /// Method has no setup operation associated
    /// </summary>
    NotSetup,
    /// <summary>
    /// Method has a setup operation associated
    /// </summary>
    Setup
}