using BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.Finance;
using CommonTool.JEasyUI.DataGrid;

namespace BusinessLogic.Finance.Reports
{
    public class GeneralLedger : PrivateService
    {
        public GeneralLedger(DataContext AppUserData) : base(AppUserData) { }

        //public List<vw_financial_transaction_detail> GetData(DateTime dateTime, 
        //    String transactionNumber,String Account)//, decimal? debit, decimal? credit 
        //{
        //    var results = new List<vw_financial_transaction_detail>();
        //    using (var db = new PhoenixERPRepo())
        //    {
        //        try
        //        {
        //            var qry = financial_transaction_detail.DefaultView;
        //            qry += " WHERE f1.transaction_datetime >=@0 AND f1.transaction_number like @1 AND (a1.account_number like @2 OR a1.account_name like @2 ) ";
        //            results = db.Fetch<vw_financial_transaction_detail>(qry, dateTime, transactionNumber, Account);

        //        }
        //        catch (Exception ex)
        //        {
        //            appLogger.Error(ex);
        //        }
        //    }
        //    return results;
        //}

        public DgResponse GetData(DgRequest DTRequest, DateTime dateTime,
            String transactionNumber, String account)
        {
            DgResponse result = null;
            try
            {
                var qry = String.Format(" f1.transaction_datetime >='{0}' AND f1.transaction_number like '%{1}%' " +
                    "AND (a1.account_number like '%{2}%' OR a1.account_name like '%{2}%' ) ",
                    dateTime, transactionNumber, account);
                DTRequest.Orders = new List<dgSort>();
                DTRequest.PageSize = null; /*Load ALL (Max Integer)*/
                DTRequest.Orders.Add(new dgSort()
                {
                    SortName = "row_index",
                    sortOrder = "ASC"
                });
                result = context.GetDataGridEasyUI<vw_financial_transaction_detail>(DTRequest, OptionalQueryBuilder: qry);
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
