namespace ZeroMock;

public class PatchFailedException : Exception
{
    public PatchFailedException(string message, Exception e) : base(message, e)
    {
    }
}
