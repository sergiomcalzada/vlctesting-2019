using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hero.Api.Middleware.ExceptionMiddleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly ExceptionMiddlewareOptions options;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IOptions<ExceptionMiddlewareOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Something went wrong and wasn't handled'");
                await this.HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (this.options.EnableExceptionTrace)
            {
                await JsonSerializer.SerializeAsync(
                    context.Response.BodyWriter.AsStream(true),
                    new TraceErrorDetail
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error.",
                        Exception = this.GetExceptionDetails(exception)
                    });
            }
            else
            {
                await JsonSerializer.SerializeAsync(
                    context.Response.BodyWriter.AsStream(true),
                    new ErrorDetail
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error."
                    });

            }
            
        }

        private ExceptionDetail GetExceptionDetails(Exception exception)
        {
            var ex = exception.InnerException == null
                ? new ExceptionDetail
                {
                    Type = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                }
                : new TraceExceptionDetail
                {
                    Type = exception.GetType().FullName,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = this.GetExceptionDetails(exception.InnerException)
                };
            return ex;
        }


        internal class ErrorDetail
        {
            public int StatusCode { get; set; }
            public string Message { get; internal set; }
        }

        internal class TraceErrorDetail : ErrorDetail
        {
            public ExceptionDetail Exception { get; set; }
        }

        internal class ExceptionDetail
        {
            public string Type { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
        }

        internal class TraceExceptionDetail : ExceptionDetail
        {
            public ExceptionDetail InnerException { get; set; }
        }
    }
}
