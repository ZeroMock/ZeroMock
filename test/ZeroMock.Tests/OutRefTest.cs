namespace ZeroMock.Tests;
public class OutRefTest
{
    private class TestClass
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
        var sut = new Mock<TestClass>();

        sut.Setup(e => e.TryUpdateValue(ref It.Ref<int>.IsAny)).Returns(expected);

        int foo = 0;
        var result = sut.Object.TryUpdateValue(ref foo);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void CanGetRefValueInReturn()
    {
        var sut = new Mock<TestClass>();

        sut.Setup(e => e.TryUpdateValue(ref It.Ref<int>.IsAny)).Returns((ref int param1) =>
        {
            var result = param1 == 10;
            return result;
        });

        int test = 10;
        var result = sut.Object.TryUpdateValue(ref test);

        Assert.That(result);
    }

    [Test]
    public void CanUpdateRefValueInReturn()
    {
        var sut = new Mock<TestClass>();

        sut.Setup(e => e.TryUpdateValue(ref It.Ref<int>.IsAny)).Returns((ref int param1) =>
        {
            param1 = 100;
            return true;
        });

        int test = 10;
        var result = sut.Object.TryUpdateValue(ref test);

        Assert.That(test, Is.EqualTo(100));
    }

    [Test]
    public void CanUpdateRefValueInCallback()
    {
        var sut = new Mock<TestClass>();

        sut.Setup(e => e.UpdateValue(ref It.Ref<int>.IsAny)).Callback((ref int param1) =>
        {
            param1 = 100;
        });

        int test = 10;
        sut.Object.UpdateValue(ref test);

        Assert.That(test, Is.EqualTo(100));
    }
}
