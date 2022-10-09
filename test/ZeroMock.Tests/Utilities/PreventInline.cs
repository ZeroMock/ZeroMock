using System.Diagnostics.CodeAnalysis;

namespace ZeroMock.Tests.Utilities;
internal static class PreventInline
{
    [DoesNotReturn]
    public static void Throw()
    {
        var inlineWorkaround = 2 % 1 == 0;
        if (inlineWorkaround)
        {
            throw new NotPatchedException();
        }
    }

    [DoesNotReturn]
    public static T Throw<T>()
    {
        var inlineWorkaround = 2 % 1 == 0;
        if (inlineWorkaround)
        {
            throw new NotPatchedException();
        }

#pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return.
        return default!;
#pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return.
    }
}
