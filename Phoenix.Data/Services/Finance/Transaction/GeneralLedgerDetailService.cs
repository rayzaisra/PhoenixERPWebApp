using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Data
{
    public interface IGeneralLedgerDetailService
    {
        Task<List<GeneralLedgerDetail>> GetList(GeneralLedgerDetailSearch model);
    }

    public class GeneralLedgerDetailService : IGeneralLedgerDetailService
    {
        readonly DataContext context;
        public GeneralLedgerDetailService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<GeneralLedgerDetail>> GetList(GeneralLedgerDetailSearch model)
        {
            //    return await context.GeneralLedgerDetails
            //        .Where(x =>
            //                (string.IsNullOrEmpty(model.LegalEntityId) ? true : x.LegalEntityId == model.LegalEntityId) &&
            //                ((model.PeriodeFrom != null && model.PeriodeTo != null) ? (Convert.ToDateTime(x.Referencedate) >= Convert.ToDateTime(model.PeriodeFrom) && Convert.ToDateTime(x.Referencedate) <= Convert.ToDateTime(model.PeriodeTo)) :
            //                (model.PeriodeFrom != null) ? Convert.ToDateTime(x.Referencedate) == Convert.ToDateTime(model.PeriodeFrom) :
            //                (model.PeriodeTo != null) ? Convert.ToDateTime(x.Referencedate) == Convert.ToDateTime(model.PeriodeTo) : true) &&
            //                (string.IsNullOrEmpty(model.EvdNo) ? true : x.ReffNumber == model.EvdNo) &&
            //                (string.IsNullOrEmpty(model.BusinessUnitId) ? true : x.BusinessUnitId == model.BusinessUnitId) &&
            //                (string.IsNullOrEmpty(model.AccountId) ? true : x.AccountId == model.AccountId)
            //        ).ToListAsync();

            var parameter = new SqlParameter[]
            {
                new SqlParameter("@LegalEntityId",(object)model.LegalEntityId??DBNull.Value),
                new SqlParameter("@PeriodeFrom", (object)model.PeriodeFrom??DBNull.Value),
                new SqlParameter("@PeriodeTo", (object)model.PeriodeTo??DBNull.Value),
                new SqlParameter("@EvdNo", (object)model.EvdNo??DBNull.Value),
                new SqlParameter("@BusinessUnitId", (object)model.BusinessUnitId??DBNull.Value),
                new SqlParameter("@Level", (object)model.Level??DBNull.Value),
                new SqlParameter("@AccountId", (object)model.AccountId??DBNull.Value)
            };
            return await context.ExecuteStoredProcedure<GeneralLedgerDetail>("fn.get_general_ledger_detail", parameter);
        }
    }
}
