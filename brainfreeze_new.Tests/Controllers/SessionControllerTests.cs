using System;
using System.Text.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using brainfreeze_new.Server.Controllers;
using System.Reflection;

namespace brainfreeze_new.Tests.Controllers
{
    public class SessionControllerTests
    {
        [Fact]
        public void GetNewSessionId_ReturnsOkResult()
        {
            // Arrange
            var controller = new SessionController();

            // Act
            var result = controller.GetNewSessionId();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

   }
}