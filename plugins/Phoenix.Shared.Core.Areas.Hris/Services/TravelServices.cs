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
    public partial interface ITravelServices : IBaseService<TravelDto, Travel, string>
    {
        GeneralResponseList<TravelDto> ListAll(TravelSearch filter);
    }
    public partial class TravelServices : BaseService<TravelDto, Travel, string>, ITravelServices
    {
        public TravelServices(IEFRepository<Travel, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TravelDto> ListAll(TravelSearch filter)
        {

            GeneralResponseList<TravelDto> resp = new GeneralResponseList<TravelDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TravelDto> que = from tb1 in _repo.GetContext().Set<Travel>()
                          select new TravelDto()
                          {
                              Id = tb1.Id,
                              actual_travel_budget = tb1.actual_travel_budget,
                              expected_travel_budget = tb1.expected_travel_budget,
                              purpose_of_visit = tb1.purpose_of_visit,
                              travel_description = tb1.travel_description,
                              travel_end_date = tb1.travel_end_date,
                              travel_start_date = tb1.travel_start_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "actual_travel_budget", filter.Search_actual_travel_budget);
                que = q.filterEquals(que, "expected_travel_budget", filter.Search_expected_travel_budget);
                que = q.filterContains(que, "purpose_of_visit", filter.Search_purpose_of_visit);
                que = q.filterContains(que, "travel_description", filter.Search_travel_description);
                que = q.filterEquals(que, "travel_end_date", filter.Search_travel_end_date);
                que = q.filterEquals(que, "travel_start_date", filter.Search_travel_start_date);

                

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

