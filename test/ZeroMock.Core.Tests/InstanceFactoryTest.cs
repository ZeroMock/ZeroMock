namespace ZeroMock.Core.Tests;

[TestFixture]
public class InstanceFactoryTest
{
    class TestClass
    {
        public TestClass(string param1, int param2, int? param3, TestClass param4)
        {
            _ = param1;
            _ = param2;
            _ = param3;
            _ = param4;
        }
    }

    private InstanceFactory _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new InstanceFactory();
    }

    [Test]
    public void CanCallCreateNew()
    {
        // Act
        var result = _sut.CreateNew<TestClass>();

        // Assert
        Assert.IsNotNull(result);
    }
}