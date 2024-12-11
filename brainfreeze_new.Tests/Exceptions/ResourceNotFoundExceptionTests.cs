using brainfreeze_new.Server.Exceptions;
using System;
using Xunit;

namespace brainfreeze_new.Tests
{
    public class ResourceNotFoundExceptionTests
    {
        [Fact]
        public void Constructor_ShouldSetMessageProperty()
        {
            // Arrange
            string expectedMessage = "Resource not found";

            // Act
            var exception = new ResourceNotFoundException(expectedMessage);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ShouldBeDerivedFromException()
        {
            // Act
            var exception = new ResourceNotFoundException("Some message");

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}
