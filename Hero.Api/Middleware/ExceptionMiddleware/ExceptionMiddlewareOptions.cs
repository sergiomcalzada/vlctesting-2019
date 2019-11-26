namespace Hero.Api.Middleware.ExceptionMiddleware
{
    public class ExceptionMiddlewareOptions
    {
        public const string Section = "ExceptionMiddleware";
        public bool EnableExceptionTrace { get; set; } = false;
    }
}