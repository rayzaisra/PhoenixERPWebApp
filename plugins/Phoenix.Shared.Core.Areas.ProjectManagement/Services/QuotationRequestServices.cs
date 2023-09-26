using Phoenix.Shared.Core.Areas.ProjectManagement.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.ProjectManagement.Services
{
    public partial interface IQuotationRequestServices : IBaseService<QuotationRequestDto, QuotationRequest, string>
    {
        GeneralResponseList<QuotationRequestDto> ListAll(QuotationRequestSearch filter);
    }
    public partial class QuotationRequestServices : BaseService<QuotationRequestDto, QuotationRequest, string>, IQuotationRequestServices
    {
        public QuotationRequestServices(IEFRepository<QuotationRequest, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<QuotationRequestDto> ListAll(QuotationRequestSearch filter)
        {

            GeneralResponseList<QuotationRequestDto> resp = new GeneralResponseList<QuotationRequestDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<QuotationRequestDto> que = from tb1 in _repo.GetContext().Set<QuotationRequest>()
                          select new QuotationRequestDto()
                          {
                              Id = tb1.Id,
                              closing_date = tb1.closing_date,
                              company_id = tb1.company_id,
                              days = tb1.days,
                              delivery_date = tb1.delivery_date,
                              delivery_place = tb1.delivery_place,
                              fq_date = tb1.fq_date,
                              payment_id = tb1.payment_id,
                              remarks = tb1.remarks,
                              shift = tb1.shift,
                              task_id = tb1.task_id,
                              transaction_type_id = tb1.transaction_type_id,
                              weeks = tb1.weeks,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "closing_date", filter.Search_closing_date);
                que = q.filterEquals(que, "company_id", filter.Search_company_id);
                que = q.filterEquals(que, "days", filter.Search_days);
                que = q.filterEquals(que, "delivery_date", filter.Search_delivery_date);
                que = q.filterContains(que, "delivery_place", filter.Search_delivery_place);
                que = q.filterEquals(que, "fq_date", filter.Search_fq_date);
                que = q.filterEquals(que, "payment_id", filter.Search_payment_id);
                que = q.filterEquals(que, "remarks", filter.Search_remarks);
                que = q.filterEquals(que, "shift", filter.Search_shift);
                que = q.filterEquals(que, "task_id", filter.Search_task_id);
                que = q.filterEquals(que, "transaction_type_id", filter.Search_transaction_type_id);
                que = q.filterEquals(que, "weeks", filter.Search_weeks);

                

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

