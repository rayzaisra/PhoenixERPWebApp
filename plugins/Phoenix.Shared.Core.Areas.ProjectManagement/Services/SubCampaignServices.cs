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
    public partial interface ISubCampaignServices : IBaseService<SubCampaignDto, SubCampaign, string>
    {
        GeneralResponseList<SubCampaignDto> ListAll(SubCampaignSearch filter);
    }
    public partial class SubCampaignServices : BaseService<SubCampaignDto, SubCampaign, string>, ISubCampaignServices
    {
        public SubCampaignServices(IEFRepository<SubCampaign, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<SubCampaignDto> ListAll(SubCampaignSearch filter)
        {

            GeneralResponseList<SubCampaignDto> resp = new GeneralResponseList<SubCampaignDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<SubCampaignDto> que = from tb1 in _repo.GetContext().Set<SubCampaign>()
                          select new SubCampaignDto()
                          {
                              Id = tb1.Id,
                              client_brief_id = tb1.client_brief_id,
                              job_posting_requisition_id = tb1.job_posting_requisition_id,
                              subcampaign = tb1.subcampaign,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterEquals(que, "job_posting_requisition_id", filter.Search_job_posting_requisition_id);
                que = q.filterContains(que, "subcampaign", filter.Search_subcampaign);

                

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

