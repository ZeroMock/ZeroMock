namespace ZeroMock.Core.Tests;

[TestFixture]
public class InstanceFactoryTest
{
    [Test]
    public void CanCreateComplexClass()
    {
        // Assert
        Assert.DoesNotThrow(() => InstanceFactory.CreateNew<InstanceFactoryTestComplexClass>());
    }

    [Test]
    public void CanCreateSimpleClass()
    {
        // Assert
        Assert.DoesNotThrow(() => InstanceFactory.CreateNew<InstanceFactoryTestSimpleClass>());
    }

    /// <summary>
    /// A class without default constructor
    /// </summary>
    class InstanceFactoryTestComplexClass
    {
        public InstanceFactoryTestComplexClass(string param1, int param2, int? param3, InstanceFactoryTestComplexClass param4)
        {
            _ = param1;
            _ = param2;
            _ = param3;
            _ = param4;
        }
    }

    /// <summary>
    /// A class with default constructor
    /// </summary>
    class InstanceFactoryTestSimpleClass
    {
    }
}