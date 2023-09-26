using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace Phoenix.Data
{
    public interface IRptStatementAPService
    {
        Task<List<VwStatementOfAp>> Get(VwStatementOfApFilter Filter);
    }
    public class RptStatementAPService : IRptStatementAPService
    {
        readonly DataContext context;
        public RptStatementAPService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<VwStatementOfAp>> Get(VwStatementOfApFilter Filter)
        {
            var result = await context.VwStatementOfAp
                        .Where(x => (string.IsNullOrEmpty(Filter.CoaId) ? true : x.CoaId == Filter.CoaId) &&
                                    (string.IsNullOrEmpty(Filter.VendorId) ? true : x.VendorId == Filter.VendorId) &&
                                    (string.IsNullOrEmpty(Filter.LegalId) ? true : x.LegalId == Filter.LegalId) &&
                                    (string.IsNullOrEmpty(Filter.CurrencyId) ? true : x.CurrencyId == Filter.CurrencyId) &&
                                    (string.IsNullOrEmpty(Filter.MainserviceCategoryId) ? true : x.MainserviceCategoryId == Filter.MainserviceCategoryId) &&
                                    (string.IsNullOrEmpty(Filter.JobId) ? true : x.JobId == Filter.JobId) &&
                                    (string.IsNullOrEmpty(Filter.TypeOfExpenseId) ? true : x.TypeOfExpenseId == Filter.TypeOfExpenseId) &&
                                    (Filter.OriginalAmount == null ? true : x.OriginalAmount == Filter.OriginalAmount) &&
                                    ((Filter.BeginDT != null && Filter.EndDT != null) ? (x.TransactionDate >= Filter.BeginDT && x.TransactionDate <= Filter.EndDT) :
                                    (Filter.BeginDT != null) ? (x.TransactionDate >= Filter.BeginDT) :
                                    (Filter.EndDT != null) ? (x.TransactionDate >= Filter.EndDT) : true)).ToListAsync();
            return result;
        }
    }
}
