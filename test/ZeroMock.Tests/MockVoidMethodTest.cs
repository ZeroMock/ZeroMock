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