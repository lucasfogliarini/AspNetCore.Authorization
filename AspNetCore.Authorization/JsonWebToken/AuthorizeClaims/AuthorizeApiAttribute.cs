using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCore.Authorization.JsonWebToken
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizeApiAttribute(string api) : AuthorizeClaimAttribute, IAuthorizationFilter
    {
        readonly string _api = api;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!IsAuthenticated(context)) return;

            var hasApiClaim = HasClaim(context.HttpContext.User, AuthenticationClaimTypes.API_CLAIM, _api);
            if (!hasApiClaim)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
