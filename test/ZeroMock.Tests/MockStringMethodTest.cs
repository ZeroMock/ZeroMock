using T = ZeroMock.Tests.Utilities.PatchMe;

namespace ZeroMock.Tests;

[TestFixture]
public class MockStringMethodTest
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
        _sut.Setup(e => e.StringMethod());

        // Assert
        Assert.DoesNotThrow(() => obj.StringMethod());
    }

    [Test]
    public void CanReturn()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.StringMethod()).Returns("Success");

        // Assert
        Assert.That(obj.StringMethod(), Is.EqualTo("Success"));
    }

    [Test]
    public void CanCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.StringMethod()).Callback(() => callbackTriggered = true);

        // Act
        obj.StringMethod();

        // Assert
        Assert.True(callbackTriggered);
    }

    [Test]
    public void CanReturnAndCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.StringMethod()).Returns("Success").Callback(() => callbackTriggered = true);

        // Act
        var result = obj.StringMethod();

        // Assert
        Assert.That(result, Is.EqualTo("Success"));
        Assert.True(callbackTriggered);
    }

    [Test]
    public void CanVerify()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringMethod()).Returns("Success");

        // Act
        obj.StringMethod();

        // Assert
        _sut.Verify(e => e.StringMethod(), Times.Once());
    }

    [Test]
    public void CanMatchArgs()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringArgMethod(It.Is<string>(e => e.Contains("Potato")))).Returns("Success");

        // Act
        var result = obj.StringArgMethod("Potato");

        // Assert
        Assert.That(result, Is.EqualTo("Success"));
    }

    [Test]
    public void CanNotMatchArgs()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringArgMethod(It.Is<string>(e => e.Contains("Potato")))).Returns("Success");

        // Act
        var result = obj.StringArgMethod("PineApple");

        // Assert
        Assert.That(result, Is.Not.EqualTo("Success"));
    }

    [Test]
    public void CanVerifyNever()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringMethod()).Returns("Success");

        // Act
        obj.StringMethod();

        // Assert
        Assert.Throws<VerificationException>(() => _sut.Verify(e => e.StringMethod(), Times.Never()));
    }
}