using CommonTool;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace CodeMarvel.Infrastructure.Sessions
{
    public static class SessionManager
    {
        private static readonly NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        public static UserSession UserSession
        {
            get
            {
                var context = StaticHttpContextAccessor.Current;
                var serverType = context.Request.Headers["Server-Type"];
                var result = new UserSession();

                try
                {
                    if (serverType == AppConstants.ServerTypeWeb)
                    {
                        var Token = context.Session.GetString(AppConstants.SessionKey);
                        var EmployeeBasicInfoId = context.Session.GetString(AppConstants.EmployeeBasicInfoId);
                        var BusinessUnitGroupId = context.Session.GetString(AppConstants.BusinessUnitGroupId);
                        var BusinessUnitSubgroupId = context.Session.GetString(AppConstants.BusinessUnitSubgroupId);
                        var BusinessUnitDivisiId = context.Session.GetString(AppConstants.BusinessUnitDivisiId);
                        var BusinessUnitDepartementId = context.Session.GetString(AppConstants.BusinessUnitDepartementId);
                        var JobTitleId = context.Session.GetString(AppConstants.JobTitleId);
                        var TokenClaimData = JwtManager.GetPrincipal(Token);
                        var tokenValue = TokenClaimData?.Claims?.ToList()?[1]?.Value;
                        if (!string.IsNullOrEmpty(tokenValue))
                        {
                            result = JsonConvert.DeserializeObject<UserSession>(tokenValue);
                            result.Token = Token;
                            result.EmployeeBasicInfoId = EmployeeBasicInfoId;
                            result.BusinessUnitGroupId = BusinessUnitGroupId;
                            result.BusinessUnitSubgroupId = BusinessUnitSubgroupId;
                            result.BusinessUnitDivisiId = BusinessUnitDivisiId;
                            result.BusinessUnitDivisiId = BusinessUnitDivisiId;
                            result.BusinessUnitDepartementId = BusinessUnitDepartementId;
                            result.JobTitleId = JobTitleId;
                        }
                    }
                    else if (serverType == AppConstants.ServerTypeAPI)
                    {
                        StringValues TokenHeader = new StringValues();
                        var EmployeeBasicInfoId = context.Session.GetString(AppConstants.EmployeeBasicInfoId);
                        var BusinessUnitGroupId = context.Session.GetString(AppConstants.BusinessUnitGroupId);
                        var BusinessUnitSubgroupId = context.Session.GetString(AppConstants.BusinessUnitSubgroupId);
                        var BusinessUnitDivisiId = context.Session.GetString(AppConstants.BusinessUnitDivisiId);
                        var BusinessUnitDepartementId = context.Session.GetString(AppConstants.BusinessUnitDepartementId);
                        var JobTitleId = context.Session.GetString(AppConstants.JobTitleId);
                        appLogger.Debug("Debug::GetToken");

                        if (context.Request.Headers.TryGetValue("Authorization", out TokenHeader))
                        {
                            appLogger.Debug("Debug::Header::Token");
                            var TokenData = JwtManager.GetPrincipal(TokenHeader.ToString().Substring("Bearer ".Length));
                            if (TokenData != null)
                            {
                                result =
                                    JsonConvert.DeserializeObject<UserSession>(TokenData.Claims.ToList()[1].Value);
                                result.Token = TokenHeader.ToString().Substring("Bearer ".Length);
                                result.EmployeeBasicInfoId = EmployeeBasicInfoId;
                                result.BusinessUnitGroupId = BusinessUnitGroupId;
                                result.BusinessUnitSubgroupId = BusinessUnitSubgroupId;
                                result.BusinessUnitDivisiId = BusinessUnitDivisiId;
                                result.BusinessUnitDivisiId = BusinessUnitDivisiId;
                                result.BusinessUnitDepartementId = BusinessUnitDepartementId;
                                result.JobTitleId = JobTitleId;
                            }
                        }
                        else
                        {
                            if (context.Request.Query.TryGetValue("x-token", out TokenHeader))
                            {

                                appLogger.Debug("Debug::QueryString::Token");
                                String xToken = string.Empty;
                                xToken = string.Format("Bearer {0}", TokenHeader.ToString());

                                var TokenData = JwtManager.GetPrincipal(xToken.Substring("Bearer ".Length));
                                if (TokenData != null)
                                {
                                    result =
                                        JsonConvert.DeserializeObject<UserSession>(TokenData.Claims.ToList()[1].Value);
                                    result.Token = TokenHeader.ToString().Substring("Bearer ".Length);
                                    result.EmployeeBasicInfoId = EmployeeBasicInfoId;
                                    result.BusinessUnitGroupId = BusinessUnitGroupId;
                                    result.BusinessUnitSubgroupId = BusinessUnitSubgroupId;
                                    result.BusinessUnitDivisiId = BusinessUnitDivisiId;
                                    result.BusinessUnitDivisiId = BusinessUnitDivisiId;
                                    result.BusinessUnitDepartementId = BusinessUnitDepartementId;
                                    result.JobTitleId = JobTitleId;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex.ToString());
                }

                return result;
            }
        }
    }
}
