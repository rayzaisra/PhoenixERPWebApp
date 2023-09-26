using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using CommonTool;
using Microsoft.AspNetCore.Mvc;

namespace CodeMarvel.Infrastructure.Acl
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute
    {
        private static string tokenBearer = "Bearer ";

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var authorization = request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorization) || !authorization.ToLower().StartsWith("bearer"))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (authorization.Length < tokenBearer.Length)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            var token = authorization.Substring(7);
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
            else
            {
                context.HttpContext.User = (ClaimsPrincipal)principal;
            }
        }

        private static bool ValidateToken(string token, out string username)
        {
            username = null;
            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
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
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }
    }
}
