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
    public partial interface IStatusEmployeeServices : IBaseService<StatusEmployeeDto, StatusEmployee, string>
    {
        GeneralResponseList<StatusEmployeeDto> ListAll(StatusEmployeeSearch filter);
    }
    public partial class StatusEmployeeServices : BaseService<StatusEmployeeDto, StatusEmployee, string>, IStatusEmployeeServices
    {
        public StatusEmployeeServices(IEFRepository<StatusEmployee, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<StatusEmployeeDto> ListAll(StatusEmployeeSearch filter)
        {

            GeneralResponseList<StatusEmployeeDto> resp = new GeneralResponseList<StatusEmployeeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<StatusEmployeeDto> que = from tb1 in _repo.GetContext().Set<StatusEmployee>()
                          select new StatusEmployeeDto()
                          {
                              Id = tb1.Id,
                              compare = tb1.compare,
                              level_position_id = tb1.level_position_id,
                              month = tb1.month,
                              name = tb1.name,
                              ptkp = tb1.ptkp,
                              tax = tb1.tax,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "compare", filter.Search_compare);
                que = q.filterEquals(que, "level_position_id", filter.Search_level_position_id);
                que = q.filterContains(que, "month", filter.Search_month);
                que = q.filterContains(que, "name", filter.Search_name);
                que = q.filterContains(que, "ptkp", filter.Search_ptkp);
                que = q.filterContains(que, "tax", filter.Search_tax);

                

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

