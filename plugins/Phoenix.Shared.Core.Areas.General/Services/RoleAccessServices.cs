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
    public partial interface IRoleAccessServices : IBaseService<RoleAccessDto, RoleAccess, string>
    {
        GeneralResponseList<RoleAccessDto> ListAll(RoleAccessSearch filter);
    }
    public partial class RoleAccessServices : BaseService<RoleAccessDto, RoleAccess, string>, IRoleAccessServices
    {
        public RoleAccessServices(IEFRepository<RoleAccess, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<RoleAccessDto> ListAll(RoleAccessSearch filter)
        {

            GeneralResponseList<RoleAccessDto> resp = new GeneralResponseList<RoleAccessDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<RoleAccessDto> que = from tb1 in _repo.GetContext().Set<RoleAccess>()
                          select new RoleAccessDto()
                          {
                              Id = tb1.Id,
                              access_activate = tb1.access_activate,
                              access_append = tb1.access_append,
                              access_approve = tb1.access_approve,
                              access_create = tb1.access_create,
                              access_delete = tb1.access_delete,
                              access_level = tb1.access_level,
                              access_lock = tb1.access_lock,
                              access_read = tb1.access_read,
                              access_share = tb1.access_share,
                              access_update = tb1.access_update,
                              application_entity_id = tb1.application_entity_id,
                              application_role_id = tb1.application_role_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "access_activate", filter.Search_access_activate);
                que = q.filterEquals(que, "access_append", filter.Search_access_append);
                que = q.filterEquals(que, "access_approve", filter.Search_access_approve);
                que = q.filterEquals(que, "access_create", filter.Search_access_create);
                que = q.filterEquals(que, "access_delete", filter.Search_access_delete);
                que = q.filterEquals(que, "access_level", filter.Search_access_level);
                que = q.filterEquals(que, "access_lock", filter.Search_access_lock);
                que = q.filterEquals(que, "access_read", filter.Search_access_read);
                que = q.filterEquals(que, "access_share", filter.Search_access_share);
                que = q.filterEquals(que, "access_update", filter.Search_access_update);
                que = q.filterEquals(que, "application_entity_id", filter.Search_application_entity_id);
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

