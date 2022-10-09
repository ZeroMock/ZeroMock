using ZeroMock.Tests.Utilities;

namespace ZeroMock.Tests;

[TestFixture]
public class MockVoidArgMethodTest
{
    private Mock<TestClass> _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new Mock<TestClass>();
    }

    [Test]
    public void CanSetupArgs()
    {
        // Arrange
        var obj = _sut.Object;
        int callbackCalled = 0;
        _sut.Setup(e => e.VoidArgMethod(It.Is<string>(e => e.Contains("Hello")))).Callback(() => callbackCalled++);

        // Act
        _sut.Object.VoidArgMethod("Hello");
        _sut.Object.VoidArgMethod("GoodBye");

        // Assert
        Assert.That(callbackCalled, Is.EqualTo(1));
    }

    [Test]
    public void CanSetupArgsConstant()
    {
        // Arrange
        var obj = _sut.Object;
        int callbackCalled = 0;
        _sut.Setup(e => e.VoidArgMethod("Hello")).Callback(() => callbackCalled++);

        // Act
        _sut.Object.VoidArgMethod("Hello");
        _sut.Object.VoidArgMethod("GoodBye");

        // Assert
        Assert.That(callbackCalled, Is.EqualTo(1));
    }

    private class TestClass
    {
        public void VoidArgMethod(string param) => PreventInline.Throw();
    }
}