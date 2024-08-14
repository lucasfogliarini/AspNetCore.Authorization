using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AspNetCore.Authorization.JsonWebToken
{
    public abstract class AuthorizeClaimAttribute : Attribute
    {
        protected bool IsAuthenticated(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return false;
            }
            return true;
        }
        protected static bool HasClaim(ClaimsPrincipal user, string type, string value)
        {
            return user.HasClaim(c => c.Value == value && c.Type == type);
        }
    }
}
