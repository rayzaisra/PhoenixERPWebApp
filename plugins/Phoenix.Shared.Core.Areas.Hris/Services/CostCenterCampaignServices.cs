using Phoenix.Shared.Core.Areas.Hris.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.Hris.Services
{
    public partial interface ICostCenterCampaignServices : IBaseService<CostCenterCampaignDto, CostCenterCampaign, string>
    {
        GeneralResponseList<CostCenterCampaignDto> ListAll(CostCenterCampaignSearch filter);
    }
    public partial class CostCenterCampaignServices : BaseService<CostCenterCampaignDto, CostCenterCampaign, string>, ICostCenterCampaignServices
    {
        public CostCenterCampaignServices(IEFRepository<CostCenterCampaign, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CostCenterCampaignDto> ListAll(CostCenterCampaignSearch filter)
        {

            GeneralResponseList<CostCenterCampaignDto> resp = new GeneralResponseList<CostCenterCampaignDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CostCenterCampaignDto> que = from tb1 in _repo.GetContext().Set<CostCenterCampaign>()
                          select new CostCenterCampaignDto()
                          {
                              Id = tb1.Id,
                              address = tb1.address,
                              cost = tb1.cost,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_date = tb1.end_date,
                              external_client_id = tb1.external_client_id,
                              notes = tb1.notes,
                              phone_no = tb1.phone_no,
                              start_date = tb1.start_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "address", filter.Search_address);
                que = q.filterEquals(que, "cost", filter.Search_cost);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_date", filter.Search_end_date);
                que = q.filterEquals(que, "external_client_id", filter.Search_external_client_id);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterContains(que, "phone_no", filter.Search_phone_no);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);

                

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

