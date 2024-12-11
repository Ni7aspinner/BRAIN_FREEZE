using brainfreeze_new.Server.Exceptions; // Import the existing exception
using Xunit;

namespace brainfreeze_new.Tests.Exceptions
{
    public class RNFExceptionTests
    {
        [Fact]
        public void Constructor_SetsMessageCorrectly()
        {
            var expectedMessage = "The resource was not found.";
            var exception = new ResourceNotFoundException(expectedMessage);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void Exception_InheritsFrom_BaseException()
        {
            var exception = new ResourceNotFoundException("Test message");

            Assert.IsAssignableFrom<Exception>(exception);
        }

        [Fact]
        public void Exception_CanBeThrownAndCaught()
        {
            var expectedMessage = "Resource not found!";

            try
            {
                throw new ResourceNotFoundException(expectedMessage);
            }
            catch (ResourceNotFoundException ex)
            {
                Assert.Equal(expectedMessage, ex.Message);
            }
        }
    }
}
