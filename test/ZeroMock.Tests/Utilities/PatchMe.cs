namespace ZeroMock.Tests.Utilities;

#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// A class that throws <see ref="NotPatchedException"/> if not patched
/// </summary>
class PatchMe
{
    public PatchMe(string param1, int param2, int? param3, PatchMe param4)
    {
        Throw();
    }

    public void VoidMethod() => Throw();

    public string StringMethod() => Throw<string>();
    public string StringArgMethod(string arg) => Throw<string>();

    public int IntMethod() => Throw<int>();

    public int? NullableIntMethod() => Throw<int?>();

    public T GenericMethod<T>() => Throw<T>();

    //public T GenericClassMethod<T>() where T : class => Throw<T>();

    public string StringProp { get => Throw<string>(); set => Throw<string>(); }

    public string StringPropGetOnly { get => Throw<string>(); }


    public string StringField = "NotPatched";

    //[DoesNotReturn]
    public static T Throw<T>()
    {
        var inlineWorkaround = 2 % 1 == 0;
        if (inlineWorkaround)
        {
            throw new NotPatchedException();
        }

        return default!;
    }

    //   [DoesNotReturn]
    public static void Throw()
    {
        var inlineWorkaround = 2 % 1 == 0;
        if (inlineWorkaround)
        {
            throw new NotPatchedException();
        }
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore CA1822 // Mark members as static