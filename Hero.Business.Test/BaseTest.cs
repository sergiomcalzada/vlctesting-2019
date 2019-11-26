using Hero.Business.Test.Support;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Hero.Business.Test
{
    public class BaseTest : IClassFixture<CompositionRootFixture>
    {
        protected readonly CompositionRootFixture Fixture;

        protected readonly ILoggerFactory LoggerFactory;

        public BaseTest(CompositionRootFixture fixture, ITestOutputHelper outputHelper)
        {
            this.Fixture = fixture;
            this.LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
           {
               builder.AddConsole();
               builder.AddDebug();
           });
            this.LoggerFactory.AddProvider(new TestLoggerProvider(outputHelper));
        }
    }
}