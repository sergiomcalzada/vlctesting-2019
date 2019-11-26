using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Hero.Api.Configuration.Authorization
{
    public class AuthorizationOptionsConfigure : IConfigureOptions<AuthorizationOptions>
    {
        private readonly ApiAuthorizationOptions apiAuthorizationOptions;

        public AuthorizationOptionsConfigure(IOptions<ApiAuthorizationOptions> options)
        {
            this.apiAuthorizationOptions = options.Value;
        }

        public void Configure(AuthorizationOptions options)
        {
            if (this.apiAuthorizationOptions.IsEnabled)
            {

                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                          .RequireAuthenticatedUser()
                                          .Build();
            }
            else
            {
                options.DefaultPolicy = null;
            }
        }
    }
}