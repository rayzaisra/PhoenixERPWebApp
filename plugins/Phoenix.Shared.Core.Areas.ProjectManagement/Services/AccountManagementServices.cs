using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IAccountManagementServices : IBaseService<AccountManagementDto, AccountManagement, string>
    {
        GeneralResponseList<AccountManagementList> ListAll(AccountManagementSearch filter);
        GeneralResponseList<Actor> ListActor();

    }
    public partial class AccountManagementServices : BaseService<AccountManagementDto, AccountManagement, string>, IAccountManagementServices
    {
        public AccountManagementServices(IEFRepository<AccountManagement, string> repo) : base(repo)
        {
            _repo = repo;
        }
        public GeneralResponseList<Actor> ListActor()
        {

            GeneralResponseList<Actor> resp = new GeneralResponseList<Actor>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<Actor> que = from tb1 in _repo.GetContext().Set<Actor>()
                                        orderby tb1.level
                                                        select new Actor()
                                                        {
                                                            id = tb1.id,
                                                            extendata = tb1.extendata,
                                                            level = tb1.level,
                                                            name = tb1.name
                                                        };

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
        public GeneralResponseList<AccountManagementList> ListAll(AccountManagementSearch filter)
        {

            GeneralResponseList<AccountManagementList> resp = new GeneralResponseList<AccountManagementList>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AccountManagementList> que = from tb1 in _repo.GetContext().Set<AccountManagement>()
                                                        join tb2 in _repo.GetContext().Set<ApplicationUser>() on tb1.user_id equals tb2.Id
                                                        select new AccountManagementList()
                          {
                              Id = tb1.Id,
                              user_id = tb1.user_id,
                              app_username=tb2.app_username,
                              app_fullname=tb2.app_fullname
                                                        };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "app_username", filter.Search_app_username);

                

                resp.RecordsFiltered = que.Count();
                resp.Total = resp.RecordsTotal;

                que = q.sortir(que, "Id", filter.sortBy, filter.sortDir);
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

