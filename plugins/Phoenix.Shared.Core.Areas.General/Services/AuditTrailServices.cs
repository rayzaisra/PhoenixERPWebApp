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
    public partial interface IAuditTrailServices : IBaseService<AuditTrailDto, AuditTrail, string>
    {
        GeneralResponseList<AuditTrailDto> ListAll(AuditTrailSearch filter);
    }
    public partial class AuditTrailServices : BaseService<AuditTrailDto, AuditTrail, string>, IAuditTrailServices
    {
        public AuditTrailServices(IEFRepository<AuditTrail, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<AuditTrailDto> ListAll(AuditTrailSearch filter)
        {

            GeneralResponseList<AuditTrailDto> resp = new GeneralResponseList<AuditTrailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<AuditTrailDto> que = from tb1 in _repo.GetContext().Set<AuditTrail>()
                          select new AuditTrailDto()
                          {
                              Id = tb1.Id,
                              application_entity_id = tb1.application_entity_id,
                              application_user_id = tb1.application_user_id,
                              app_fullname = tb1.app_fullname,
                              app_username = tb1.app_username,
                              new_record = tb1.new_record,
                              new_view = tb1.new_view,
                              old_record = tb1.old_record,
                              old_view = tb1.old_view,
                              organization_id = tb1.organization_id,
                              organization_name = tb1.organization_name,
                              record_id = tb1.record_id,
                              user_action = tb1.user_action,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_entity_id", filter.Search_application_entity_id);
                que = q.filterEquals(que, "application_user_id", filter.Search_application_user_id);
                que = q.filterContains(que, "app_fullname", filter.Search_app_fullname);
                que = q.filterContains(que, "app_username", filter.Search_app_username);
                que = q.filterContains(que, "new_record", filter.Search_new_record);
                que = q.filterContains(que, "new_view", filter.Search_new_view);
                que = q.filterContains(que, "old_record", filter.Search_old_record);
                que = q.filterContains(que, "old_view", filter.Search_old_view);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);
                que = q.filterContains(que, "organization_name", filter.Search_organization_name);
                que = q.filterEquals(que, "record_id", filter.Search_record_id);
                que = q.filterContains(que, "user_action", filter.Search_user_action);

                

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

