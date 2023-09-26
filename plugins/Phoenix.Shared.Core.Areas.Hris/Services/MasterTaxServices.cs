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
    public partial interface IMasterTaxServices : IBaseService<MasterTaxDto, MasterTax, string>
    {
        GeneralResponseList<MasterTaxDto> ListAll(MasterTaxSearch filter);
    }
    public partial class MasterTaxServices : BaseService<MasterTaxDto, MasterTax, string>, IMasterTaxServices
    {
        public MasterTaxServices(IEFRepository<MasterTax, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<MasterTaxDto> ListAll(MasterTaxSearch filter)
        {

            GeneralResponseList<MasterTaxDto> resp = new GeneralResponseList<MasterTaxDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<MasterTaxDto> que = from tb1 in _repo.GetContext().Set<MasterTax>()
                          select new MasterTaxDto()
                          {
                              Id = tb1.Id,
                              bpjs_tk = tb1.bpjs_tk,
                              bruto_calculate = tb1.bruto_calculate,
                              calculatetax = tb1.calculatetax,
                              code = tb1.code,
                              percent_value = tb1.percent_value,
                              tax_name = tb1.tax_name,
                              tax_no = tb1.tax_no,
                              type = tb1.type,
                              value = tb1.value,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterContains(que, "bpjs_tk", filter.Search_bpjs_tk);
                que = q.filterContains(que, "bruto_calculate", filter.Search_bruto_calculate);
                que = q.filterContains(que, "calculatetax", filter.Search_calculatetax);
                que = q.filterContains(que, "code", filter.Search_code);
                que = q.filterContains(que, "percent_value", filter.Search_percent_value);
                que = q.filterContains(que, "tax_name", filter.Search_tax_name);
                que = q.filterContains(que, "tax_no", filter.Search_tax_no);
                que = q.filterContains(que, "type", filter.Search_type);
                que = q.filterContains(que, "value", filter.Search_value);

                

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

