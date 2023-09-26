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
    public partial interface ICampaignTypeServices : IBaseService<CampaignTypeDto, CampaignType, string>
    {
        GeneralResponseList<CampaignTypeDto> ListAll(CampaignTypeSearch filter);
    }
    public partial class CampaignTypeServices : BaseService<CampaignTypeDto, CampaignType, string>, ICampaignTypeServices
    {
        public CampaignTypeServices(IEFRepository<CampaignType, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CampaignTypeDto> ListAll(CampaignTypeSearch filter)
        {

            GeneralResponseList<CampaignTypeDto> resp = new GeneralResponseList<CampaignTypeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CampaignTypeDto> que = from tb1 in _repo.GetContext().Set<CampaignType>()
                          select new CampaignTypeDto()
                          {
                              Id = tb1.Id,
                              campaign_type = tb1.campaign_type,
                              description = tb1.description,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "campaign_type", filter.Search_campaign_type);
                que = q.filterContains(que, "description", filter.Search_description);

                

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

