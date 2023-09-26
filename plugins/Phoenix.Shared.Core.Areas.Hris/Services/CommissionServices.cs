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
    public partial interface ICommissionServices : IBaseService<CommissionDto, Commission, string>
    {
        GeneralResponseList<CommissionDto> ListAll(CommissionSearch filter);
    }
    public partial class CommissionServices : BaseService<CommissionDto, Commission, string>, ICommissionServices
    {
        public CommissionServices(IEFRepository<Commission, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CommissionDto> ListAll(CommissionSearch filter)
        {

            GeneralResponseList<CommissionDto> resp = new GeneralResponseList<CommissionDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CommissionDto> que = from tb1 in _repo.GetContext().Set<Commission>()
                          select new CommissionDto()
                          {
                              Id = tb1.Id,
                              amount = tb1.amount,
                              commission_description = tb1.commission_description,
                              date_commission = tb1.date_commission,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              notes = tb1.notes,
                              title = tb1.title,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "amount", filter.Search_amount);
                que = q.filterContains(que, "commission_description", filter.Search_commission_description);
                que = q.filterEquals(que, "date_commission", filter.Search_date_commission);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "notes", filter.Search_notes);
                que = q.filterContains(que, "title", filter.Search_title);

                

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

