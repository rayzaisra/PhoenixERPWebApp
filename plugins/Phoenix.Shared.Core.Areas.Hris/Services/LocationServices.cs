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
    public partial interface ILocationServices : IBaseService<LocationDto, Location, string>
    {
        GeneralResponseList<LocationDto> ListAll(LocationSearch filter);
    }
    public partial class LocationServices : BaseService<LocationDto, Location, string>, ILocationServices
    {
        public LocationServices(IEFRepository<Location, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<LocationDto> ListAll(LocationSearch filter)
        {

            GeneralResponseList<LocationDto> resp = new GeneralResponseList<LocationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<LocationDto> que = from tb1 in _repo.GetContext().Set<Location>()
                          select new LocationDto()
                          {
                              Id = tb1.Id,
                              client_brief_id = tb1.client_brief_id,
                              lattitude = tb1.lattitude,
                              location_name = tb1.location_name,
                              longitude = tb1.longitude,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "client_brief_id", filter.Search_client_brief_id);
                que = q.filterEquals(que, "lattitude", filter.Search_lattitude);
                que = q.filterContains(que, "location_name", filter.Search_location_name);
                que = q.filterEquals(que, "longitude", filter.Search_longitude);

                

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

