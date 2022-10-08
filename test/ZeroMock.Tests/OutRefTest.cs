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

        public void UpdateValue(ref int value)
        {
            value++;
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

    [Test]
    public void CanUpdateRefValueInReturn()
    {
        var sut = new Mock<RefClass>();

        sut.Setup(e => e.TryUpdateValue(ref It.Ref<int>.IsAny)).Returns((ref int param1) =>
        {
            param1 = 100;
            return true;
        });

        int test = 10;
        var result = sut.Object.TryUpdateValue(ref test);

        Assert.AreEqual(100, test);
    }

    [Test]
    public void CanUpdateRefValueInCallback()
    {
        var sut = new Mock<RefClass>();

        sut.Setup(e => e.UpdateValue(ref It.Ref<int>.IsAny)).Callback((ref int param1) =>
        {
            param1 = 100;
        });

        int test = 10;
        sut.Object.UpdateValue(ref test);

        Assert.AreEqual(100, test);
    }
}
