using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockGenericIntFuncTest
{
    private Mock<TestClass> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<TestClass>();
    }

    [Test, Order(0)]
    public void NotSetupGeneric_CallsOriginalImplementation()
    {
        // Arrange
        var obj = _sut.Object;

        // Assert
        Assert.Throws<NotPatchedException>(() => obj.Target<int>());
    }

    [Test]
    public void CanSetup()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.Target<int>());

        // Assert
        Assert.DoesNotThrow(() => obj.Target<int>());
    }

    [Test]
    public void CanReturn()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.Target<int>()).Returns(123);

        // Assert
        Assert.That(obj.Target<int>(), Is.EqualTo(123));
    }

    [Test]
    public void CanCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.Target<int>()).Callback(() => callbackTriggered = true);

        // Act
        obj.Target<int>();

        // Assert
        Assert.That(callbackTriggered);
    }

    [Test]
    public void CanThrow()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.Target<int>()).Throws(new Exception());

        // Assert
        Assert.Throws<Exception>(() => obj.Target<int>());
    }

    [Test]
    public void CanThrow1()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.Target<int>()).Throws<Exception>();

        // Assert
        Assert.Throws<Exception>(() => obj.Target<int>());
    }

    [Test]
    public void CanThrow2()
    {
        // Arrange
        var obj = _sut.Object;
        _sut.Setup(e => e.Target<int>()).Throws(() => new Exception());

        // Assert
        Assert.Throws<Exception>(() => obj.Target<int>());
    }

    [Test]
    public void CanReturnAndCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.Target<int>()).Returns(123).Callback(() => callbackTriggered = true);

        // Act
        var result = obj.Target<int>();

        // Assert
        Assert.That(result, Is.EqualTo(123));
        Assert.That(callbackTriggered);
    }

    private class TestClass
    {
        public T Target<T>() => PreventInline.Throw<T>();
    }
}