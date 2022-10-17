using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockStringArgMethodTest
{
    private Mock<TestClass> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<TestClass>();
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
    public void CanMatchArgsInReturn()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringArgMethod(It.Is<string>(e => e.Contains("Potato")))).Returns((string param1) => param1);

        // Act
        var result = obj.StringArgMethod("Potatoooo");

        // Assert
        Assert.That(result, Is.EqualTo("Potatoooo"));
    }

    [Test]
    public void CanVerifyArgs()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.StringArgMethod("Potato")).Returns("Success");

        // Act
        var result = obj.StringArgMethod("Potato");

        // Assert
        _sut.Verify(e => e.StringArgMethod("Potato"), Times.Once());
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

    private class TestClass
    {
        public string StringArgMethod(string param1) => PreventInline.Throw<string>();
    }
}