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
    public partial interface IReimburseServices : IBaseService<ReimburseDto, Reimburse, string>
    {
        GeneralResponseList<ReimburseDto> ListAll(ReimburseSearch filter);
    }
    public partial class ReimburseServices : BaseService<ReimburseDto, Reimburse, string>, IReimburseServices
    {
        public ReimburseServices(IEFRepository<Reimburse, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<ReimburseDto> ListAll(ReimburseSearch filter)
        {

            GeneralResponseList<ReimburseDto> resp = new GeneralResponseList<ReimburseDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<ReimburseDto> que = from tb1 in _repo.GetContext().Set<Reimburse>()
                          select new ReimburseDto()
                          {
                              Id = tb1.Id,
                              amount = tb1.amount,
                              employee_basic_info_id = tb1.employee_basic_info_id,
                              receipt = tb1.receipt,
                              reimburse_date = tb1.reimburse_date,
                              reimburse_description = tb1.reimburse_description,
                              type_reimburse = tb1.type_reimburse,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "amount", filter.Search_amount);
                que = q.filterEquals(que, "employee_basic_info_id", filter.Search_employee_basic_info_id);
                que = q.filterContains(que, "receipt", filter.Search_receipt);
                que = q.filterEquals(que, "reimburse_date", filter.Search_reimburse_date);
                que = q.filterContains(que, "reimburse_description", filter.Search_reimburse_description);
                que = q.filterContains(que, "type_reimburse", filter.Search_type_reimburse);

                

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

