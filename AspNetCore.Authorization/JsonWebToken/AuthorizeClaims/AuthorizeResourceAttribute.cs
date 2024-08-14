using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCore.Authorization.JsonWebToken
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeResourceAttribute(string resource) : AuthorizeClaimAttribute, IAuthorizationFilter
    {
        readonly string _resource = resource;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsAuthenticated(context)) return;

            var hasResourceClaim = HasClaim(context.HttpContext.User, AuthenticationClaimTypes.RESOURCE_CLAIM, _resource);
            if (!hasResourceClaim)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
