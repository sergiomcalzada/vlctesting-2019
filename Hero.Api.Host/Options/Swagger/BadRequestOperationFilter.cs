using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hero.Api.Host.Options.Swagger
{
    internal class BadRequestOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var schema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);

            operation.Responses.Add("400", new OpenApiResponse
            {
                Description = "ProblemDetails object",
                Reference = schema.Reference
            });
        }
    }
}