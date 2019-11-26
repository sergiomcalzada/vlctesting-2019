using System.Collections.Generic;
using System.Security.Claims;

namespace Hero.Api.WebTest
{
    public static class Identities
    {
        public static readonly IEnumerable<Claim> TestUser = new[]
        {
            new Claim(
                type: ClaimTypes.NameIdentifier,
                value: "1",
                valueType: ClaimValueTypes.Integer32,
                issuer: "TestIssuer",
                originalIssuer: "OriginalTestIssuer"),
            new Claim(type: ClaimTypes.Name, value: "User"),
        };

        public static readonly IEnumerable<Claim> Empty = new Claim[0];
    }
}
