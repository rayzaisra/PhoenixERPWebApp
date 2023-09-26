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
    public partial interface ITravelDestinationServices : IBaseService<TravelDestinationDto, TravelDestination, string>
    {
        GeneralResponseList<TravelDestinationDto> ListAll(TravelDestinationSearch filter);
    }
    public partial class TravelDestinationServices : BaseService<TravelDestinationDto, TravelDestination, string>, ITravelDestinationServices
    {
        public TravelDestinationServices(IEFRepository<TravelDestination, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TravelDestinationDto> ListAll(TravelDestinationSearch filter)
        {

            GeneralResponseList<TravelDestinationDto> resp = new GeneralResponseList<TravelDestinationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TravelDestinationDto> que = from tb1 in _repo.GetContext().Set<TravelDestination>()
                          select new TravelDestinationDto()
                          {
                              Id = tb1.Id,
                              arrangement_type = tb1.arrangement_type,
                              place_of_visit = tb1.place_of_visit,
                              travel_id = tb1.travel_id,
                              travel_mode = tb1.travel_mode,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "arrangement_type", filter.Search_arrangement_type);
                que = q.filterContains(que, "place_of_visit", filter.Search_place_of_visit);
                que = q.filterEquals(que, "travel_id", filter.Search_travel_id);
                que = q.filterContains(que, "travel_mode", filter.Search_travel_mode);

                

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

