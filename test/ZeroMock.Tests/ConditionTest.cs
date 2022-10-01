namespace ZeroMock.Tests;
public class ConditionTest
{
    [Test]
    public void CanReadCondition()
    {
        var foo = new Mock<TestClass>();
        foo.Setup(e => e.Example(It.Is<bool>(e => true)));
    }

    private class TestClass
    {
        public bool Example(bool param) => param;
    }
}
