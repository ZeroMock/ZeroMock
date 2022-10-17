namespace ZeroMock.Tests;
public class ConditionTest
{
    [Test]
    public void Condition_WhenMatching_IncrementsCounter()
    {
        // Arrange
        var foo = new Mock<TestClass>();
        var trueCount = 0;
        var falseCount = 0;
        foo.Setup(e => e.ReturnSameAsInput(It.Is<bool>(e => e == true))).Callback(() => trueCount++);
        foo.Setup(e => e.ReturnSameAsInput(It.Is<bool>(e => e == false))).Callback(() => falseCount++);

        // Act
        foo.Object.ReturnSameAsInput(true);
        var result = foo.Object.ReturnSameAsInput(false);

        // Assert
        Assert.That(trueCount, Is.EqualTo(1));
        Assert.That(falseCount, Is.EqualTo(1));
    }

    private class TestClass
    {
        public bool ReturnSameAsInput(bool param) => param;
    }
}
