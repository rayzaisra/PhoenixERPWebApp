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
    public partial interface ICampaignStatusServices : IBaseService<CampaignStatusDto, CampaignStatus, string>
    {
        GeneralResponseList<CampaignStatusDto> ListAll(CampaignStatusSearch filter);
    }
    public partial class CampaignStatusServices : BaseService<CampaignStatusDto, CampaignStatus, string>, ICampaignStatusServices
    {
        public CampaignStatusServices(IEFRepository<CampaignStatus, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CampaignStatusDto> ListAll(CampaignStatusSearch filter)
        {

            GeneralResponseList<CampaignStatusDto> resp = new GeneralResponseList<CampaignStatusDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CampaignStatusDto> que = from tb1 in _repo.GetContext().Set<CampaignStatus>()
                          select new CampaignStatusDto()
                          {
                              Id = tb1.Id,
                              campaign_status = tb1.campaign_status,
                              description = tb1.description,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "campaign_status", filter.Search_campaign_status);
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

