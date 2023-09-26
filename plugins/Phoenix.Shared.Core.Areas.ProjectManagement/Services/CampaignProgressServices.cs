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
    public partial interface ICampaignProgressServices : IBaseService<CampaignProgressDto, CampaignProgress, string>
    {
        GeneralResponseList<CampaignProgressDto> ListAll(CampaignProgressSearch filter);
    }
    public partial class CampaignProgressServices : BaseService<CampaignProgressDto, CampaignProgress, string>, ICampaignProgressServices
    {
        public CampaignProgressServices(IEFRepository<CampaignProgress, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CampaignProgressDto> ListAll(CampaignProgressSearch filter)
        {

            GeneralResponseList<CampaignProgressDto> resp = new GeneralResponseList<CampaignProgressDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CampaignProgressDto> que = from tb1 in _repo.GetContext().Set<CampaignProgress>()
                          select new CampaignProgressDto()
                          {
                              Id = tb1.Id,
                              campaign_progress = tb1.campaign_progress,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "campaign_progress", filter.Search_campaign_progress);

                

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

