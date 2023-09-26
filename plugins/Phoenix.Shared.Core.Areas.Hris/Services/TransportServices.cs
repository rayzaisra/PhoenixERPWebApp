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
    public partial interface ITransportServices : IBaseService<TransportDto, Transport, string>
    {
        GeneralResponseList<TransportDto> ListAll(TransportSearch filter);
    }
    public partial class TransportServices : BaseService<TransportDto, Transport, string>, ITransportServices
    {
        public TransportServices(IEFRepository<Transport, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<TransportDto> ListAll(TransportSearch filter)
        {

            GeneralResponseList<TransportDto> resp = new GeneralResponseList<TransportDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<TransportDto> que = from tb1 in _repo.GetContext().Set<Transport>()
                          select new TransportDto()
                          {
                              Id = tb1.Id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_date = tb1.end_date,
                              start_date = tb1.start_date,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_date", filter.Search_end_date);
                que = q.filterEquals(que, "start_date", filter.Search_start_date);
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

