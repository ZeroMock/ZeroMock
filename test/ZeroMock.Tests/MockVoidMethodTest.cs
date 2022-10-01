using T = ZeroMock.Tests.Utilities.PatchMe;

namespace ZeroMock.Tests;

[TestFixture]
public class MockVoidMethodTest
{
    private Mock<T> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<T>();
    }

    [Test]
    public void CanSetup()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.VoidMethod());

        // Assert
        Assert.DoesNotThrow(() => obj.VoidMethod());
    }

    [Test]
    public void CanSetupArgs()
    {
        // Arrange
        var obj = _sut.Object;
        int callbackCalled = 0;
        _sut.Setup(e => e.VoidArgMethod(It.Is<string>(e => e.Contains("Hello")))).Callback(() => callbackCalled++);

        // Act
        _sut.Object.VoidArgMethod("Hello");
        _sut.Object.VoidArgMethod("GoodBye");

        // Assert
        Assert.That(callbackCalled, Is.EqualTo(1));
    }

    [Test]
    public void CanSetupArgsConstant()
    {
        // Arrange
        var obj = _sut.Object;
        int callbackCalled = 0;
        _sut.Setup(e => e.VoidArgMethod("Hello")).Callback(() => callbackCalled++);

        // Act
        _sut.Object.VoidArgMethod("Hello");
        _sut.Object.VoidArgMethod("GoodBye");

        // Assert
        Assert.That(callbackCalled, Is.EqualTo(1));
    }

    [Test]
    public void CanCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.VoidMethod()).Callback(() => callbackTriggered = true);

        // Act
        obj.VoidMethod();

        // Assert
        Assert.True(callbackTriggered);
    }
}