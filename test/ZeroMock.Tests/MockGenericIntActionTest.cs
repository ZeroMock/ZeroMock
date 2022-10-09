using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockGenericIntActionTest
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

    private class TestClass
    {
        public void Target<T>() => PreventInline.Throw<T>();
    }
}