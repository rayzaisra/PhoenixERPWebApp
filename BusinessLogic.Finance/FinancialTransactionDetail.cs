using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using CommonTool.JEasyUI.DataGrid;
using System.Linq;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Finance
{
    public partial class FinancialTransactionDetail : FinancialTransactionDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public FinancialTransactionDetail(DataContext AppUserData) : base(AppUserData)
        {

        }



        public bool DeleteByParent(string Id, ref int DeletedRecords)
        {
            bool result = false;
            DeletedRecords = 0;

            try
            {
                var records = financial_transaction_detail.Fetch(" WHERE financial_transaction_id=@0 " +
                    "AND COALESCE(is_active,0)=1 AND COALESCE(is_locked,0)=0 AND COALESCE(is_deleted,0)=0 ", Id);

                if (records == null || records.Count ==0) return true; /*Return true jika detail masih kosong*/
                appLogger.Debug("DELETE BY PARENT TIDAK KOSONG");
                result = this.Delete(records.Select(o => o.id).ToArray(), ref DeletedRecords);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public override bool Delete(string Id)
        {
            bool result = false;
            try
            {
                var record = Details(Id);
                if (record != null)
                {
                    if (record.is_locked ?? false)
                    {
                        throw new Exception("Can't delete data submited");
                    }

                    result = Svc.Delete(Id);
                }

            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }


        public override bool Delete(string[] Ids, ref int DeletedRecords)
        {
            bool result = false;
            DeletedRecords = 0;

            try
            {
                foreach (var id in Ids)
                {
                    var record = Details(id);
                    if (record != null)
                    {
                        if (record.is_locked ?? false)
                        {
                            throw new Exception("Can't delete data submited");
                        }
                    }
                }

                result = Svc.Delete(Ids, ref DeletedRecords);

            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }


        public override bool Delete(string[] Ids, Action<string> ActionContinuation = null)
        {
            bool result = false;

            try
            {

                foreach (var id in Ids)
                {
                    appLogger.Info(id);
                    var record = Details(id);


                    if (record != null)
                    {
                        if (record.is_locked ?? false)
                        {
                            appLogger.Debug("Can't delete data submited");
                            throw new Exception("Can't delete data submited");
                        }
                    }
                }
                appLogger.Debug("kok lewat");
                result = Svc.Delete(Ids, ActionContinuation);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DgResponse GetDataGridEasyUI(DgRequest DTRequest, String FinancialTransactionId)
        {
            DgResponse result = null;
            try
            {
                var qry = String.Format(" r.financial_transaction_id ='{0}'", FinancialTransactionId);
                DTRequest.Orders = new List<dgSort>();
                DTRequest.Orders.Add(new dgSort()
                {
                    SortName = "row_index",
                    sortOrder = "ASC"
                });
                result = Context.GetDataGridEasyUI<vw_financial_transaction_detail>(DTRequest, OptionalQueryBuilder: qry);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String FinancialTransactionId)
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.financial_transaction_id ='{0}'", FinancialTransactionId);
                result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
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
