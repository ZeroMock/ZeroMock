using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockStringMethodTest
{
    private Mock<TestClass> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<TestClass>();
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
    public void CanVerifyTwice()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringMethod()).Returns("Success");

        // Act
        obj.StringMethod();
        obj.StringMethod();

        // Assert
        _sut.Verify(e => e.StringMethod(), Times.Exactly(2));
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

    private class TestClass
    {
        public string StringMethod() => PreventInline.Throw<string>();
    }
}