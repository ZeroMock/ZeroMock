using T = ZeroMock.Tests.Utilities.PatchMe;

namespace ZeroMock.Tests;

[TestFixture]
public class MockGenericIntMethodTest
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
        _sut.Setup(e => e.GenericMethod<int>());

        // Assert
        Assert.DoesNotThrow(() => obj.GenericMethod<int>());
    }

    [Test]
    public void CanReturn()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.GenericMethod<int>()).Returns(123);

        // Assert
        Assert.That(obj.GenericMethod<int>(), Is.EqualTo(123));
    }

    [Test]
    public void CanCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.GenericMethod<int>()).Callback(() => callbackTriggered = true);

        // Act
        obj.GenericMethod<int>();

        // Assert
        Assert.True(callbackTriggered);
    }

    [Test]
    public void CanReturnAndCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.GenericMethod<int>()).Returns(123).Callback(() => callbackTriggered = true);

        // Act
        var result = obj.GenericMethod<int>();

        // Assert
        Assert.That(result, Is.EqualTo(123));
        Assert.True(callbackTriggered);
    }
}