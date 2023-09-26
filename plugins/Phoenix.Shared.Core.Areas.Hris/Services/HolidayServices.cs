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
    public partial interface IHolidayServices : IBaseService<HolidayDto, Holiday, string>
    {
        GeneralResponseList<HolidayDto> ListAll(HolidaySearch filter);
    }
    public partial class HolidayServices : BaseService<HolidayDto, Holiday, string>, IHolidayServices
    {
        public HolidayServices(IEFRepository<Holiday, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<HolidayDto> ListAll(HolidaySearch filter)
        {

            GeneralResponseList<HolidayDto> resp = new GeneralResponseList<HolidayDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<HolidayDto> que = from tb1 in _repo.GetContext().Set<Holiday>()
                          select new HolidayDto()
                          {
                              Id = tb1.Id,
                              date = tb1.date,
                              name = tb1.name,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "date", filter.Search_date);
                que = q.filterContains(que, "name", filter.Search_name);

                

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

