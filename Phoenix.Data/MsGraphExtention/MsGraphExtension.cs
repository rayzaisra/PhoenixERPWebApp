using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Phoenix.ApiExtension;
using Phoenix.Shared.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Phoenix.Data.MsGraphExtention
{
    public class MsGraph
    {
        string[] scopes = { "https://graph.microsoft.com/.default" };

        private string ClientId { get; set; }
        private string TenantId { get; set; }
        private string Authority { get; set; }

        public MsGraph(string clientId, string tenantId)
        {
            ClientId = clientId;
            TenantId = tenantId;
            Authority = "https://login.microsoftonline.com/" + tenantId;
            //Authority = " https://login.microsoftonline.com/common/oauth2/nativeclient";
        }
        public GraphServiceClient CreateGraphServiceClient(string token)
        {
            var graphClient = new GraphServiceClient("https://graph.microsoft.com/v1.0",
                new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        await Task.Delay(10);
                        requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }));

            return graphClient;
        }

        private async Task<Microsoft.Identity.Client.AuthenticationResult> GetTokenForWebApiUsingDeviceCodeFlowAsync(IEnumerable<string> scopes)
        {
            Microsoft.Identity.Client.AuthenticationResult result;
            try
            {
                IPublicClientApplication App = PublicClientApplicationBuilder.Create(ClientId)
                                             .WithAuthority(Authority)
                                             .WithTenantId(TenantId)
                                             .Build();

                result = await App.AcquireTokenWithDeviceCode(scopes,
                    deviceCodeCallback =>
                    {
                        // This will print the message on the console which tells the user where to go sign-in using 
                        // a separate browser and the code to enter once they sign in.
                        // The AcquireTokenWithDeviceCodeAsync() method will poll the server after firing this
                        // device code callback to look for the successful login of the user via that browser.
                        // This background polling (whose interval and timeout data is also provided as fields in the 
                        // deviceCodeCallback class) will occur until:
                        // * The user has successfully logged in via browser and entered the proper code
                        // * The timeout specified by the server for the lifetime of this code (typically ~15 minutes) has been reached
                        // * The developing application calls the Cancel() method on a CancellationToken sent into the method.
                        //   If this occurs, an OperationCanceledException will be thrown (see catch below for more details).
                        Console.WriteLine(deviceCodeCallback.Message);
                        return Task.FromResult(0);
                    }).ExecuteAsync();
            }
            catch (MsalServiceException ex)
            {
                // Kind of errors you could have (in errorCode and ex.Message)
                string errorCode = ex.ErrorCode;

                // AADSTS50059: No tenant-identifying information found in either the request or implied by any provided credentials.
                // Mitigation: as explained in the message from Azure AD, the authority needs to be tenanted. you have probably created
                // your public client application with the following authorities:
                // https://login.microsoftonline.com/common or https://login.microsoftonline.com/organizations

                // AADSTS90133: Device Code flow is not supported under /common or /consumers endpoint.
                // Mitigation: as explained in the message from Azure AD, the authority needs to be tenanted

                // AADSTS90002: Tenant <tenantId or domain you used in the authority> not found. This may happen if there are 
                // no active subscriptions for the tenant. Check with your subscription administrator.
                // Mitigation: if you have an active subscription for the tenant this might be that you have a typo in the 
                // tenantId (GUID) or tenant domain name, update the 

                // The issues above are typically programming / app configuration errors, they need to be fixed
                throw;
            }
            catch (OperationCanceledException)
            {
                // If you use an override with a CancellationToken, and call the Cancel() method on it, then this may be triggered
                // to indicate that the operation was cancelled. 
                // See https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads 
                // for more detailed information on how C# supports cancellation in managed threads.
                result = null;
            }
            catch (MsalClientException ex)
            {
                string errorCode = ex.ErrorCode;

                // Verification code expired before contacting the server
                // This exception will occur if the user does not manage to sign-in before a time out (15 mins) and the
                // call to `AcquireTokenWithDeviceCodeAsync` is not cancelled in between
                result = null;
            }
            return result;
        }


        public async Task<Microsoft.Identity.Client.AuthenticationResult> GetAuthentication(string email, string password)
        {
            IPublicClientApplication app = PublicClientApplicationBuilder.Create(ClientId)
                                              .WithAuthority(Authority)
                                              .WithTenantId(TenantId)
                                              .Build();

            //GetTokenForWebApiUsingDeviceCodeFlowAsync(scopes);
                   //var app = ConfidentialClientApplicationBuilder.Create(clientId)
                   //    .WithClientSecret(clientSc)
                   //    .WithTenantId(tenantId)                
                   //    .WithAuthority(new Uri(authority))
                   //    .Build();

                   //var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

                   var accounts = await app.GetAccountsAsync();

            Microsoft.Identity.Client.AuthenticationResult result = null;
            if (accounts.Any())
            {
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                  .ExecuteAsync();

                return result;
            }
            else
            {
                try
                {
                    var securePassword = new SecureString();
                    foreach (char c in password)        // you should fetch the password keystroke
                        securePassword.AppendChar(c);  // by keystroke

                    //await app.AcquireTokenInteractive(scopes).ExecuteAsync();
                    result = await app.AcquireTokenByUsernamePassword(scopes, email, securePassword).ExecuteAsync();
                    //         result = await app.AcquireTokenWithDeviceCode(scopes,
                    //deviceCodeResult =>
                    //{
                    //    // This will print the message on the console which tells the user where to go sign-in using
                    //    // a separate browser and the code to enter once they sign in.
                    //    // The AcquireTokenWithDeviceCode() method will poll the server after firing this
                    //    // device code callback to look for the successful login of the user via that browser.
                    //    // This background polling (whose interval and timeout data is also provided as fields in the
                    //    // deviceCodeCallback class) will occur until:
                    //    // * The user has successfully logged in via browser and entered the proper code
                    //    // * The timeout specified by the server for the lifetime of this code (typically ~15 minutes) has been reached
                    //    // * The developing application calls the Cancel() method on a CancellationToken sent into the method.
                    //    //   If this occurs, an OperationCanceledException will be thrown (see catch below for more details).
                    //    var rx = deviceCodeResult.Message;
                    //    return Task.FromResult(0);
                    //}).ExecuteAsync();



                    return result;
                }
                catch (MsalUiRequiredException ex) when (ex.Message.Contains("AADSTS65001"))
                {
                    throw;
                    // Here are the kind of error messages you could have, and possible mitigations

                    // ------------------------------------------------------------------------
                    // MsalUiRequiredException: AADSTS65001: The user or administrator has not consented to use the application
                    // with ID '{appId}' named '{appName}'. Send an interactive authorization request for this user and resource.

                    // Mitigation: you need to get user consent first. This can be done either statically (through the portal), 
                    /// or dynamically (but this requires an interaction with Azure AD, which is not possible with 
                    // the username/password flow)
                    // Statically: in the portal by doing the following in the "API permissions" tab of the application registration:
                    // 1. Click "Add a permission" and add all the delegated permissions corresponding to the scopes you want (for instance
                    // User.Read and User.ReadBasic.All)
                    // 2. Click "Grant/revoke admin consent for <tenant>") and click "yes".
                    // Dynamically, if you are not using .NET Core (which does not have any Web UI) by 
                    // calling (once only) AcquireTokenInteractive.
                    // remember that Username/password is for public client applications that is desktop/mobile applications.
                    // If you are using .NET core or don't want to call AcquireTokenInteractive, you might want to:
                    // - use device code flow (See https://aka.ms/msal-net-device-code-flow)
                    // - or suggest the user to navigate to a URL to consent: https://login.microsoftonline.com/common/oauth2/v2.0/authorize?client_id={clientId}&response_type=code&scope=user.read
                    // ------------------------------------------------------------------------

                    // ------------------------------------------------------------------------
                    // ErrorCode: invalid_grant
                    // SubError: basic_action
                    // MsalUiRequiredException: AADSTS50079: The user is required to use multi-factor authentication.
                    // The tenant admin for your organization has chosen to oblige users to perform multi-factor authentication.
                    // Mitigation: none for this flow
                    // Your application cannot use the Username/Password grant.
                    // Like in the previous case, you might want to use an interactive flow (AcquireTokenInteractive()), 
                    // or Device Code Flow instead.
                    // Note this is one of the reason why using username/password is not recommended;
                    // ------------------------------------------------------------------------

                    // ------------------------------------------------------------------------
                    // ex.ErrorCode: invalid_grant
                    // subError: null
                    // Message = "AADSTS70002: Error validating credentials.
                    // AADSTS50126: Invalid username or password
                    // In the case of a managed user (user from an Azure AD tenant opposed to a
                    // federated user, which would be owned
                    // in another IdP through ADFS), the user has entered the wrong password
                    // Mitigation: ask the user to re-enter the password
                    // ------------------------------------------------------------------------

                    // ------------------------------------------------------------------------
                    // ex.ErrorCode: invalid_grant
                    // subError: null
                    // MsalServiceException: ADSTS50034: To sign into this application the account must be added to 
                    // the {domainName} directory.
                    // or The user account does not exist in the {domainName} directory. To sign into this application, 
                    // the account must be added to the directory.
                    // The user was not found in the directory
                    // Explanation: wrong username
                    // Mitigation: ask the user to re-enter the username.
                    // ------------------------------------------------------------------------
                }
                catch (MsalServiceException ex) when (ex.ErrorCode == "invalid_request")
                {
                    throw;
                    // ------------------------------------------------------------------------
                    // AADSTS90010: The grant type is not supported over the /common or /consumers endpoints. 
                    // Please use the /organizations or tenant-specific endpoint.
                    // you used common.
                    // Mitigation: as explained in the message from Azure AD, the authority you use in the application needs 
                    // to be tenanted or otherwise "organizations". change the
                    // "Tenant": property in the appsettings.json to be a GUID (tenant Id), or domain name (contoso.com) 
                    // if such a domain is registered with your tenant
                    // or "organizations", if you want this application to sign-in users in any Work and School accounts.
                    // ------------------------------------------------------------------------

                }
                catch (MsalServiceException ex) when (ex.ErrorCode == "unauthorized_client")
                {
                    throw;
                    // ------------------------------------------------------------------------
                    // AADSTS700016: Application with identifier '{clientId}' was not found in the directory '{domain}'.
                    // This can happen if the application has not been installed by the administrator of the tenant or consented 
                    // to by any user in the tenant.
                    // You may have sent your authentication request to the wrong tenant
                    // Cause: The clientId in the appsettings.json might be wrong
                    // Mitigation: check the clientId and the app registration
                    // ------------------------------------------------------------------------
                }
                catch (MsalServiceException ex) when (ex.ErrorCode == "invalid_client")
                {
                    throw;
                    // ------------------------------------------------------------------------
                    // AADSTS70002: The request body must contain the following parameter: 'client_secret or client_assertion'.
                    // Explanation: this can happen if your application was not registered as a public client application in Azure AD
                    // Mitigation: in the Azure portal, edit the manifest for your application and set the `allowPublicClient` to `true`
                    // ------------------------------------------------------------------------
                }
                catch (MsalServiceException)
                {
                    throw;
                }

                catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user_type")
                {
                    // Message = "Unsupported User Type 'Unknown'. Please see https://aka.ms/msal-net-up"
                    // The user is not recognized as a managed user, or a federated user. Azure AD was not
                    // able to identify the IdP that needs to process the user
                    throw new ArgumentException("U/P: Wrong username", ex);
                }
                catch (MsalClientException ex) when (ex.ErrorCode == "user_realm_discovery_failed")
                {
                    // The user is not recognized as a managed user, or a federated user. Azure AD was not
                    // able to identify the IdP that needs to process the user. That's for instance the case
                    // if you use a phone number
                    throw new ArgumentException("U/P: Wrong username", ex);
                }
                catch (MsalClientException ex) when (ex.ErrorCode == "unknown_user")
                {
                    // the username was probably empty
                    // ex.Message = "Could not identify the user logged into the OS. See http://aka.ms/msal-net-iwa for details."
                    throw new ArgumentException("U/P: Wrong username", ex);
                }
                catch (MsalClientException ex) when (ex.ErrorCode == "parsing_wstrust_response_failed")
                {
                    throw;
                    // ------------------------------------------------------------------------
                    // In the case of a Federated user (that is owned by a federated IdP, as opposed to a managed user owned in an Azure AD tenant)
                    // ID3242: The security token could not be authenticated or authorized.
                    // The user does not exist or has entered the wrong password
                    // ------------------------------------------------------------------------
                }
            }
        }
    }

    public static class MsGraphExtension
    {
        public static async Task<Stream> DownloadFile(this GraphServiceClient graphClient, string fileName, bool uploadToSharePoint)
        {
            if (uploadToSharePoint)
                return await graphClient.Sites["root"].Drive.Root.ItemWithPath(fileName).Content.Request().GetAsync();
            else
                return await graphClient.Me.Drive.Root.ItemWithPath(fileName).Content.Request().GetAsync();
        }

        public static async Task<DriveItem> UploadSmallFile(this GraphServiceClient graphClient, Stream fileStream, string fileName, bool uploadToSharePoint)
        {
            if (uploadToSharePoint)
                return await graphClient.Sites["root"].Drive.Root.ItemWithPath(fileName).Content.Request().PutAsync<DriveItem>(fileStream);
            else
                return await graphClient.Me.Drive.Root.ItemWithPath(fileName).Content.Request().PutAsync<DriveItem>(fileStream);
        }

        public static async Task<DriveItem> CreateFolder(this GraphServiceClient graphClient, string folderName, string pathFolder, bool isRoot)
        {
            var result = new DriveItem();
            var driveItem = new DriveItem
            {
                Name = folderName,
                Folder = new Folder { },
                AdditionalData = new Dictionary<string, object>()
                {
                    {"@microsoft.graph.conflictBehavior","rename"}
                }
            };

            if (isRoot)
            {
                var documentsBuilder = await graphClient.Me.Drive.Root.Children.Request().GetAsync();

                // try to find existing folder
                var folder = documentsBuilder.FirstOrDefault(c => c.Name.Equals(folderName));
                if (folder == null)
                {
                    result = await graphClient.Me.Drive.Root.Children.Request().AddAsync(driveItem);
                }

            }
            else
            {
                var documentsBuilder = await graphClient.Me.Drive.Root.ItemWithPath("/" + pathFolder).Children.Request().GetAsync();

                // try to find existing folder
                var folder = documentsBuilder.FirstOrDefault(c => c.Name.Equals(folderName));
                if (folder == null)
                {
                    result = await graphClient.Me.Drive.Root.ItemWithPath("/" + pathFolder).Children.Request().AddAsync(driveItem);
                }
                
            }
            return result;  
        }

        /// <summary>
        /// Created By M.Salih
        /// Source Folder bisa berupa ID Item Folder atau Path ,contoh : "ROOT/SubFOlder
        /// </summary>
        /// <param name="graphClient">authentication msGraph</param>
        /// <param name="sourceFolder">Id Item Folder atau PAth Folder</param>
        /// <param name="usingId">true : menggunakan Id Item Folder, false : menggunakan Path</param>
        /// <returns></returns>
        public static async Task RemoveFolder(this GraphServiceClient graphClient, string sourceFolder, bool usingId)
        {
            if (usingId)
                await graphClient.Me.Drive.Items[sourceFolder].Request().DeleteAsync();
            else
                await graphClient.Me.Drive.Root.ItemWithPath("/" + sourceFolder).Request().DeleteAsync();
        }

        public static async Task<BaseGeneralResponse> MoveFolder(this GraphServiceClient graphClient, string directory, string fileName, string oldDirectory)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            Stream file; 
            try
            {
                file = await DownloadFile(graphClient, oldDirectory , false);
            }
            catch (Exception ex3)
            {
                resp.Success = false;
                resp.Message = ex3.Message;
                return resp;
            }

            try
            {
                var uploadFile = await graphClient.UploadLargeFile(file, directory + "/" + fileName, false);
            }
            catch (Exception ex2)
            {
                resp.Success = false;
                resp.Message = ex2.Message;
                return resp;
            }

            try
            {
                await graphClient.Me.Drive.Root.ItemWithPath("/" + oldDirectory).Request().DeleteAsync();
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
                return resp;
            }
            resp.Success = true;
            resp.Message = "Success";
            return resp;
        }

        public static async Task<BaseGeneralResponse> MoveFileToMyDam(this GraphServiceClient graphClient, string directory, string fileName, string origFileName)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {

                var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + directory).Children.Request().GetAsync();
                var origFullPath = directory + "/" + origFileName;
                var fullpath = directory + "/" + fileName;

                var downloadFile = await DownloadFile(graphClient, "/" + origFullPath, false);
                Stream file = null;
                bool IsExist = false;

                var uploadFile = await graphClient.UploadLargeFile(downloadFile, "PROJECTMANAGEMENT/DAM/" + fileName, false);

                resp.Code = "200";
                resp.Success = true;
                //var downloadFile = await DownloadFile(graphClient, "/" + fileName, false);
                //var uploadFile = await graphClient.UploadLargeFile(downloadFile, UserName + "/" + fileName, false);

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                resp.Code = "404";
                resp.Success = false;

            }
            return resp;

        }

        public static async Task<BaseGeneralResponse> CopyFile(this GraphServiceClient graphClient, string directory, string fileName, string UserName)
        {
            BaseGeneralResponse resp = new BaseGeneralResponse();
            try
            {

                var driveItem = await graphClient.Me.Drive.Root.ItemWithPath("/" + directory).Children.Request().GetAsync();
                try
                {
                    var driveItemFolder = new DriveItem
                    {
                        Name = UserName,
                        Folder = new Folder { },
                        AdditionalData = new Dictionary<string, object>()
                            {
                                {"@microsoft.graph.conflictBehavior","fail"}
                            }
                    };
                    await graphClient.Me.Drive.Root.ItemWithPath("/USER").Children.Request().AddAsync(driveItemFolder);
                }
                catch (Exception ex2)
                {

                }

                var fullpath = directory + "/" + fileName;

                var downloadFile = await DownloadFile(graphClient, "/" + fullpath, false);
                Stream file = null;
                bool IsExist = false;
                try
                {
                    file = await DownloadFile(graphClient, "USER/" + UserName + "/" + fullpath, false);
                    IsExist = true;
                }
                catch (Exception ex3)
                {
                    IsExist = false;
                }

                if (!IsExist)
                {
                    var uploadFile = await graphClient.UploadLargeFile(downloadFile, "USER/" + UserName + "/" + fullpath, false);
                }


                resp.Code = "200";
                resp.Success = true;
                //var downloadFile = await DownloadFile(graphClient, "/" + fileName, false);
                //var uploadFile = await graphClient.UploadLargeFile(downloadFile, UserName + "/" + fileName, false);

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                resp.Code = "404";
                resp.Success = false;

            }
            return resp;

        }

        public static async Task<DriveItem> UploadLargeFile(this GraphServiceClient graphClient, Stream fileStream, string fileName, bool uploadToSharePoint)
        {
            //Memory<Stream> stream;
            DriveItem uploadedFile = null;
            UploadSession uploadSession = null;

            // Do we want OneDrive for Business/Consumer or do we want a SharePoint Site? 
            if (uploadToSharePoint)
            {
                uploadSession = await graphClient.Sites["root"].Drive.Root.ItemWithPath(fileName).CreateUploadSession().Request().PostAsync();
            }
            else
            {
                uploadSession = await graphClient.Me.Drive.Root.ItemWithPath(fileName).CreateUploadSession().Request().PostAsync();
            }

            if (uploadSession != null)
            {
                // Chunk size must be divisible by 320KiB, our chunk size will be slightly more than 1MB 
                int maxSizeChunk = (320 * 1024) * 4;
                ChunkedUploadProvider uploadProvider = new ChunkedUploadProvider(uploadSession, graphClient, fileStream, maxSizeChunk);
                var chunkRequests = uploadProvider.GetUploadChunkRequests();
                var exceptions = new List<Exception>();
                var readBuffer = new byte[maxSizeChunk];

                foreach (var request in chunkRequests)
                {
                    var result = await uploadProvider.GetChunkRequestResponseAsync(request, readBuffer, exceptions);

                    if (result.UploadSucceeded)
                    {
                        uploadedFile = result.ItemResponse;
                    }
                }
            }
            return uploadedFile;
        }
    }
}
