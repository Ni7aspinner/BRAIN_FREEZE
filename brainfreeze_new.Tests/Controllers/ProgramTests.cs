using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;
using brainfreeze_new.Server.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace brainfreeze_new.Tests.Controllers
{
    public class Programtests
    {

        [Fact]
        public void Services_RegisterCorsPolicy()
        {
            var services = new ServiceCollection();

            // Add required logging services
            services.AddLogging();

            // Register CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                    builder.WithOrigins("https://localhost:5173")
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });

            var serviceProvider = services.BuildServiceProvider();

            // Validate that the CORS service is resolved
            var corsService = serviceProvider.GetService<ICorsService>();
            Assert.NotNull(corsService);
        }


        [Fact]
        public void Configuration_LoadsConnectionString()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "ConnectionStrings:DevConnection", "Server=.;Database=TestDatabase;Trusted_Connection=True;" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var connectionString = configuration.GetConnectionString("DevConnection");
            Assert.Equal("Server=.;Database=TestDatabase;Trusted_Connection=True;", connectionString);
        }
    }
}
