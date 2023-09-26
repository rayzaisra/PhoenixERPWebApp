using Phoenix.Shared.Core.Areas.General.Dtos;
using Phoenix.Shared.Core.Entities;
using Phoenix.Shared.Core.Parameters;
using Phoenix.Shared.Core.Repositories;
using Phoenix.Shared.Core.Services;
using Phoenix.Shared.Responses;
using System;
using System.Linq;


namespace Phoenix.Shared.Core.Areas.General.Services
{
    public partial interface ICurrencyServices : IBaseService<CurrencyDto, Currency, string>
    {
        GeneralResponseList<CurrencyDto> ListAll(currencySearch filter);
    }
    public partial class CurrencyServices : BaseService<CurrencyDto, Currency, string>, ICurrencyServices
    {
        public CurrencyServices(IEFRepository<Currency, string> repo) : base(repo)
        {
            _repo = repo;
        }
 
        public GeneralResponseList<CurrencyDto> ListAll(currencySearch filter)
        {

            GeneralResponseList<CurrencyDto> resp = new GeneralResponseList<CurrencyDto>();
            try
            {
                PagingHelper q = new PagingHelper();
                IQueryable<CurrencyDto> que = from tb1 in _repo.GetContext().Set<Currency>()
                          select new CurrencyDto()
                          {
                              Id = tb1.Id,
                              currency_code = tb1.currency_code,
                              currency_name = tb1.currency_name,
                              currency_symbol = tb1.currency_symbol,
                          };
                
                resp.RecordsTotal = que.Count();

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

