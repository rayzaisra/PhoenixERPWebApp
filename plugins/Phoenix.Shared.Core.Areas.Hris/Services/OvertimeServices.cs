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
    public partial interface IOvertimeServices : IBaseService<OvertimeDto, Overtime, string>
    {
        GeneralResponseList<OvertimeDto> ListAll(OvertimeSearch filter);
    }
    public partial class OvertimeServices : BaseService<OvertimeDto, Overtime, string>, IOvertimeServices
    {
        public OvertimeServices(IEFRepository<Overtime, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<OvertimeDto> ListAll(OvertimeSearch filter)
        {

            GeneralResponseList<OvertimeDto> resp = new GeneralResponseList<OvertimeDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<OvertimeDto> que = from tb1 in _repo.GetContext().Set<Overtime>()
                          select new OvertimeDto()
                          {
                              Id = tb1.Id,
                              amount = tb1.amount,
                              approval = tb1.approval,
                              approval_by = tb1.approval_by,
                              approval_date = tb1.approval_date,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              hour_overtime = tb1.hour_overtime,
                              last_update = tb1.last_update,
                              overtime_date = tb1.overtime_date,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "amount", filter.Search_amount);
                que = q.filterContains(que, "approval", filter.Search_approval);
                que = q.filterContains(que, "approval_by", filter.Search_approval_by);
                que = q.filterEquals(que, "approval_date", filter.Search_approval_date);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "hour_overtime", filter.Search_hour_overtime);
                que = q.filterEquals(que, "last_update", filter.Search_last_update);
                que = q.filterEquals(que, "overtime_date", filter.Search_overtime_date);

                

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

