using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Phoenix.Shared.Core.Filters;
using Phoenix.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phoenix.Shared.Core.PrincipalHelpers
{
    public class ClainJwtPrincipalHelper
    {
        public static string GetIpAddress(HttpContext context)
        {
            return context.Connection?.RemoteIpAddress?.ToString();
        }
        public static string GetUserId(HttpContext context)
        {
            var userid = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    userid = SessionContext.AppUserId;
                }
            }
            return userid;
        }
        public static string GetRawSession(HttpContext context)
        {
            var rawSession = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    rawSession = SessionContext.WindowsUserClaim;
                }
            }
            return rawSession;
        }
        public static string GetEmailUser(HttpContext context)
        {
            var userid = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    userid = SessionContext.EmailUser;
                }
            }
            return userid;
        }
        public static string GetGroupAccess(HttpContext context)
        {
            var userid = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    userid = SessionContext.GroupAccess;
                }
            }
            return userid;
        }
        public static string GetEmployeeId(HttpContext context)
        {
            var valueClaim = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    valueClaim = SessionContext.EmployeeId;
                }
            }
            return valueClaim;
        }
        public static string GetBusinessGroup(HttpContext context)
        {
            var valueClaim = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    valueClaim = SessionContext.BusinessGroup;
                }
            }
            return valueClaim;
        }

        public static string GetBusinessSubGroup(HttpContext context)
        {
            var valueClaim = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    valueClaim = SessionContext.BusinessSubGroup;
                }
            }
            return valueClaim;
        }

        public static string GetBusinessUnitDivisi(HttpContext context)
        {
            var valueClaim = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    valueClaim = SessionContext.BusinessUnitDivisi;
                }
            }
            return valueClaim;
        }
        public static string GetBusinessUnitDepartment(HttpContext context)
        {
            var valueClaim = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    valueClaim = SessionContext.BusinessUnitDepartment;
                }
            }
            return valueClaim;
        }
        public static string GetJobTitleId(HttpContext context)
        {
            var valueClaim = "";
            if (context.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                var principal = JwtManager.GetPrincipal(value.ToString().Substring("Bearer ".Length));
                if (principal == null)
                {
                    return "Invalid Token";
                }
                else
                {
                    var xuser = principal.Claims.ToList()[1].Value;
                    var SessionContext = JsonConvert.DeserializeObject<UserSession>(principal.Claims.ToList()[1].Value);
                    valueClaim = SessionContext.JobTitleId;
                }
            }
            return valueClaim;
        }
    }
}
