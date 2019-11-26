namespace Hero.Api.Host.Options.OAuth2
{
    public class OAuth2Options
    {
        public const string Section = "OAuth2";

        public string Application { get; set; }
        public string Authority { get; set; }
        public string Scope { get; set; }
    }
}
