using System.Threading;
using System.Threading.Tasks;
using Hero.Api.Controllers;
using Hero.Business.Model;
using Hero.Business.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Hero.Api.Test.Hero
{
    public class HeroControllerTest : BaseTest
    {
        public HeroControllerTest(CompositionRootFixture fixture, ITestOutputHelper outputHelper) : base(fixture,
            outputHelper)
        {
            this.mockService = new Mock<IHeroService>();
            this.controller = new HeroController(
                this.LoggerFactory.CreateLogger<HeroController>(),
                this.mockService.Object);
        }

        private readonly Mock<IHeroService> mockService;
        private readonly HeroController controller;

        [Fact]
        public async Task GetAsync()
        {
            //Arrange
            var models = new[]
            {
                new HeroModel
                {
                    Pseudonym = "Batman"
                }
            };
            this.mockService
                .Setup(x => x.GetAllAsync(CancellationToken.None))
                .Returns(() => Task.FromResult(models));

            // Act
            var actionResult = await this.controller.Get();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.Equal(models, jsonResult.Value);
            var responseValue = Assert.IsType<HeroModel[]>(jsonResult.Value);
            Assert.Equal(models.Length, responseValue.Length);
            Assert.Equal(models[0].Pseudonym, responseValue[0].Pseudonym);
        }
    }
}