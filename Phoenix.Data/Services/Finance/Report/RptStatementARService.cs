using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface IRptStatementARService
    {
        Task<List<VwStatementOfAr>> Get(VwStatementOfArFilter Filter);
    }
    public class RptStatementARService : IRptStatementARService
    {
        readonly DataContext context;
        public RptStatementARService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<VwStatementOfAr>> Get(VwStatementOfArFilter Filter)
        {
            var result = await context.VwStatementOfAr
                        .Where(x => (string.IsNullOrEmpty(Filter.CoaId) ? true : x.CoaId == Filter.CoaId) &&
                                    (string.IsNullOrEmpty(Filter.ClientId) ? true : x.ClientId == Filter.ClientId) &&
                                    (string.IsNullOrEmpty(Filter.LegalId) ? true : x.LegalId == Filter.LegalId) &&
                                    (string.IsNullOrEmpty(Filter.CurrencyId) ? true : x.CurrencyId == Filter.CurrencyId) && (string.IsNullOrEmpty(Filter.MainserviceCategoryId) ? true : x.MainserviceCategoryId == Filter.MainserviceCategoryId) &&
                                    (string.IsNullOrEmpty(Filter.JobId) ? true : x.JobId.Contains(Filter.JobId)) &&
                                    (string.IsNullOrEmpty(Filter.TypeOfExpenseId) ? true : x.TypeOfExpenseId.Contains(Filter.TypeOfExpenseId)) &&
                                    (Filter.OriginalAmount == null ? true : x.OriginalAmount == Filter.OriginalAmount) &&
                                    ((Filter.BeginDT != null && Filter.EndDT != null) ? (x.TransactionDate >= Filter.BeginDT && x.TransactionDate <= Filter.EndDT) :
                                    (Filter.BeginDT != null) ? (x.TransactionDate >= Filter.BeginDT) :
                                    (Filter.EndDT != null) ? (x.TransactionDate >= Filter.EndDT) : true)).ToListAsync();
            return result;
        }
    }
}
