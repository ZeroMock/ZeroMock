namespace ZeroMock.Tests;

[TestFixture]
public static class MockOfTest
{
    [Test]
    public static void CanCallOf()
    {
        // Act
        var result = Mock.Of<TestClass>();
        var mock = Mock.Get(result);

        // Assert
        Assert.That(result, Is.SameAs(mock.Object));
    }

    private class TestClass
    { }
}