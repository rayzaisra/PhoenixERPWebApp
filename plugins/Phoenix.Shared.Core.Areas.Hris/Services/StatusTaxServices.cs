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
    public partial interface IStatusTaxServices : IBaseService<StatusTaxDto, StatusTax, string>
    {
        GeneralResponseList<StatusTaxDto> ListAll(StatusTaxSearch filter);
    }
    public partial class StatusTaxServices : BaseService<StatusTaxDto, StatusTax, string>, IStatusTaxServices
    {
        public StatusTaxServices(IEFRepository<StatusTax, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<StatusTaxDto> ListAll(StatusTaxSearch filter)
        {

            GeneralResponseList<StatusTaxDto> resp = new GeneralResponseList<StatusTaxDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<StatusTaxDto> que = from tb1 in _repo.GetContext().Set<StatusTax>()
                          select new StatusTaxDto()
                          {
                              Id = tb1.Id,
                              master_tax_id = tb1.master_tax_id,
                              tax_status = tb1.tax_status,
                          };
                
                resp.RecordsTotal = que.Count();

                                que = q.filterEquals(que, "master_tax_id", filter.Search_master_tax_id);
                que = q.filterContains(que, "tax_status", filter.Search_tax_status);

                

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

