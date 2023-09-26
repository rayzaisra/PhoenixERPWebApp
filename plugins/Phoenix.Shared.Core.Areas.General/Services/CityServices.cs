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
    public partial interface ICityServices : IBaseService<CityDto, City, string>
    {
        GeneralResponseList<CityDto> ListAll(CitySearch filter);
    }
    public partial class CityServices : BaseService<CityDto, City, string>, ICityServices
    {
        public CityServices(IEFRepository<City, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CityDto> ListAll(CitySearch filter)
        {

            GeneralResponseList<CityDto> resp = new GeneralResponseList<CityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CityDto> que = from tb1 in _repo.GetContext().Set<City>()
                          select new CityDto()
                          {
                              Id = tb1.Id,
                              city_code = tb1.city_code,
                              city_name = tb1.city_name,
                              country_id = tb1.country_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "city_code", filter.Search_city_code);
                que = q.filterContains(que, "city_name", filter.Search_city_name);
                que = q.filterEquals(que, "country_id", filter.Search_country_id);

                

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

