using System.Globalization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PersonDictionary.Middlewares;

public class AcceptLanguageMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly CultureInfo DefaultCulture = new("en-US");

    public AcceptLanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();
        CultureInfo defaultCulture = DefaultCulture;

        if (!string.IsNullOrEmpty(acceptLanguageHeader))
        {
            var languages = acceptLanguageHeader.Split(',').Select(x => x.Trim().Split(';')[0]);

            foreach (var language in languages)
            {
                try
                {
                    var culture = CultureInfo.GetCultureInfo(language);
                    defaultCulture = culture;
                    break;
                }
                catch (CultureNotFoundException)
                {
                    // Ignore invalid culture names
                }
            }
        }

        CultureInfo.CurrentCulture = defaultCulture;
        CultureInfo.CurrentUICulture = defaultCulture;
        context.Items["Accept-Language"] = defaultCulture;
        context.Response.Headers.Add("Content-Language", defaultCulture.Name);

        await _next(context);
    }

    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("en-US")
                }
            });
        }
    }
}