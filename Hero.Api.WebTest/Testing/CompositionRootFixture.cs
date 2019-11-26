using Microsoft.Extensions.Configuration;
using Respawn;

namespace Hero.Api.WebTest
{
    public class CompositionRootFixture
    {

        private static readonly Checkpoint CheckPoint = new Checkpoint()
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" },
        };

        private static readonly IConfiguration Config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false)
                        .AddEnvironmentVariables()
                        .Build();

        public CompositionRootFixture()
        {


        }


        public static void ResetDatabase()
        {
            CheckPoint.Reset(Config.GetConnectionString("DefaultConnection")).Wait();
        }


    }
}
