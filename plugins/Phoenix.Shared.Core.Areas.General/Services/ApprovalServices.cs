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
    public partial interface IApprovalServices : IBaseService<ApprovalDto, Approval, string>
    {
        GeneralResponseList<ApprovalDto> ListAll(ApprovalSearch filter);
    }
    public partial class ApprovalServices : BaseService<ApprovalDto, Approval, string>, IApprovalServices
    {
        public ApprovalServices(IEFRepository<Approval, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApprovalDto> ListAll(ApprovalSearch filter)
        {

            GeneralResponseList<ApprovalDto> resp = new GeneralResponseList<ApprovalDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApprovalDto> que = from tb1 in _repo.GetContext().Set<Approval>()
                          select new ApprovalDto()
                          {
                              Id = tb1.Id,
                              application_entity_id = tb1.application_entity_id,
                              approval_group_id = tb1.approval_group_id,
                              record_id = tb1.record_id,
                              status = tb1.status,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "application_entity_id", filter.Search_application_entity_id);
                que = q.filterEquals(que, "approval_group_id", filter.Search_approval_group_id);
                que = q.filterEquals(que, "record_id", filter.Search_record_id);
                que = q.filterContains(que, "status", filter.Search_status);

                

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

