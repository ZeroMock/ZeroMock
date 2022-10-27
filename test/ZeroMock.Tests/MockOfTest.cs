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

    [Test]
    public static void CanSetProperty()
    {
        // Act
        var result = Mock.Of<TestClass>();
        result.Test = "Test";

        // Assert
        Assert.That(result.Test, Is.EqualTo("Test"));
    }

    [Test]
    public static void CanSetPropertyInCtor()
    {
        // Act
        var result = Mock.Of<TestClass>(e => e.Test == "Test");

        // Assert
        Assert.That(result.Test, Is.EqualTo("Test"));
    }

    private class TestClass
    {
        public string Test { get; set; }
    }
}