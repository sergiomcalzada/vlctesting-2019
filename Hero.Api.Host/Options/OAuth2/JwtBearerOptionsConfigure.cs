using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hero.Api.Host.Options.OAuth2
{

    public class JwtBearerOptionsConfigure : IConfigureOptions<JwtBearerOptions>
    {
        private readonly OAuth2Options oauth2Options;

        public JwtBearerOptionsConfigure(IOptions<OAuth2Options> options)
        {
            this.oauth2Options = options.Value;
        }

        public void Configure(JwtBearerOptions options)
        {
            options.Authority = this.oauth2Options.Authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuers = new[]
                {
                    this.oauth2Options.Authority
                },
                ValidAudiences = new[]
                {
                    this.oauth2Options.Authority + "/resources", this.oauth2Options.Application
                },
            };
            //options.BackchannelHttpHandler = new HttpClientHandler()
            //{
            //    Proxy = new WebProxy(proxyuri, true, new string[0], new NetworkCredential(userName, password, domain))
            //};
        }

        private Task AuthenticationFailed(AuthenticationFailedContext arg)
        {
            // For debugging purposes only!
            var s = $"AuthenticationFailed: {arg.Exception.Message}";
            arg.Response.ContentLength = s.Length;
            arg.Response.Body.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
            return Task.FromResult(0);
        }
    }
}