using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Phoenix.Shared.Json.Results;
using Phoenix.Shared.Helpers;

namespace Phoenix.Api.Shared.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAsyncAuthorizationFilter, IActionFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple =  false;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            string authHeader = request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                System.Console.WriteLine(token);
                var principal = await AuthenticateJwtToken(token);
                if (principal == null)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = InvalidTokenResult.Get();
                }
                else
                {
                    Glosing.Instance.Username=principal.Identity.Name;
                    context.HttpContext.User = principal;
                }
            }else
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = MissingJwtResult.Get();
            }
            
         
        }

        private ClaimsPrincipal GetPrincipalByToken(string token)
        {

            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return null;
            else
            {
                return simplePrinciple;
            }

        }
        
        private static bool ValidateToken(string token, out string username)
        {
            username = null;

            var simplePrinciple = JwtManager.GetPrincipal(token);
            if (simplePrinciple==null)
                return false;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        protected Task<ClaimsPrincipal> AuthenticateJwtToken(string token)
        {
            string username;

            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                ClaimsPrincipal user = new ClaimsPrincipal(identity);
                
                return Task.FromResult(user);
            }

            return Task.FromResult<ClaimsPrincipal>(null);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            string authHeader = request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                System.Console.WriteLine(token);
                var principal = GetPrincipalByToken(token);
                if (principal == null)
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = InvalidTokenResult.Get();
                }
                else
                {
                    context.HttpContext.User = principal;
                }
            }else
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = MissingJwtResult.Get();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           // throw new NotImplementedException();
        }
    }
}