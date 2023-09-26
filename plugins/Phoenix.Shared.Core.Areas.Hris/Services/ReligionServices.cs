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
    public partial interface IReligionServices : IBaseService<ReligionDto, Religion, string>
    {
        GeneralResponseList<ReligionDto> ListAll(ReligionSearch filter);
    }
    public partial class ReligionServices : BaseService<ReligionDto, Religion, string>, IReligionServices
    {
        public ReligionServices(IEFRepository<Religion, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ReligionDto> ListAll(ReligionSearch filter)
        {

            GeneralResponseList<ReligionDto> resp = new GeneralResponseList<ReligionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ReligionDto> que = from tb1 in _repo.GetContext().Set<Religion>()
                          select new ReligionDto()
                          {
                              Id = tb1.Id,
                              religion_name = tb1.religion_name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "religion_name", filter.Search_religion_name);

                

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

