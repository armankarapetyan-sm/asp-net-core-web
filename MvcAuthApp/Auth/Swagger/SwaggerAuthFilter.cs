using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MvcAuthApp.Auth;

public class SwaggerAuthFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!HasAttribute<AuthorizeAttribute>(context) || HasAttribute<AllowAnonymousAttribute>(context))
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            Requirement("Bearer")
        };
    }

    private static OpenApiSecurityRequirement Requirement(string id)
    {
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
                },
                new List<string>()
            }
        };
    }

    private static bool HasAttribute<T>(OperationFilterContext context) where T : Attribute
    {
        if (context.MethodInfo.GetCustomAttributes(true).OfType<T>().Any())
        {
            return true;
        }

        return context.MethodInfo.DeclaringType != null
            && context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<T>().Any();
    }
}
