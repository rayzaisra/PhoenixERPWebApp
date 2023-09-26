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
    public partial interface IApprovalTypeServices : IBaseService<ApprovalTypeDto, ApprovalType, string>
    {
        GeneralResponseList<ApprovalTypeDto> ListAll(ApprovalTypeSearch filter);
    }
    public partial class ApprovalTypeServices : BaseService<ApprovalTypeDto, ApprovalType, string>, IApprovalTypeServices
    {
        public ApprovalTypeServices(IEFRepository<ApprovalType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApprovalTypeDto> ListAll(ApprovalTypeSearch filter)
        {

            GeneralResponseList<ApprovalTypeDto> resp = new GeneralResponseList<ApprovalTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApprovalTypeDto> que = from tb1 in _repo.GetContext().Set<ApprovalType>()
                          select new ApprovalTypeDto()
                          {
                              Id = tb1.Id,
                              approval_type = tb1.approval_type,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "approval_type", filter.Search_approval_type);

                

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

