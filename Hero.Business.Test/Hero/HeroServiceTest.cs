using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hero.Business.Model;
using Hero.Business.Repository;
using Hero.Business.Service;
using Hero.Business.Test.Support;
using Hero.Domain.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Hero.Business.Test
{
    public class HeroServiceTest : BaseTest
    {
        private readonly IHeroService service;
        private readonly Mock<IEntityRepository> entityRepositoryMoq;

        public HeroServiceTest(CompositionRootFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {
            this.entityRepositoryMoq = new Mock<IEntityRepository>();


            this.service = new HeroService(
                this.LoggerFactory.CreateLogger<HeroService>(),
                this.entityRepositoryMoq.Object,
                new TestQueryableInvoker());
        }


        [Fact]
        public async Task GetAllTest()
        {
            //Arrange
            var entities = new[] {
                new Domain.Entity.Hero()
                    {
                        FirstName = "Bruce",
                        LastName = "Wayne",
                        Pseudonym = "Batman"
                    }
                };
            this.entityRepositoryMoq
               .Setup(x => x.Query<Domain.Entity.Hero>())
               .Returns(() => entities.AsQueryable());

            //Act
            var models = await this.service.GetAllAsync(CancellationToken.None);

            //Assert
            Assert.NotNull(models);
            Assert.Equal(entities.Count(), models.Count());
            Assert.IsAssignableFrom<IEnumerable<HeroModel>>(models);

            Assert.Equal(entities[0].Pseudonym, models[0].Pseudonym);

        }
    }
}
