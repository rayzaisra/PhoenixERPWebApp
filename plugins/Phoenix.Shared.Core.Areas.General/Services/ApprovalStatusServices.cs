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
    public partial interface IApprovalStatusServices : IBaseService<ApprovalStatusDto, ApprovalStatus, string>
    {
        GeneralResponseList<ApprovalStatusDto> ListAll(ApprovalStatusSearch filter);
    }
    public partial class ApprovalStatusServices : BaseService<ApprovalStatusDto, ApprovalStatus, string>, IApprovalStatusServices
    {
        public ApprovalStatusServices(IEFRepository<ApprovalStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApprovalStatusDto> ListAll(ApprovalStatusSearch filter)
        {

            GeneralResponseList<ApprovalStatusDto> resp = new GeneralResponseList<ApprovalStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApprovalStatusDto> que = from tb1 in _repo.GetContext().Set<ApprovalStatus>()
                          select new ApprovalStatusDto()
                          {
                              Id = tb1.Id,
                              approval_group_id = tb1.approval_group_id,
                              approval_status = tb1.approval_status,
                              status_level = tb1.status_level,
                              status_name = tb1.status_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "approval_group_id", filter.Search_approval_group_id);
                que = q.filterContains(que, "approval_status", filter.Search_approval_status);
                que = q.filterEquals(que, "status_level", filter.Search_status_level);
                que = q.filterContains(que, "status_name", filter.Search_status_name);

                

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

