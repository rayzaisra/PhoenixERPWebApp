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
    public partial interface ICountryServices : IBaseService<CountryDto, Country, string>
    {
        GeneralResponseList<CountryDto> ListAll(CountrySearch filter);
    }
    public partial class CountryServices : BaseService<CountryDto, Country, string>, ICountryServices
    {
        public CountryServices(IEFRepository<Country, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CountryDto> ListAll(CountrySearch filter)
        {

            GeneralResponseList<CountryDto> resp = new GeneralResponseList<CountryDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CountryDto> que = from tb1 in _repo.GetContext().Set<Country>()
                          select new CountryDto()
                          {
                              Id = tb1.Id,
                              country_code = tb1.country_code,
                              country_name = tb1.country_name,
                              organization_id = tb1.organization_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "country_code", filter.Search_country_code);
                que = q.filterContains(que, "country_name", filter.Search_country_name);
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

