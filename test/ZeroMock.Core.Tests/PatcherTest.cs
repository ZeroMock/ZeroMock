using ZeroMock.Core.Tests.Utilities;

namespace ZeroMock.Core.Tests;

[TestFixture]
public class PatcherTest
{
    private PatchMe _sut;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Patcher.Prepare<PatchMe>();
    }

    [SetUp]
    public void SetUp()
    {
        _sut = InstanceFactory.CreateNew<PatchMe>();
    }

    [Test]
    public void CanPatchVoidMethod()
    {
        // Assert
        Assert.DoesNotThrow(() => _sut.VoidMethod());
    }

    [Test]
    public void CanPatchStringMethod()
    {
        // Assert
        Assert.DoesNotThrow(() => _sut.StringMethod());
    }

    [Test]
    public void CanPatchIntMethod()
    {
        // Assert
        Assert.DoesNotThrow(() => _sut.IntMethod());
    }

    [Test]
    public void CanPatchNullableIntMethod()
    {
        // Assert
        Assert.DoesNotThrow(() => _sut.NullableIntMethod());
    }

    [Test]
    public void CanPatchStringProp()
    {
        // Assert
        Assert.DoesNotThrow(() => _ = _sut.StringProp);
    }

    [Test]
    public void CanPatchStringPropGetOnly()
    {
        // Assert
        Assert.DoesNotThrow(() => _ = _sut.StringPropGetOnly);
    }

    [Test]
    public void CanPatchStringField()
    {
        // Assert
        Assert.IsEmpty(_sut.StringField);
    }
}