using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;
using System.Transactions;

namespace BusinessLogic.Services
{
    public partial class RequestForQuotationVendor : RequestForQuotationVendorService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public RequestForQuotationVendor(DataContext AppUserData) : base(AppUserData) { }

        public bool DeletePermanent(string id)
        {
            var result = false;
            try
            {
                result = request_for_quotation_vendor.Delete(id) > 0;
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
            }
            return result;
        }

        public bool DeleteChildPermanent(string requestForQuotationId)
        {
            var result = false;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = Sql.Builder.Append($"DELETE FROM {request_for_quotation_vendor.GetTableName()} WHERE request_for_quotation_id=@0 ", requestForQuotationId);
                    int affected = 0;
                    affected = db.Execute(qry);
                    result = affected >= 0;
                    appLogger.Debug($"Deleted all RFQ vendor list id:{requestForQuotationId}, affected {affected} rows");

                    if (result)
                    {
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }

            return result;
        }

    }
}
