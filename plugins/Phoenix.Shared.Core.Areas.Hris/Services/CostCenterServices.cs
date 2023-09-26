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
    public partial interface ICostCenterServices : IBaseService<CostCenterDto, CostCenter, string>
    {
        GeneralResponseList<CostCenterDto> ListAll(CostCenterSearch filter);
    }
    public partial class CostCenterServices : BaseService<CostCenterDto, CostCenter, string>, ICostCenterServices
    {
        public CostCenterServices(IEFRepository<CostCenter, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CostCenterDto> ListAll(CostCenterSearch filter)
        {

            GeneralResponseList<CostCenterDto> resp = new GeneralResponseList<CostCenterDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CostCenterDto> que = from tb1 in _repo.GetContext().Set<CostCenter>()
                          select new CostCenterDto()
                          {
                              Id = tb1.Id,
                              activity_id = tb1.activity_id,
                              cost_id = tb1.cost_id,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              end_periode = tb1.end_periode,
                              percentage = tb1.percentage,
                              periode = tb1.periode,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "activity_id", filter.Search_activity_id);
                que = q.filterEquals(que, "cost_id", filter.Search_cost_id);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterEquals(que, "end_periode", filter.Search_end_periode);
                que = q.filterContains(que, "percentage", filter.Search_percentage);
                que = q.filterEquals(que, "periode", filter.Search_periode);

                

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

