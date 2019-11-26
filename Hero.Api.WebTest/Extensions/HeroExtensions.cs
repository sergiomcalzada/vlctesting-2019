using System;
using Hero.Domain.Entity;
using System.Threading.Tasks;

namespace Hero.Api.WebTest
{
    public static class HeroExtensions
    {
        public static Task<Domain.Entity.Hero> WithHeroInTheDatabase(this BaseTest current)
        {
            var hero = new Domain.Entity.Hero()
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Pseudonym = Guid.NewGuid().ToString()
            };
            return current.WithEntityInTheDatabase(hero);
        }

        
    }
}
