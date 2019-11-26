using Hero.Api.Controllers;
using Hero.Api.WebTest;
using Hero.Business.Model;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Hero.Api.Test
{
    public class HeroControllerTest : BaseTest
    {

        public HeroControllerTest(CompositionRootFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {

        }

        [Fact]
        [ResetDatabase]
        public async Task UnauthorizedHeroRequestWhenAnonymous()
        {
            //Arrange
            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Get(CancellationToken.None));

            // Act
            var response = await request.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        }

        [Fact]
        [ResetDatabase]
        public async Task SuccessfulHeroGet()
        {
            //Arrange

            var hero = await this.WithHeroInTheDatabase();

            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Get(CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);

            var json = await responseMessage.Content.ReadAsStringAsync();
            var responseModels = JsonConvert.DeserializeObject<HeroModel[]>(json);

            Assert.Single(responseModels);
            Assert.Equal(hero.Id, responseModels[0].Id);

        }

        [Fact]
        [ResetDatabase]
        public async Task SuccessfulSingleHeroGetById()
        {
            //Arrange
            var hero = await this.WithHeroInTheDatabase();

            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Get(hero.Id, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);

            var json = await responseMessage.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<HeroModel>(json);
            Assert.Equal(hero.Id, responseModel.Id);

        }


        [Fact]
        [ResetDatabase]
        public async Task NotFoundHeroGetById()
        {
            //Arrange
            await this.WithHeroInTheDatabase();

            const int idNotExistent = int.MinValue;

            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Get(idNotExistent, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);

        }

        [Fact]
        [ResetDatabase]
        public async Task SuccessfulHeroCreation()
        {
            //Arrange
            var heroModel = new HeroModel
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                Pseudonym = "Batman"
            };
            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Post(heroModel, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.PostAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
            Assert.NotNull(responseMessage.Headers.Location);

            var json = await responseMessage.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<HeroModel>(json);

            Assert.True(responseModel.Id > 0);
            Assert.Equal(heroModel.FirstName, responseModel.FirstName);
            Assert.Equal(heroModel.LastName, responseModel.LastName);
            Assert.Equal(heroModel.Pseudonym, responseModel.Pseudonym);

        }


        [Fact]
        [ResetDatabase]
        public async Task SuccessfulHeroUpdate()
        {
            //Arrange
            var hero = await this.WithHeroInTheDatabase();

            var modifiedPseudonym = hero.Pseudonym + "TestContentModified";

            var heroModel = new HeroModel
            {
                Id = hero.Id,
                FirstName = hero.FirstName,
                LastName = hero.LastName,
                Pseudonym = modifiedPseudonym
            };
            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Put(heroModel.Id, heroModel, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.SendAsync(HttpMethod.Put.Method);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            
        }


        [Fact]
        [ResetDatabase]
        public async Task NotFoundHeroUpdate()
        {
            //Arrange
            var hero = await this.WithHeroInTheDatabase();

            var modifiedPseudonym = hero.Pseudonym + "TestContentModified";
            const int idNotExistent = int.MinValue;

            var heroModel = new HeroModel
            {
                Id = idNotExistent,
                FirstName = hero.FirstName,
                LastName = hero.LastName,
                Pseudonym = modifiedPseudonym
            };
            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Put(heroModel.Id, heroModel, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.SendAsync(HttpMethod.Put.Method);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);


        }

        [Fact]
        [ResetDatabase]
        public async Task SuccessfulHeroDelete()
        {
            //Arrange
            var hero = await this.WithHeroInTheDatabase();

            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Delete(hero.Id, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            var requestCheck = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Get(CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.SendAsync(HttpMethod.Delete.Method);
            var responseRep = await requestCheck.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);

            Assert.Equal(HttpStatusCode.OK, responseRep.StatusCode);
            var json = await responseRep.Content.ReadAsStringAsync();
            var responseModels = JsonConvert.DeserializeObject<HeroModel[]>(json);
            Assert.Empty(responseModels);
        }


        [Fact]
        [ResetDatabase]
        public async Task NotFoundHeroDelete()
        {
            //Arrange
            await this.WithHeroInTheDatabase();
            const int idNotExistent = int.MinValue;

            var request = this.Server
                                .CreateHttpApiRequest<HeroController>(
                                    controller => controller.Delete(idNotExistent, CancellationToken.None))
                                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.SendAsync(HttpMethod.Delete.Method);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
        }
    }
}
