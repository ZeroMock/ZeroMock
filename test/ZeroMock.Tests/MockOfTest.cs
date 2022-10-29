using ZeroMock.Tests.Utilities;

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
        result.SomeProperty = "Test";

        // Assert
        Assert.That(result.SomeProperty, Is.EqualTo("Test"));
    }

    [Test]
    public static void CanSetPropertyInCtor()
    {
        // Act
        var result = Mock.Of<TestClass>(e => e.SomeProperty == "Test");

        // Assert
        Assert.That(result.SomeProperty, Is.EqualTo("Test"));
    }


    [Test]
    public static void CanSetMethodInCtor()
    {
        // Act
        var result = Mock.Of<TestClass>(e => e.SomeMethod() == "Test");

        // Assert
        Assert.That(result.SomeMethod(), Is.EqualTo("Test"));
    }

    [Test]
    public static void CanSetMethodAndPropertyInCtor()
    {
        // Act
        var result = Mock.Of<TestClass>(e => e.SomeProperty == "Test" && e.SomeMethod() == "Test");

        // Assert
        Assert.That(result.SomeProperty, Is.EqualTo("Test"));
        Assert.That(result.SomeMethod(), Is.EqualTo("Test"));
    }

    [Test]
    public static void CanSetMethodAndPropertyAndPropertyAgainInCtor()
    {
        // Act
        var result = Mock.Of<TestClass>(e => e.SomeProperty == "Bad" && e.SomeMethod() == "Test" && e.SomeProperty == "Test");

        // Assert
        Assert.That(result.SomeProperty, Is.EqualTo("Test"));
        Assert.That(result.SomeMethod(), Is.EqualTo("Test"));
    }

    private class TestClass
    {
        public string SomeProperty { get; set; }

        public string SomeMethod() => PreventInline.Throw<string>();
    }
}