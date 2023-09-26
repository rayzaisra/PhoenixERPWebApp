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
    public partial interface IQuotationVendorServices : IBaseService<QuotationVendorDto, QuotationVendor, string>
    {
        GeneralResponseList<QuotationVendorDto> ListAll(QuotationVendorSearch filter);
    }
    public partial class QuotationVendorServices : BaseService<QuotationVendorDto, QuotationVendor, string>, IQuotationVendorServices
    {
        public QuotationVendorServices(IEFRepository<QuotationVendor, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<QuotationVendorDto> ListAll(QuotationVendorSearch filter)
        {

            GeneralResponseList<QuotationVendorDto> resp = new GeneralResponseList<QuotationVendorDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<QuotationVendorDto> que = from tb1 in _repo.GetContext().Set<QuotationVendor>()
                          select new QuotationVendorDto()
                          {
                              Id = tb1.Id,
                              delivery_place = tb1.delivery_place,
                              delivery_time = tb1.delivery_time,
                              downpayment = tb1.downpayment,
                              expiry_date = tb1.expiry_date,
                              external_id = tb1.external_id,
                              lead_time = tb1.lead_time,
                              noted = tb1.noted,
                              payment_id = tb1.payment_id,
                              pe_bidding_id = tb1.pe_bidding_id,
                              price = tb1.price,
                              qty = tb1.qty,
                              quotation_date = tb1.quotation_date,
                              rfq_number = tb1.rfq_number,
                              task_id = tb1.task_id,
                              transaction_type_id = tb1.transaction_type_id,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "delivery_place", filter.Search_delivery_place);
                que = q.filterEquals(que, "delivery_time", filter.Search_delivery_time);
                que = q.filterEquals(que, "downpayment", filter.Search_downpayment);
                que = q.filterEquals(que, "expiry_date", filter.Search_expiry_date);
                que = q.filterEquals(que, "external_id", filter.Search_external_id);
                que = q.filterEquals(que, "lead_time", filter.Search_lead_time);
                que = q.filterContains(que, "noted", filter.Search_noted);
                que = q.filterEquals(que, "payment_id", filter.Search_payment_id);
                que = q.filterEquals(que, "pe_bidding_id", filter.Search_pe_bidding_id);
                que = q.filterEquals(que, "price", filter.Search_price);
                que = q.filterEquals(que, "qty", filter.Search_qty);
                que = q.filterEquals(que, "quotation_date", filter.Search_quotation_date);
                que = q.filterContains(que, "rfq_number", filter.Search_rfq_number);
                que = q.filterEquals(que, "task_id", filter.Search_task_id);
                que = q.filterEquals(que, "transaction_type_id", filter.Search_transaction_type_id);

                

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

