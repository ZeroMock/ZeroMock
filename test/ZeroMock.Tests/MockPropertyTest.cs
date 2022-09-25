using T = ZeroMock.Tests.Utilities.PatchMe;

namespace ZeroMock.Tests;

[TestFixture]
public class MockPropertyTest
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
        _sut.Setup(e => e.StringProp);

        // Assert
        Assert.DoesNotThrow(() => _ = obj.StringProp);
    }

    [Test]
    public void CanReturn()
    {
        // Arrange
        var obj = _sut.Object;

        // Act
        _sut.Setup(e => e.StringProp).Returns("Success");

        // Assert
        Assert.AreEqual(obj.StringProp, "Success");
    }

    [Test]
    public void CanCallback()
    {
        // Arrange
        var obj = _sut.Object;
        var callbackTriggered = false;
        _sut.Setup(e => e.StringProp).Callback(() => callbackTriggered = true);

        // Act
        _ = obj.StringProp;

        // Assert
        Assert.True(callbackTriggered);
    }
}