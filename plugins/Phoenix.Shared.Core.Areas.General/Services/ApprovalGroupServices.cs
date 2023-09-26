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
    public partial interface IApprovalGroupServices : IBaseService<ApprovalGroupDto, ApprovalGroup, string>
    {
        GeneralResponseList<ApprovalGroupDto> ListAll(ApprovalGroupSearch filter);
    }
    public partial class ApprovalGroupServices : BaseService<ApprovalGroupDto, ApprovalGroup, string>, IApprovalGroupServices
    {
        public ApprovalGroupServices(IEFRepository<ApprovalGroup, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ApprovalGroupDto> ListAll(ApprovalGroupSearch filter)
        {

            GeneralResponseList<ApprovalGroupDto> resp = new GeneralResponseList<ApprovalGroupDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ApprovalGroupDto> que = from tb1 in _repo.GetContext().Set<ApprovalGroup>()
                          select new ApprovalGroupDto()
                          {
                              Id = tb1.Id,
                              group_name = tb1.group_name,
                              organization_id = tb1.organization_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "group_name", filter.Search_group_name);
                que = q.filterEquals(que, "organization_id", filter.Search_organization_id);

                

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

