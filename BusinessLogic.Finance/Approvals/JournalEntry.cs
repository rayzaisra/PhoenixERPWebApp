using BusinessLogic.Services.General;
using DataAccess.PhoenixERP.Finance;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Finance.Approvals
{
    public class JournalEntryApprovalService : ApprovalService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public JournalEntryApprovalService(DataContext AppUserData) : base(AppUserData){}


        public DataTablesResponseNetCore GetRequested(IDataTablesRequest DTRequest, 
            String OptionalFilters="",
            String OptionalParameters="",
            String OptionalQueryBuilder=""
            )
        {
            DataTablesResponseNetCore result = null;
             
            try
            {
                OptionalFilters = " dbo.ufn_get_approval_status(r.id,'REQUESTED') = 1 AND "; // Filter By Role Access
                OptionalFilters += " r.application_entity_id = @0 ";
                OptionalParameters = financial_transaction.EntityId;
                result = Context.GetListDataTablesMarvelBinderCustom<vw_approval_financial_transaction>(DTRequest,
                    OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }
    }
}
