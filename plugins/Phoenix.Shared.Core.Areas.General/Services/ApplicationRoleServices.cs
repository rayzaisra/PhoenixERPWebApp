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
    public partial interface IApplicationRoleServices : IBaseService<ApplicationRoleDto, ApplicationRole, string>
    {
        GeneralResponseList<ApplicationRoleDto> ListAll(ApplicationRoleSearch filter);
    }
    public partial class ApplicationRoleServices : BaseService<ApplicationRoleDto, ApplicationRole, string>, IApplicationRoleServices
    {
        public ApplicationRoleServices(IEFRepository<ApplicationRole, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApplicationRoleDto> ListAll(ApplicationRoleSearch filter)
        {

            GeneralResponseList<ApplicationRoleDto> resp = new GeneralResponseList<ApplicationRoleDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApplicationRoleDto> que = from tb1 in _repo.GetContext().Set<ApplicationRole>()
                          select new ApplicationRoleDto()
                          {
                              Id = tb1.Id,
                              is_default_role = tb1.is_default_role,
                              organization_id = tb1.organization_id,
                              role_name = tb1.role_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "is_default_role", filter.Search_is_default_role);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);
                que = q.filterContains(que, "role_name", filter.Search_role_name);

                

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

