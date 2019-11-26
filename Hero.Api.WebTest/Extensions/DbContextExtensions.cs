using Hero.Domain;

namespace Hero.Api.WebTest
{
    public static class DbContextExtensions
    {
        public static void Seed(this DataContext context)
        {
            var hero = new Domain.Entity.Hero
            {
                FirstName = "Bruce",
                LastName = "Wayne",
                Pseudonym = "Batman"
            };

            context.Add(hero);

            context.SaveChanges();
        }
    }
}