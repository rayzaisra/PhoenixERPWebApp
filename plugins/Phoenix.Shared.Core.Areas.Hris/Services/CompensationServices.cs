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
    public partial interface ICompensationServices : IBaseService<CompensationDto, Compensation, string>
    {
        GeneralResponseList<CompensationDto> ListAll(CompensationSearch filter);
    }
    public partial class CompensationServices : BaseService<CompensationDto, Compensation, string>, ICompensationServices
    {
        public CompensationServices(IEFRepository<Compensation, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CompensationDto> ListAll(CompensationSearch filter)
        {

            GeneralResponseList<CompensationDto> resp = new GeneralResponseList<CompensationDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CompensationDto> que = from tb1 in _repo.GetContext().Set<Compensation>()
                          select new CompensationDto()
                          {
                              Id = tb1.Id,
                              compensation_id = tb1.compensation_id,
                              date_compensation = tb1.date_compensation,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_compensation = tb1.end_compensation,
                              type = tb1.type,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "compensation_id", filter.Search_compensation_id);
                que = q.filterEquals(que, "date_compensation", filter.Search_date_compensation);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_compensation", filter.Search_end_compensation);
                que = q.filterContains(que, "type", filter.Search_type);
                que = q.filterEquals(que, "value", filter.Search_value);

                

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

