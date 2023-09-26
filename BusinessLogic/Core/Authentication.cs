using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using CommonTool;
using DataAccess;
using DataAccess.PhoenixERP;
using NLog;
using CodeMarvel.Infrastructure.ModelShared;
using DataAccess.PhoenixERP.General;
using Microsoft.Extensions.Options;
using Phoenix.Shared.Core.Services;
using CodeMarvel.Infrastructure.Sessions;
using System.Linq;
using NPoco;

namespace BusinessLogic.Core
{
    public partial class Authentication
    {
        private readonly Logger appLogger;
        private MsGraphConfig _msgraphconfig=new MsGraphConfig();

        public Authentication(Logger AppLogger)
        {
            appLogger = AppLogger;
        }

        public bool Authenticate(UserCredential Credential, out string Token, out string MsToken)
        {


            bool result = false;
            Token = null;
            MsToken = null;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = string.Empty;
                    qry = application_user.DefaultView;
                    qry += " WHERE r.app_username = @0";
                    var data = db.FirstOrDefault<vw_application_user>(qry, Credential.Username);
                    appLogger.Debug("Authenticate {0}", Credential?.Username);

                    if (data != null)
                    {
                        bool userexist = false;
                        if (data.is_msuser == true)
                        {
                            //-------------------Check to MSGRAPH--------------------
                            MsGraphLogin parameter = new MsGraphLogin();
                            parameter.username = data.email;
                            parameter.password = Credential.Password;
                            parameter.client_id = "766203ec-9c5f-4f99-9422-7adc76a30788";
                            parameter.client_secret = "EsVF5tDpzyMkS9GcjMY9CB2QybeMfbT0Bp0YYLlokAw=";
                            parameter.grant_type = "password";
                            parameter.requested_token_use = "on_behalf_of";
                            parameter.resource = "https://graph.microsoft.com";
                            parameter.scope = "openid";
                            RestClient<MsGraphLoginresult> ApiClient = new RestClient<MsGraphLoginresult>();
                            var retval = ApiClient.GetResponse("https://login.microsoftonline.com/3291f647-acb0-4ac9-b57d-60158629a183/oauth2/token?prompt=consent", parameter);
                            if (retval != null)
                            {
                                MsToken = retval.access_token;
                                userexist = true;
                            }
                        }
                        else
                        {
                            qry = string.Empty;
                            qry = " SELECT app_password FROM application_user WHERE id = @0 ";
                            var pswd = db.ExecuteScalar<string>(qry, data.id);
                            userexist = PasswordHash.ValidatePassword(Credential.Password, pswd);
                        }
 
                        if (result = userexist)
                        {
                            appLogger.Debug("User {0} is authenticated", Credential?.Username);
                            var BU = db.FirstOrDefault<business_unit>(" WHERE id=@0 AND is_active=1 AND COALESCE(is_deleted,0)=0", data.business_unit_id);
                            var sqlRole = Sql.Builder.Append(user_role.DefaultView);
                            sqlRole.Append(" WHERE r.application_user_id=@0 AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0", data.id);
                            var appRole = db.Fetch<vw_user_role>(sqlRole);

                            if (BU == null)
                                throw new ArgumentException("Business Unit Not Found");

                            //Please Update Claim Data if class an object UserSession after changed
                            Token = JwtManager.GenerateToken(Credential.Username, new
                            {
                                AppUserId = data.id,
                                Credential.Username,
                                FullName = data.app_fullname,
                                Email = data.email,
                                BusinessUnitId = data.business_unit_id,
                                LegalEntityId = data.legal_entity_id,
                                OrganizationId = BU.organization_id,
                                AppRole = (from r in appRole
                                          select new App_Role {
                                              Id = r.application_role_id,
                                              RoleName = r.role_name
                                          }).ToList(),
                                IsSystemAdministrator = data.is_system_administrator ?? false
                            });
                        }
                        else
                        {
                            appLogger.Error("Invalid username or password");
                        }
                    }
                    else
                    {
                        appLogger.Error("Invalid username or password");
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex.ToString());
                    appLogger.Trace(db.LastCommand);
                    throw;
                }
            }

            return result;
        }
    }
}