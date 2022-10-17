using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockPropertyTest
{
    private Mock<TestClass> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<TestClass>();
    }

    [Test, Order(0)]
    public void NotSetup_PropertyIsDefault()
    {
        // Arrange
        var obj = _sut.Object;

        // Assert

        Assert.AreEqual(GetDefault<string>(), obj.Target);
    }

    [Test]
    public void CanSetup()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.Target);

        // Assert
        Assert.DoesNotThrow(() => _ = obj.Target);
    }

    [Test]
    public void CanReturn()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.Target).Returns("Success");

        // Assert
        Assert.That(obj.Target, Is.EqualTo("Success"));
    }

    [Test]
    public void CanCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.Target).Callback(() => callbackTriggered = true);

        // Act
        _ = obj.Target;

        // Assert
        Assert.That(callbackTriggered);
    }

    private T GetDefault<T>() => default!;

    private class TestClass
    {
        public string Target { get => PreventInline.Throw<string>(); set => PreventInline.Throw<string>(); }
    }
}