using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;

namespace API.Common.Swagger
{
    public sealed class GlobalHeadersOperationFilter : IOperationFilter
    {
        // Define all global headers in one place
        private static readonly HeaderDefinition[] Headers =
        {
        new(
            Name: "CompanyId",
            Description: "Company identifier for the current request",
            Required: true,
            Default: "1",
            Type: JsonSchemaType.Integer),

        new(
            Name: "Language",
            Description: "Language code (en = 1, ar = 2). Default is en.",
            Required: false,
            Default: "en",
            Type: JsonSchemaType.String),

        // UserId is normally derived from JWT — only include if you truly
        // need it as a header for dev/testing. Otherwise remove this entry.
        new(
            Name: "UserId",
            Description: "User identifier (development only — production uses JWT claim)",
            Required: false,
            Default: "1",
            Type: JsonSchemaType.Integer),
    };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<IOpenApiParameter>();

            // Skip anonymous endpoints (login, etc.)
            var isAnonymous = context.ApiDescription.CustomAttributes()
                .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
                .Any();

            foreach (var header in Headers)
            {
                // Some headers shouldn't appear on anonymous endpoints
                if (isAnonymous && header.Name == "CompanyId")
                    continue;

                // Avoid duplicates if already added manually on an endpoint
                if (operation.Parameters.Any(p => p.Name == header.Name))
                    continue;

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = header.Name,
                    In = ParameterLocation.Header,
                    Required = header.Required,
                    Description = header.Description,
                    Schema = new OpenApiSchema
                    {
                        Type = header.Type,
                        Default = JsonValue.Create(header.Default)
                    },
                    Example = JsonValue.Create(header.Default)
                });
            }
        }

        private sealed record HeaderDefinition(
            string Name,
            string Description,
            bool Required,
            string Default,
            JsonSchemaType Type);
    }
}
