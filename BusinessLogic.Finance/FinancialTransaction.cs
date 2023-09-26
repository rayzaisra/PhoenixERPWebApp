using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP;
using NPoco;
using DataAccess;
using Newtonsoft.Json;
using DataTables.AspNet.Core;
using DataTables.AspNet.AspNetCore;
using System.Linq;
using System.Data.SqlClient;

namespace BusinessLogic.Finance
{
    public partial class FinancialTransaction : FinancialTransactionService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public FinancialTransaction(DataContext AppUserData) : base(AppUserData)
        {

        }
        public override bool Delete(string Id)
        {
            bool result = false;
            var DeletedDetailRecords = 0;
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

                    var detailService = new FinancialTransactionDetail(Context);
                    result &= detailService.DeleteByParent(Id, ref DeletedDetailRecords);
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
            var DeletedDetailRecords = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
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

                            var detailService = new FinancialTransactionDetail(Context);
                            result = detailService.DeleteByParent(record.id, ref DeletedDetailRecords);
                            appLogger.Debug("STATUS DELETE DETAIL: " + result);
                            result &= Svc.Delete(Ids, ref DeletedRecords);
                            appLogger.Debug("STATUS DELETE HEADER: " + result);
                        }
                    }
                    appLogger.Debug("STATUS DELETE : " + result);
                    if (result)
                    {
                        scope.Complete();
                    }

                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public bool SaveToDraft(FinancialTransactionDetails RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        
                        var _transactionDetails = RecordDto.details;
                        var _transaction = RecordDto.transaction;
                        if (_transactionDetails == null) return result;

                        if (!string.IsNullOrEmpty(_transaction.id))
                        {
                            var oldRecord = financial_transaction.FirstOrDefault("WHERE id=@0 ", _transaction.id);

                            //legal entity tidak boleh berubah, jaga jaga aje
                            _transaction.legal_entity_id = oldRecord.legal_entity_id;
                        }
                        else
                        {
                            _transaction.organization_id = Context.OrganizationId;
                            _transaction.transaction_number = 
                                new FinancialTransaction(Context).GetNextAutoNumber();
                        }

                        if (_transaction.is_internal ?? false == true)
                        {
                            _transaction.company_id = string.Empty;
                        }

                        result = Svc.SaveEntity(_transaction, ref isNew);

                        if (result)
                        {
                            #region Check active financial year

                            var qry = "WHERE is_active = 1 AND legal_entity_id = @0 AND start_date <= @1 AND end_date >= @1";
                            var financialYear = financial_year.FirstOrDefault(qry,
                                _transaction.legal_entity_id, _transaction.transaction_datetime);
                            result &= financialYear != null;

                            if (result)
                            {
                                _transaction.financial_year_id = financialYear?.id;
                            }
                            else
                            {
                                throw new Exception("Active financial year is not defined.");
                            }

                            #endregion

                            var _transactionDetailService = new FinancialTransactionDetailService(Context);
                            decimal debitTotal = 0;
                            decimal creditTotal = 0;

                            var rowIndex = 1;
                            foreach (var record in _transactionDetails)
                            {
                                if (record == null) continue;

                                #region Make sure Account having current Legal Entity
                                var accountMatch = db.ExecuteScalar<int>("SELECT COUNT(*) FROM fn.account WHERE id=@0 AND legal_entity_id=@1", record.account_id, _transaction.legal_entity_id);
                                if (accountMatch != 1) throw new Exception("Account does't Match on Legal Entity");

                                #endregion

                                if (record.debit > 0 && record.credit > 0)
                                    throw new Exception("Cannot insert debit and credit for single account");

                                var isNewDetails = false;
                                record.financial_transaction_id = _transaction.id;


                                if (record.is_internal ?? false == true)
                                {
                                    record.company_id = null;
                                    record.company_name = null;
                                }
                                else
                                {
                                    record.company_id = string.IsNullOrEmpty(record.company_id) ?
                                    _transaction.company_id : record.company_id;

                                    record.company_name = string.IsNullOrEmpty(record.company_name) ?
                                    _transaction.company_name : record.company_name;
                                }

                                record.business_unit_id = string.IsNullOrEmpty(record.business_unit_id) ?
                                    _transaction.business_unit_id : record.business_unit_id;
                                record.affiliation_id = string.IsNullOrEmpty(record.affiliation_id) ?
                                    _transaction.affiliation_id : record.affiliation_id;

                                record.legal_entity_id = _transaction.legal_entity_id;
                                record.row_index = rowIndex;
                                appLogger.Debug("START TDETAIL");
                                appLogger.Debug(JsonConvert.SerializeObject(record));
                                appLogger.Debug("END TDETAIL");
                                result &= _transactionDetailService.SaveEntity(record, ref isNewDetails);

                                if (!result)
                                    throw new Exception("Fail to insert transaction detail");

                                debitTotal += record.normalized_debit ?? 0;
                                creditTotal += record.normalized_credit ?? 0;
                                rowIndex++;
                            }

                            result &= debitTotal == creditTotal;
                            if (!result)
                                throw new Exception("Total debit and credit is not balance");
                        }

                        if (result)
                        {
                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        appLogger.Error(ex);

                        if (ex.Message.Contains("transaction_number"))
                            throw new Exception("Self Reference does not exist in database !");


                        if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                            throw new Exception("Can't duplicate data");

                        throw;
                    }

                }
    
            }
            return result;
        }

        public bool SubmitTransaction(String RecordId, ref bool isNew)
        {
            bool result = false;

            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var record = financial_transaction.FirstOrDefault(" WHERE id=@0", RecordId);
                        if (record == null)
                            throw new Exception("Record not found");

                        var _transactionDetailService = new FinancialTransactionDetailService(Context);
                        var details = financial_transaction_detail.Fetch(
                            " WHERE financial_transaction_id=@0 AND COALESCE(is_deleted,0)=0", 
                            record.id);
                        if (details == null)
                            throw new Exception("Canceled submiting data, you don't have transaction detail");

                    result = (details.Select(o => o.normalized_debit ?? 0 ).Sum() == details.Select(o => o.normalized_credit ?? 0).Sum());
                    if (!result)
                        throw new Exception("Total Debit and Credit are not balance");

                        record.is_locked = true;
                        var recordDTO = new financial_transactionDTO();
                        recordDTO.InjectFrom(record);
                        result = Svc.SaveEntity(recordDTO, ref isNew);

                        if (result)
                        {
                            var fttBeginningBalance = financial_transaction_type.FirstOrDefault(
                                "WHERE organization_id = @0 AND transaction_type = @1",
                                Context.OrganizationId, financial_transaction_type.BEGINNING_BALANCE);

                            var detailsDTO = new List<financial_transaction_detailDTO>();
                            detailsDTO = details
                                .Select(x => new financial_transaction_detailDTO().InjectFrom(x))
                                .Cast<financial_transaction_detailDTO>()
                                .ToList();

                            var accountsWithoutBeginningBalance = new List<string>();
                            foreach (var detail in detailsDTO)
                            {
                                detail.is_locked = true;
                                result &= _transactionDetailService.Svc.SaveEntity(detail, ref isNew);

                                if(recordDTO.transaction_type_id != fttBeginningBalance.id)
                                {
                                    #region Check if current account has beginning balance

                                    var acc = account.FirstOrDefault(
                                        "WHERE id = @0 AND [fn].[ufn_has_beginning_balance](id) = 0",
                                        detail.account_id);
                                    appLogger.Debug(db.LastCommand);

                                    result &= (acc == null);

                                    if (acc != null)
                                    {
                                        accountsWithoutBeginningBalance.Add(acc.account_number);
                                    }

                                    #endregion
                                }
                            }

                            if (!result && accountsWithoutBeginningBalance.Count > 0)
                            {
                                throw new Exception(string.Format(
                                    "Following account(s) has no beginning balance : \r\n{0}",
                                    string.Join(',', accountsWithoutBeginningBalance.ToArray())));
                            }
                        }

                        if (result)
                        {
                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        appLogger.Error(ex);
                        appLogger.Error(db.LastCommand);
                        throw;
                    }
                }
            }

            return result;
        }

        public dynamic Lookup(string Id = null, int page = 1, int length = 100, 
            string optionalFilters = null, string optionalParameters = null)
        {
            dynamic result = null;
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters
                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in financial_transaction.DefaultViewColumns)
                        {
                            filters = optionalFilters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                        }
                        filters = filters.Replace(";", "--");
                    }

                    parameters = optionalParameters;
                    int pageDraw = page;
                    int rowPerPage = length;

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var sql = Sql.Builder.Append(financial_transaction.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            if (!String.IsNullOrEmpty(filters))
                            {
                                if (String.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.transaction_number ");
                            result = db.Page<vw_financial_transaction>(page, rowPerPage, sql);

                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex);
                        }
                        finally
                        {
                            appLogger.Debug(db.LastCommand);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Use Id
                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            int rowPerPage = 10;
                            var sql = Sql.Builder.Append(financial_transaction.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.transaction_number ");
                            result = db.Page<vw_financial_transaction>(1, rowPerPage, sql);

                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex.ToString());
                        }
                        finally
                        {
                            appLogger.Debug(db.LastCommand);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataTablesResponseNetCore GetListDataTablesCoaBalance(IDataTablesRequest DTRequest)
        {
            DataTablesResponseNetCore result = null;

            try
            {
                var OptionalFilters = " ( ft.transaction_type like @0 ) ";
                var OptionalParameters = "%balance%";
                result = Context.GetListDataTablesMarvelBinder<vw_financial_transaction>(
                    DTRequest, OptionalFilters, OptionalParameters);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataTablesResponseNetCore GetListDataTablesAdjustment(IDataTablesRequest DTRequest)
        {
            DataTablesResponseNetCore result = null;

            try
            {
                var OptionalFilters = " ft.transaction_type LIKE @0 AND r.self_reference IS NOT NULL";
                var OptionalParameters = "%mutation%";
                result = Context.GetListDataTablesMarvelBinder<vw_financial_transaction>(
                    DTRequest, OptionalFilters, OptionalParameters);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public string GetNextAutoNumber()
        {
            return Context.GenerateAutoNumber("[fn].[sq_financial_transaction]", "TX");
        }
    }
}
