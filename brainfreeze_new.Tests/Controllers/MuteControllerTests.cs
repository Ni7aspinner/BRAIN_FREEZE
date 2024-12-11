using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using brainfreeze_new.Server.Controllers;

namespace brainfreeze_new.Server.Tests.Controllers
{
    public class MuteControllerTests
    {
        [Fact]
        public void SetMuteState_WhenCalledWithTrue_ShouldReturnOkResult()
        {
            // Arrange
            var controller = new MuteController();
            var muteState = new MuteState { IsMuted = true };

            // Act
            var result = controller.SetMuteState(muteState);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObject = okResult.Value;

            // Use reflection to check the properties since it's an anonymous type
            var messageProperty = responseObject.GetType().GetProperty("message");
            var messageValue = messageProperty?.GetValue(responseObject) as string;
            
            Assert.Equal("Mute state updated", messageValue);

            // Verify the state was actually set by calling GetMuteState
            var getResult = controller.GetMuteState();
            var getOkResult = Assert.IsType<OkObjectResult>(getResult);
            var getResponseObject = getOkResult.Value;

            var isMutedProperty = getResponseObject.GetType().GetProperty("isMuted");
            var isMutedValue = (bool?)isMutedProperty?.GetValue(getResponseObject);
            
            Assert.True(isMutedValue);
        }

        [Fact]
        public void SetMuteState_WhenCalledWithFalse_ShouldReturnOkResult()
        {
            // Arrange
            var controller = new MuteController();
            var muteState = new MuteState { IsMuted = false };

            // Act
            var result = controller.SetMuteState(muteState);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseObject = okResult.Value;

            // Use reflection to check the properties since it's an anonymous type
            var messageProperty = responseObject.GetType().GetProperty("message");
            var messageValue = messageProperty?.GetValue(responseObject) as string;
            
            Assert.Equal("Mute state updated", messageValue);

            // Verify the state was actually set by calling GetMuteState
            var getResult = controller.GetMuteState();
            var getOkResult = Assert.IsType<OkObjectResult>(getResult);
            var getResponseObject = getOkResult.Value;

            var isMutedProperty = getResponseObject.GetType().GetProperty("isMuted");
            var isMutedValue = (bool?)isMutedProperty?.GetValue(getResponseObject);
            
            Assert.False(isMutedValue);
        }
    }
}