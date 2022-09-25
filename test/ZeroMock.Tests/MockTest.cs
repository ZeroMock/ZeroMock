using T = ZeroMock.Tests.Utilities.PatchMe;

namespace ZeroMock.Tests
{
    [TestFixture]
    public class MockTest
    {
        private Mock<T> _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Mock<T>();
        }

        [Test]
        public void CanCallSetup()
        {
            // Arrange
            var obj = _sut.Object;

            // Act
            _sut.Setup(e => e.StringMethod());

            // Assert
            Assert.DoesNotThrow(() => obj.StringMethod());
        }


        [Test]
        public void CanCallReturn()
        {
            // Arrange
            var obj = _sut.Object;

            // Act
            _sut.Setup(e => e.StringMethod()).Returns("Success");

            // Assert
            Assert.That(obj.StringMethod(), Is.EqualTo("Success"));
        }
    }
}