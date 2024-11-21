using System;
using System.Collections.Generic;
using Xunit;
using brainfreeze_new.Server;

namespace brainfreeze_new.Server.Tests
{
    public class DataTests
    {
        // Test the default initialization of CreatedList and ExpectedList
        [Fact]
        public void Data_Initialization_ListsAreEmpty()
        {
            // Arrange
            var data = new Data();

            // Act
            var createdList = data.CreatedList;
            var expectedList = data.ExpectedList;

            // Assert
            Assert.NotNull(createdList);
            Assert.Empty(createdList);

            Assert.NotNull(expectedList);
            Assert.Empty(expectedList);
        }

        // Test Level property when CreatedList is empty
        [Fact]
        public void Data_Level_WhenCreatedListEmpty_ReturnsZero()
        {
            // Arrange
            var data = new Data();

            // Act
            var level = data.Level;

            // Assert
            Assert.Equal(0, level);
        }

        // Test Level property when CreatedList has items
        [Fact]
        public void Data_Level_WhenCreatedListHasItems_ReturnsCorrectCount()
        {
            // Arrange
            var data = new Data();
            data.CreatedList.Add("Item1");
            data.CreatedList.Add("Item2");

            // Act
            var level = data.Level;

            // Assert
            Assert.Equal(2, level);
        }

        // Test setting and getting CreatedList
        [Fact]
        public void Data_SetCreatedList_ReturnsUpdatedList()
        {
            // Arrange
            var data = new Data();
            var newList = new List<object> { "Item1", "Item2" };

            // Act
            data.CreatedList = newList;

            // Assert
            Assert.Equal(newList, data.CreatedList);
        }

        // Test setting and getting ExpectedList
        [Fact]
        public void Data_SetExpectedList_ReturnsUpdatedList()
        {
            // Arrange
            var data = new Data();
            var expectedList = new List<object> { "Expected1", "Expected2" };

            // Act
            data.ExpectedList = expectedList;

            // Assert
            Assert.Equal(expectedList, data.ExpectedList);
        }

        // Test setting and getting Difficulty property
        [Fact]
        public void Data_SetDifficulty_ReturnsCorrectValue()
        {
            // Arrange
            var data = new Data();
            int difficulty = 5;

            // Act
            data.Difficulty = difficulty;

            // Assert
            Assert.Equal(difficulty, data.Difficulty);
        }
    }
}
