using Microsoft.AspNetCore.Authorization;
using MvcAuthApp.Models;

namespace MvcAuthApp.Auth;

public class EditPostHandler : AuthorizationHandler<EditPostRequirement, Post>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EditPostRequirement requirement,
        Post resource)
    {
        if (context.User.IsInRole(AppRoles.Admin))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.Identity != null
            && context.User.Identity.IsAuthenticated
            && string.Equals(context.User.Identity.Name, resource.Author, StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
