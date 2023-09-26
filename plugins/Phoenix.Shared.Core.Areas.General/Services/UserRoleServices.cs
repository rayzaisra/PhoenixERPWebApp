using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface IUserRoleServices : IBaseService<UserRoleDto, UserRole, string>
    {
        GeneralResponseList<UserRoleDto> ListAll(UserRoleSearch filter);
    }
    public partial class UserRoleServices : BaseService<UserRoleDto, UserRole, string>, IUserRoleServices
    {
        public UserRoleServices(IEFRepository<UserRole, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<UserRoleDto> ListAll(UserRoleSearch filter)
        {

            GeneralResponseList<UserRoleDto> resp = new GeneralResponseList<UserRoleDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<UserRoleDto> que = from tb1 in _repo.GetContext().Set<UserRole>()
                          select new UserRoleDto()
                          {
                              Id = tb1.Id,
                              application_role_id = tb1.application_role_id,
                              application_user_id = tb1.application_user_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_role_id", filter.Search_application_role_id);
                que = q.filterEquals(que, "application_user_id", filter.Search_application_user_id);

                

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

