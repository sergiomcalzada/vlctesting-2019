using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Hero.Api.Controllers;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Hero.Api.WebTest.Redis
{
    public class RedisControllerTest : BaseTest
    {

        public RedisControllerTest(CompositionRootFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {

        }

        [Fact]
        [ResetDatabase]
        public async Task Successful_Create_Cache_Entry()
        {
            //Arrange

            var model = new RedisController.RedisModel { Value = "TestingValue" };
            var request = this.Server
                .CreateHttpApiRequest<RedisController>(
                    controller => controller.Post(model, CancellationToken.None))
                .WithIdentity(Identities.TestUser);

            // Act
            var responseMessage = await request.PostAsync();

            // Assert
            Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);

            var json = await responseMessage.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<dynamic>(json);

            Assert.NotNull(responseModel);
            Assert.NotNull(responseModel.key);

        }


        [Fact]
        [ResetDatabase]
        public async Task Successful_Create_And_Get_CacheEntry()
        {
            //Arrange
            var createModel = new RedisController.RedisModel { Value = "TestingValue" };
            var createRequest = this.Server
                .CreateHttpApiRequest<RedisController>(
                    controller => controller.Post(createModel, CancellationToken.None))
                .WithIdentity(Identities.TestUser);

            var createMessage = await createRequest.PostAsync();
            Assert.Equal(HttpStatusCode.Created, createMessage.StatusCode);

            var createJson = await createMessage.Content.ReadAsStringAsync();
            var createMessageModel = JsonConvert.DeserializeObject<dynamic>(createJson);
            var key = createMessageModel.key.ToString() as string;


            var request = this.Server
                .CreateHttpApiRequest<RedisController>(
                    controller => controller.Get(key, CancellationToken.None))
                .WithIdentity(Identities.TestUser);
            // Act

            var responseMessage = await request.GetAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);

            var json = await responseMessage.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<RedisController.RedisModel>(json);
            Assert.Equal(createModel.Value, responseModel.Value);

        }





    }
}
