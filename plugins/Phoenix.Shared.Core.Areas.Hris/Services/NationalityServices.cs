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
    public partial interface INationalityServices : IBaseService<NationalityDto, Nationality, string>
    {
        GeneralResponseList<NationalityDto> ListAll(NationalitySearch filter);
    }
    public partial class NationalityServices : BaseService<NationalityDto, Nationality, string>, INationalityServices
    {
        public NationalityServices(IEFRepository<Nationality, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<NationalityDto> ListAll(NationalitySearch filter)
        {

            GeneralResponseList<NationalityDto> resp = new GeneralResponseList<NationalityDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<NationalityDto> que = from tb1 in _repo.GetContext().Set<Nationality>()
                          select new NationalityDto()
                          {
                              Id = tb1.Id,
                              nationality_name = tb1.nationality_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "nationality_name", filter.Search_nationality_name);

                

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

