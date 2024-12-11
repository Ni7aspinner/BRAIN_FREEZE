using Xunit;
using brainfreeze_new.Server.Exceptions;
using System;

namespace brainfreeze_new.Tests.Exceptions
{
    public class ResourceNotFoundExceptionTests
    {
        [Fact]
        public void ResourceNotFoundException_ShouldStoreMessageCorrectly()
        {
            // Arrange
            string testMessage = "Resource not found: User ID 123";

            // Act
            var exception = new ResourceNotFoundException(testMessage);

            // Assert
            Assert.Equal(testMessage, exception.Message);
        }

        [Fact]
        public void ResourceNotFoundException_ShouldBeInstanceOfException()
        {
            // Arrange
            string testMessage = "Another test message";

            // Act
            var exception = new ResourceNotFoundException(testMessage);

            // Assert
            Assert.IsType<ResourceNotFoundException>(exception);
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}
