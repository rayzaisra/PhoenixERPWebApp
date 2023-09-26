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
    public partial interface IApprovalDetailServices : IBaseService<ApprovalDetailDto, ApprovalDetail, string>
    {
        GeneralResponseList<ApprovalDetailDto> ListAll(ApprovalDetailSearch filter);
    }
    public partial class ApprovalDetailServices : BaseService<ApprovalDetailDto, ApprovalDetail, string>, IApprovalDetailServices
    {
        public ApprovalDetailServices(IEFRepository<ApprovalDetail, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApprovalDetailDto> ListAll(ApprovalDetailSearch filter)
        {

            GeneralResponseList<ApprovalDetailDto> resp = new GeneralResponseList<ApprovalDetailDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApprovalDetailDto> que = from tb1 in _repo.GetContext().Set<ApprovalDetail>()
                          select new ApprovalDetailDto()
                          {
                              Id = tb1.Id,
                              approval_id = tb1.approval_id,
                              approval_level = tb1.approval_level,
                              approval_status_id = tb1.approval_status_id,
                              approver_id = tb1.approver_id,
                              expiry_datetime = tb1.expiry_datetime,
                              is_auto_approved = tb1.is_auto_approved,
                              previous_approval = tb1.previous_approval,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "approval_id", filter.Search_approval_id);
                que = q.filterEquals(que, "approval_level", filter.Search_approval_level);
                que = q.filterEquals(que, "approval_status_id", filter.Search_approval_status_id);
                que = q.filterEquals(que, "approver_id", filter.Search_approver_id);
                que = q.filterEquals(que, "expiry_datetime", filter.Search_expiry_datetime);
                que = q.filterEquals(que, "is_auto_approved", filter.Search_is_auto_approved);
                que = q.filterContains(que, "previous_approval", filter.Search_previous_approval);

                

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

