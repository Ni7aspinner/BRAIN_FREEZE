using System;
using Xunit;

namespace brainfreeze_new.Server.Controllers.Tests
{
    public class WordsExtendedControllerTests
    {
        [Fact]
        public void Expected_ReturnsExpectedString()
        {
            // Arrange
            DateTime testDate = DateTime.Now;

            // Act
            string result = testDate.Expected();

            // Assert
            Assert.Equal("\nExpectedList: ", result);
        }

        [Fact]
        public void Created_ReturnsCreatedString()
        {
            // Arrange
            DateTime testDate = DateTime.Now;

            // Act
            string result = testDate.Created();

            // Assert
            Assert.Equal("CreatedList: ", result);
        }

        [Fact]
        public void Space_ReturnsSpaceString()
        {
            // Arrange
            DateTime testDate = DateTime.Now;

            // Act
            string result = testDate.Space();

            // Assert
            Assert.Equal(" ", result);
        }
    }
}
