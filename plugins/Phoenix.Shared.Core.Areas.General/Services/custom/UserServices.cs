using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using Phoenix.Shared.Core.Areas.General.Dtos;
using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Phoenix.Shared.Helpers;

namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface IUserServices : IBaseService<ApplicationUserDto, ApplicationUser, string>
    {
        GeneralResponse<UserLoginData> Login(LoginInputModel parameter);
        GeneralResponse<UserLoginData> LoginMicrosoftGraph(LoginInputModel logger);
        GeneralResponseList<UserSort> AccountManager(SearchParameter filter);
        //BaseGeneralResponse CekAkses(AccessPage accesspage);
    }
    public partial class UserServices : BaseService<ApplicationUserDto, ApplicationUser, string>, IUserServices
    {

        private readonly MsGraphConfig _msgraphconfig;
        public UserServices(IEFRepository<ApplicationUser, string> repo, IOptions<MsGraphConfig> settings) : base(repo)
        {
            _repo = repo;
   
            _msgraphconfig = settings.Value;
        }
        /*
        public BaseGeneralResponse CekAkses(AccessPage accesspage)
        {
            BaseGeneralResponse response = new BaseGeneralResponse();
            if (accesspage.address == "pm/brief/list")
            {
                response.Success = false;
            }
            else
            {
                response.Success = true;
            }

            response.Message = "";
            return response;
        }
        */
        public GeneralResponse<UserLoginData> LoginMicrosoftGraph(LoginInputModel logger)
        {
            MsGraphLogin parameter = new MsGraphLogin();
            parameter.username = logger.Username;
            parameter.password = logger.Password;
            parameter.client_id = _msgraphconfig.client_id;
            parameter.client_secret = _msgraphconfig.client_secret;
            parameter.grant_type = "password";
            parameter.requested_token_use = "on_behalf_of";
            parameter.resource = _msgraphconfig.resource;
            parameter.scope = _msgraphconfig.scope;
            GeneralResponse<UserLoginData> response = new GeneralResponse<UserLoginData>();
            RestClient<MsGraphLoginresult> ApiClient = new RestClient<MsGraphLoginresult>();
            //var retval = ApiClient.GetResponse(_msgraphconfig.login_url, parameter);
            response.Data = new UserLoginData();
            response.Success = false;
 
            /*
            if (retval != null)
            {
                response.Data.Name = parameter.username;
                response.Data.MsGraphToken = retval.access_token;

                if (retval.access_token != null)
                {
                    response.Success = true;
                    response.Message = "Login Success";
                }

            }
            */

            return response;
        }
        public GeneralResponse<UserLoginData> Login(LoginInputModel parameter)
        {
            GeneralResponse<UserLoginData> response = new GeneralResponse<UserLoginData>();
            if (_msgraphconfig.use_mslogin)
            {
                response = LoginMicrosoftGraph(parameter);
            }
            else
            {

                UserLoginData userLoginData = new UserLoginData();
                response.Success = false;
                response.Message = "Login Failed";
                ApplicationUser duser = new ApplicationUser();


                QueryParameter<ApplicationUser> currentuser = new QueryParameter<ApplicationUser>();
                currentuser.dbQuery = from cust in _repo.GetContext().Set<ApplicationUser>()
                                      where cust.app_username.Equals(parameter.Username)
                                      select cust;
                try
                {


                    duser = _repo.FindFirst(currentuser);
                    if (duser != null)
                    {
                        response.Success = true;
                        response.Message = "Login Success";
                        userLoginData.Name = duser.app_username;


                        response.Data = userLoginData;
                        /*
                        if (duser.app_password.Equals(parameter.Password))
                        {
                            response.Success = true;
                            response.Message = "Login Success";
                            userLoginData.Name = duser.app_username;


                            response.Data = userLoginData;
                        }
                        else
                        {
                            response.Message = "Incorect Password";
                        }
                        */
                    }

                }
                catch (Exception ex)
                {
                    response.Message = "User not Found";
                }

            }


            return response;
        }

        public GeneralResponseList<UserSort> AccountManager(SearchParameter filter)
        {
            GeneralResponseList<UserSort> resp = new GeneralResponseList<UserSort>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<UserSort> que = from tb1 in _repo.GetContext().Set<ApplicationUser>()
                                            join tb2 in _repo.GetContext().Set<AccountManagement>()
                     on tb1.Id equals tb2.user_id
                                            select new UserSort()
                                                                {
                                                                    Id = tb1.Id,
                                                                   Username = tb1.app_username
                                                                };

                resp.RecordsTotal = que.Count();
                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;
                if (filter.search != null)
                {
                    que = que.Where(x => x.Username.Contains(filter.search));
                }
                que = q.sortir(que, "username", filter.sortBy, filter.sortDir);
                que = q.limit(que, filter.GetLimit(), filter.GetOffset());

                resp.Rows = que.ToList();
                resp.Success = true;

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }
    }
}
