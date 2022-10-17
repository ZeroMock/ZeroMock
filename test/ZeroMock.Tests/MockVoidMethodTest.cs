using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockVoidMethodTest
{
    private Mock<TestClass> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<TestClass>();
    }

    [Test]
    public void CanVerify()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.VoidMethod());

        // Assert
        Assert.DoesNotThrow(() => obj.VoidMethod());
        _sut.Verify(e => e.VoidMethod(), Times.Once());
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

    private class TestClass
    {
        public void VoidMethod() => PreventInline.Throw();
    }
}