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
    public partial interface IRoleAccessMenuServices : IBaseService<RoleAccessMenuDto, RoleAccessMenu, string>
    {
        GeneralResponseList<RoleAccessMenuDto> ListAll(RoleAccessMenuSearch filter);
    }
    public partial class RoleAccessMenuServices : BaseService<RoleAccessMenuDto, RoleAccessMenu, string>, IRoleAccessMenuServices
    {
        public RoleAccessMenuServices(IEFRepository<RoleAccessMenu, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<RoleAccessMenuDto> ListAll(RoleAccessMenuSearch filter)
        {

            GeneralResponseList<RoleAccessMenuDto> resp = new GeneralResponseList<RoleAccessMenuDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<RoleAccessMenuDto> que = from tb1 in _repo.GetContext().Set<RoleAccessMenu>()
                          select new RoleAccessMenuDto()
                          {
                              Id = tb1.Id,
                              application_menu_id = tb1.application_menu_id,
                              application_role_id = tb1.application_role_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_menu_id", filter.Search_application_menu_id);
                que = q.filterEquals(que, "application_role_id", filter.Search_application_role_id);

                

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

