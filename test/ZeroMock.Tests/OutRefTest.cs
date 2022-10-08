namespace ZeroMock.Tests;
public class OutRefTest
{
    private class RefClass
    {
        public bool TryUpdateValue(ref int value)
        {
            value++;
            return true;
        }
    }

    [Test]
    public void CanMockRef([Values] bool expected)
    {
        var sut = new Mock<RefClass>();

        sut.Setup(e => e.TryUpdateValue(ref It.Ref<int>.IsAny)).Returns(expected);

        int foo = 0;
        var result = sut.Object.TryUpdateValue(ref foo);

        Assert.AreEqual(expected, result);
    }

    [Test]
    public void CanGetRefValueInReturn()
    {
        var sut = new Mock<RefClass>();

        sut.Setup(e => e.TryUpdateValue(ref It.Ref<int>.IsAny)).Returns((ref int param1) =>
        {
            var result = param1 == 10;
            return result;
        });

        int test = 10;
        var result = sut.Object.TryUpdateValue(ref test);

        Assert.True(result);
    }
}
