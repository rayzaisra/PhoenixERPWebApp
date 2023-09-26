using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.General;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using DataAccess.Models.Entity.Custom.Finance;
using NPoco;
using System.Transactions;
using Omu.ValueInjecter;
using Newtonsoft.Json;
using CommonTool.JEasyUI.DataGrid;
using CommonTool.KendoUI.Grid;
using BusinessLogic.Finance;

namespace BusinessLogic.Services
{
    public partial class Account : AccountService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Account(DataContext AppUserData) : base(AppUserData)
        {

        }

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(account.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_account>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.account_number");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_account>(pageDraw, rowPerPage, sql);
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
            }
            return result;
        }

        public bool SingleUpdateCoaBalance(vw_coa_balanceDTO record)
        {
            var result = false;
            using (var db = new PhoenixERPRepo())
                try
                {
                    if (record == null) return false;

                    var _oldRecord = financial_transaction_detail.FirstOrDefault(" WHERE id=@0", record.financial_transaction_id);
                    if (_oldRecord != null)
                    {


                        _oldRecord.currency_id = record.currency_id;
                        _oldRecord.exchange_rate = record.exchange_rate;
                        _oldRecord.debit = record.debit;
                        _oldRecord.credit = record.credit;
                        _oldRecord.normalized_debit = record.normalized_debit;
                        _oldRecord.normalized_credit = record.normalized_credit;

                        _oldRecord.modified_by = Context.AppUserId;
                        _oldRecord.modified_on = DateTime.Now;
                        db.Update(_oldRecord);

                        appLogger.Debug(JsonConvert.SerializeObject(record));
                        appLogger.Debug(JsonConvert.SerializeObject(_oldRecord));

                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }

            return result;
        }

        public bool SyncronizeAccount(String legalEntityId)
        {
            var result = false;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append("SELECT [fn].[usf_transaction_coa_balance_by_legal_entity](@0)", legalEntityId);
                    var coaBalanceId = db.ExecuteScalar<string>(sql);
                    if (!string.IsNullOrEmpty(coaBalanceId))
                    {
                        sql = Sql.Builder.Append(account.DefaultView);
                        sql.Append(" WHERE r.legal_entity_id = @0 AND r.account_level = 5 AND r.id NOT IN( "
                                    + " SELECT d.account_id "
                                    + " FROM fn.financial_transaction_detail d "
                                    + " WHERE d.financial_transaction_id =@1"
                                    + " AND d.is_active = 1 "
                                    + " AND d.is_default = 1 "
                                    + " AND d.is_locked = 1 "
                                    + " ) ", legalEntityId, coaBalanceId);

                        var accountSync = db.Fetch<account>(sql);
                        if (accountSync != null)
                        {
                            appLogger.Debug($"Legal Entity : {legalEntityId} => Total no having coa balance {accountSync.Count}");
                        }
                        appLogger.Debug(db.LastCommand);

                    }

                    result = true;
                }
                catch (Exception ex)
                {
                    appLogger.Debug(db.LastCommand);
                    appLogger.Error(ex);
                }

            }

            return result;
        }


        public bool CreateCoaBalance(String legalEntityId, DateTime transactionDate)
        {
            var result = false;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append("SELECT [fn].[usf_transaction_coa_balance_by_legal_entity](@0)", legalEntityId);
                    var coaBalanceId = db.ExecuteScalar<string>(sql);
                    if (string.IsNullOrEmpty(coaBalanceId))
                    {

                        var record = new financial_transaction();
                        record.id = Guid.NewGuid().ToString();
                        record.created_by = Context.AppUserId;
                        record.created_on = DateTime.Now;
                        record.owner_id = Context.AppUserId;
                        record.organization_id = Context.OrganizationId;
                        record.legal_entity_id = legalEntityId;
                        record.transaction_datetime = transactionDate;
                        record.transaction_number = new FinancialTransaction(Context).GetNextAutoNumber();
                        record.self_reference = "TX-0000";
                        record.is_active = true;
                        record.is_locked = true;
                        record.is_default = true;

                        db.Insert(record);
                        result = true;
                        sql = Sql.Builder.Append(account.DefaultView);
                        sql.Append(" WHERE r.is_active=1 AND COALESCE(r.is_deleted,0)=0 AND r.organization_id=@0 ", Context.OrganizationId);
                        sql.Append(" AND r.legal_entity_id=@0 ", legalEntityId);
                        var accountDetails = db.Fetch<vw_account>(sql);
                        if (accountDetails == null)
                            throw new Exception($"Legal Entity {legalEntityId} doesn't have account !");

                        foreach (var a in accountDetails)
                        {
                            try
                            {
                                var recordDetail = new financial_transaction_detail();
                                recordDetail.id = Guid.NewGuid().ToString();
                                recordDetail.created_by = Context.AppUserId;
                                recordDetail.created_on = DateTime.Now;
                                recordDetail.owner_id = Context.AppUserId;

                                recordDetail.currency_id = "46e4d57e-5ce2-4c31-82be-f81382dc243a";
                                recordDetail.financial_transaction_id = record.id;
                                recordDetail.account_id = a.id;
                                recordDetail.debit = 0;
                                recordDetail.credit = 0;
                                recordDetail.exchange_rate = 1;
                                recordDetail.normalized_debit = 0;
                                recordDetail.normalized_credit = 0;

                                recordDetail.is_active = true;
                                recordDetail.is_locked = true;
                                recordDetail.is_default = true;
                                db.Insert(recordDetail);
                                result &= true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                appLogger.Error(ex);
                            }

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

                }
            }
            return result;
        }
        public override bool SaveEntity(accountDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    #region Validation

                    if ((RecordDto.account_level ?? 0) <= 1)
                    {
                        if (!string.IsNullOrEmpty(RecordDto.account_parent_id))
                        {
                            throw new Exception(string.Format("Account level {0} may not have parent account",
                                RecordDto.account_level));
                        }
                    }
                    else if (!string.IsNullOrEmpty(RecordDto.account_parent_id))
                    {
                        var sql = Sql.Builder.Append(account.DefaultView);
                        sql.Append("WHERE r.id = @0", RecordDto.account_parent_id);
                        var parentAccount = db.FirstOrDefault<vw_account>(sql);

                        if (parentAccount != null)
                        {
                            if ((RecordDto.account_level ?? 0) - (parentAccount.account_level ?? 0) != 1)
                            {
                                throw new Exception("Invalid parent account level");
                            }
                            else
                            {
                                switch (RecordDto.account_level ?? 0)
                                {
                                    case 2: // N.xx.xx.xx.xx
                                        if (parentAccount.account_number?.Substring(1, 2) !=
                                            RecordDto.account_number?.Substring(1, 2))
                                        {
                                            throw new Exception("Invalid account number");
                                        }
                                        break;

                                    case 3: // N.N.xx.xx.xx
                                        if (parentAccount.account_number?.Substring(1, 4) !=
                                            RecordDto.account_number?.Substring(1, 4))
                                        {
                                            throw new Exception("Invalid account number");
                                        }
                                        break;

                                    case 4: // N.N.NN.xx.xx
                                        if (parentAccount.account_number?.Substring(1, 7) !=
                                            RecordDto.account_number?.Substring(1, 7))
                                        {
                                            throw new Exception("Invalid account number");
                                        }
                                        break;

                                    case 5: // N.N.NN.NN.xx
                                        if (parentAccount.account_number?.Substring(1, 10) !=
                                            RecordDto.account_number?.Substring(1, 10))
                                        {
                                            throw new Exception("Invalid account number");
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid parent account");
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Account level {0} must have parent account",
                            RecordDto.account_level));
                    }

                    #endregion

                    RecordDto.organization_id = Context.OrganizationId;
                    result = Svc.SaveEntity(RecordDto, ref isNew);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public virtual vw_account_cost_sharingDTO DetailsCostSharingByAccount(String AccountId)
        {
            vw_account_cost_sharingDTO result = new vw_account_cost_sharingDTO();

            using (var db = new PhoenixERPRepo())
            {
                try
                {

                    var accountRecord = account.FirstOrDefault(" WHERE id=@0 ", AccountId);
                    if (accountRecord == null) throw new ArgumentException($"Account ID {AccountId} not found ");
                    if (accountRecord.cost_sharing_allocation_id == null) throw new ArgumentException($"Account ID {AccountId} does't have cost sharing allocation");

                    string qry = "";
                    var _costSharingAllocationService = new CostSharingAllocation(Context);
                    var costSharingAllocation = _costSharingAllocationService.Details(accountRecord.cost_sharing_allocation_id);

                    qry = cost_sharing_allocation_detail.DefaultView;
                    qry += " WHERE r.cost_sharing_allocation_id=@0 ";

                    var costSharingDetailAllocation = db.Fetch<vw_cost_sharing_allocation_detail>(qry, costSharingAllocation.id);
                    result.cost_sharing_allocation = costSharingAllocation;

                    result.cost_sharing_allocation_detail = costSharingDetailAllocation
                    .Select(x => new vw_cost_sharing_allocation_detailDTO().InjectFrom(x)).Cast<vw_cost_sharing_allocation_detailDTO>()
                    .ToList();


                    //if (view != null)
                    //{
                    //    result = (vw_account_cost_sharingDTO)((new vw_account_cost_sharingDTO()).InjectFrom(view));
                    //}
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public bool RemoveAllocationSharing(string AccountId)
        {
            bool result = false;
            try
            {
                var record = account.FirstOrDefault(" WHERE id=@0 ", AccountId);
                if (record != null && record.cost_sharing_allocation_id != null)
                {
                    var _costSharingAllocationService = new CostSharingAllocation(Context);
                    result = _costSharingAllocationService.SetNotActive(record.cost_sharing_allocation_id);
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public bool SaveCostSharingAllocation(String AccountId, account_cost_sharingDTO RecordDto, ref bool isNew)
        {
            bool result = false;
            if (RecordDto == null && AccountId == null) return result;

            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var Accountrecord = new accountDTO();
                    var oldRecordAccount = account.FirstOrDefault(" WHERE id=@0 ", AccountId);
                    Accountrecord.InjectFrom(oldRecordAccount);

                    if (Accountrecord == null) throw new ArgumentException($"Account : {AccountId} not found !");

                    var _costSharingAllocation = RecordDto.cost_sharing_allocation;
                    var _costSharingAllocationDetail = RecordDto.cost_sharing_allocation_detail;

                    if (_costSharingAllocation == null) return result;

                    var _costSharingAllocationService = new CostSharingAllocation(Context);
                    var _costSharingAllocationDetailService = new CostSharingAllocationDetailService(Context);
                    _costSharingAllocation.id = Guid.NewGuid().ToString();
                    _costSharingAllocation.organization_id = Context.OrganizationId;
                    _costSharingAllocation.cost_sharing_allocation_name = string.Format("CSA-{0}", CommonTool.Helper.RandomString.Generate(5));
                    _costSharingAllocation.is_active = true;
                    result = _costSharingAllocationService.Svc.SaveEntity(_costSharingAllocation, ref isNew);
                    // Check cost sharing has saved
                    if (result)
                    {
                        //Process save New or Re-New cost sharing detail Collection

                        foreach (var record in _costSharingAllocationDetail)
                        {
                            if (record == null) continue;
                            record.id = Guid.NewGuid().ToString();
                            record.cost_sharing_allocation_id = _costSharingAllocation.id;
                            appLogger.Debug(JsonConvert.SerializeObject(record));
                            result &= _costSharingAllocationDetailService.Svc.SaveEntity(record, ref isNew);
                        }
                    }


                    //Set Not Active Old record cost sharing and detail 
                    if (result && Accountrecord.cost_sharing_allocation_id != null)
                    {
                        appLogger.Debug("Masuk Detail cost_sharing_allocation_id " + Accountrecord.cost_sharing_allocation_id);
                        result &= _costSharingAllocationService.SetNotActive(Accountrecord.cost_sharing_allocation_id);
                        //var costSharing = cost_sharing_allocation.FirstOrDefault(" WHERE id=@0 ", Accountrecord.cost_sharing_allocation_id);
                        //costSharing.is_active = false;
                        //result &= _costSharingAllocationService.Context.SaveEntity(costSharing, false);
                        //if (result)
                        //{
                        //    var costSharingDetail = cost_sharing_allocation_detail.Fetch(" WHERE cost_sharing_allocation_id=@0 ",
                        //        Accountrecord.cost_sharing_allocation_id);
                        //    foreach (var record in costSharingDetail)
                        //    {
                        //        record.is_active = false;
                        //        result &= _costSharingAllocationDetailService.Context.SaveEntity(record, false);
                        //    }
                        //}
                    }

                    if (result)
                    {
                        Accountrecord.cost_sharing_allocation_id = _costSharingAllocation.id;
                        result = Svc.SaveEntity(Accountrecord, ref isNew);
                    }

                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
                finally
                {
                    if (result)
                    {
                        scope.Complete();
                    }
                }
            }

            return result;
        }

        public DgResponse GetDataGridEasyUI(DgRequest DTRequest, string OptionalQueryBuilder)
        {
            DgResponse result = null;

            if (!string.IsNullOrEmpty(OptionalQueryBuilder))
            {
                appLogger.Debug(OptionalQueryBuilder);
                OptionalQueryBuilder = String.Format(" r.legal_entity_id = '{0}' ", OptionalQueryBuilder);
            }

            try
            {
                result = Context.GetDataGridEasyUI<vw_account>(DTRequest, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic Lookup(string Id = null, int page = 1, int length = 100, String optionalFilters = null, String optionalParameters = null)
        {
            dynamic result = null;
            try
            {
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters
                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in account.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(account.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 AND r.organization_id =@0 ", Context.OrganizationId);

                            if (!string.IsNullOrEmpty(filters))
                            {
                                if (string.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(string.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(string.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.account_number ");
                            result = db.Page<vw_account>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(account.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" AND r.organization_id = @0 ", Context.OrganizationId);
                            sql.Append(" ORDER BY r.account_number ");
                            result = db.Page<vw_account>(1, rowPerPage, sql);

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

        public dynamic AccountLevelsLookup()
        {
            dynamic result = new ExpandoObject();

            try
            {
                result.currentPage = 1;
                result.totalPages = 1;
                result.totalItems = 5;
                result.itemsPerPage = 5;
                result.items = new List<dynamic>();
                for (int i = 1; i <= (int)result.totalItems; i++)
                {
                    dynamic item = new ExpandoObject();
                    item.id = i;
                    item.text = i.ToString();

                    ((List<dynamic>)result.items).Add(item);
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        private List<DataAccess.Models.Entity.Custom.Finance.Account> exportData(string LegalEntityId, out string ErrorMessage)
        {
            List<DataAccess.Models.Entity.Custom.Finance.Account> result =
                new List<DataAccess.Models.Entity.Custom.Finance.Account>();
            ErrorMessage = "";

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append("SELECT");
                    sql.Append("l.legal_entity_name AS LegalEntityName");
                    sql.Append(", a.account_number AS AccountNumber");
                    sql.Append(", a.account_name AS AccountName");
                    sql.Append(", a.account_level AS AccountLevel");
                    sql.Append(", a2.account_name AS ParentAccountNumber");
                    sql.Append(", g.account_group_name AS AccountGroupName");
                    sql.Append(", CASE");
                    sql.Append("WHEN COALESCE(a.is_normal_debit, 0) = 1 THEN 'Debit'");
                    sql.Append("ELSE 'Credit'");
                    sql.Append("END AS NormalCondition");
                    sql.Append(", CASE");
                    sql.Append("WHEN COALESCE(a.report_balance, 0) = 1 THEN 'BalanceSheet");
                    sql.Append("WHEN COALESCE(a.report_profit_loss, 0) = 1 THEN 'ProfitLoss");
                    sql.Append("END AS ReportUsage");
                    sql.Append("FROM fn.account a");
                    sql.Append("INNER JOIN legal_entity l ON l.id = a.legal_entity_id");
                    sql.Append("LEFT JOIN account a2 ON a2.id = a.account_parent_id");
                    sql.Append("LEFT JOIN account_group g ON g.id = a.account_group_id");
                    sql.Append("WHERE l.legal_entity_id = @0", LegalEntityId);

                    result = db.Fetch<DataAccess.Models.Entity.Custom.Finance.Account>(sql);
                }
                catch (Exception ex)
                {
                    appLogger.Debug(db.LastCommand);
                    appLogger.Error(ex.ToString());
                    ErrorMessage = ex.ToString();
                }
            }

            return result;
        }

        public bool ExportData(string LegalEntityId, out Dictionary<string, int> Header,
            out List<Dictionary<int, string>> Data, out string ErrorMessage)
        {
            bool result = false;
            ErrorMessage = "";
            Header = new Dictionary<string, int>();
            Data = new List<Dictionary<int, string>>();

            try
            {
                Header.Add("LegalEntityName", 0);
                Header.Add("AccountNumber", 1);
                Header.Add("AccountName", 2);
                Header.Add("AccountLevel", 3);
                Header.Add("ParentAccountNumber", 4);
                Header.Add("AccountGroupName", 5);
                Header.Add("NormalCondition", 6);
                Header.Add("ReportUsage", 7);

                var accounts = exportData(LegalEntityId, out ErrorMessage);
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    foreach (var acc in accounts)
                    {
                        var data = new Dictionary<int, string>();
                        data.Add(0, acc.LegalEntityName);
                        data.Add(1, acc.AccountNumber);
                        data.Add(2, acc.AccountName);
                        data.Add(3, acc.AccountLevel.ToString());
                        data.Add(4, acc.ParentAccountNumber);
                        data.Add(5, acc.AccountGroupName);
                        data.Add(6, acc.NormalCondition);
                        data.Add(7, acc.ReportUsage);

                        Data.Add(data);
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
                ErrorMessage = ex.ToString();
            }

            return result;
        }

        public bool ImportData(string LegalEntityId, Dictionary<string, int> Header,
            List<Dictionary<int, string>> Data, out string ErrorMessage)
        {
            bool result = false;
            ErrorMessage = "";

            try
            {
                var newAccounts = new List<DataAccess.Models.Entity.Custom.Finance.Account>();

                foreach (var data in Data)
                {
                    var newAccount = new DataAccess.Models.Entity.Custom.Finance.Account();
                    newAccount.LegalEntityName = data[Header["LegalEntityName"]];
                    newAccount.AccountNumber = data[Header["AccountNumber"]];
                    newAccount.AccountName = data[Header["AccountName"]];
                    newAccount.AccountLevel = int.Parse(data[Header["AccountLevel"]]);
                    newAccount.ParentAccountNumber = data[Header["ParentAccountNumber"]];
                    newAccount.AccountGroupName = data[Header["AccountGroupName"]];
                    newAccount.NormalCondition = data[Header["NormalCondition"]];
                    newAccount.ReportUsage = data[Header["ReportUsage"]];
                }

                result = importData(LegalEntityId, newAccounts);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex.ToString());
                //ErrorMessage = ex.Message;
                ErrorMessage = ex.ToString();
            }

            return result;
        }

        private bool importData(string LegalEntityId, List<DataAccess.Models.Entity.Custom.Finance.Account> Data)
        {
            bool result = false;

            using (var db = new PhoenixERPRepo())
            {
                using (var tx = db.GetTransaction())
                {
                    try
                    {
                        var sql = Sql.Builder.Append("SELECT * FROM legal_entity");
                        sql.Append("WHERE id = @0", LegalEntityId);
                        var legalEntity = db.FirstOrDefault<legal_entity>(sql);

                        if (legalEntity != null)
                        {
                            sql = Sql.Builder.Append("SELECT * FROM fn.account_group");
                            sql.Append("WHERE organization_id = @0", legalEntity.organization_id);
                            var accountGroups = db.Fetch<account_group>();

                            foreach (var data in Data.OrderBy(o => o.AccountLevel))
                            {
                                if (string.IsNullOrEmpty(data.LegalEntityName)) continue;
                                if (data.LegalEntityName.ToLower() != legalEntity.legal_entity_name.ToLower()) continue;

                                var newAccount = new account()
                                {
                                    id = Guid.NewGuid().ToString(),
                                    created_by = Context.AppUserId,
                                    created_on = DateTime.Now,
                                    is_active = true,
                                    owner_id = legalEntity.owner_id,
                                    legal_entity_id = legalEntity.id,
                                    account_number = data.AccountNumber,
                                    account_name = data.AccountName,
                                    account_level = data.AccountLevel
                                };

                                if (!string.IsNullOrEmpty(data.NormalCondition))
                                {
                                    switch (data.NormalCondition.ToLower())
                                    {
                                        case "debit":
                                            newAccount.is_normal_debit = true;
                                            break;

                                        case "credit":
                                            newAccount.is_normal_debit = false;
                                            break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(data.ReportUsage))
                                {
                                    switch (data.ReportUsage.ToLower())
                                    {
                                        case "balancesheet":
                                            newAccount.report_balance = true;
                                            break;

                                        case "profitloss":
                                            newAccount.report_profit_loss = true;
                                            break;
                                    }
                                }

                                if (!string.IsNullOrEmpty(data.AccountGroupName))
                                {
                                    foreach (var ag in accountGroups)
                                    {
                                        if (string.IsNullOrEmpty(ag.account_group_name)) continue;

                                        if (data.AccountGroupName.ToLower() == ag.account_group_name.ToLower())
                                        {
                                            newAccount.account_group_id = ag.id;
                                            break;
                                        }
                                    }
                                }

                                var oldAccount = db.FirstOrDefault<account>(
                                    "WHERE account_number = @0 AND legal_entity_id = @1 AND is_active = 1",
                                    newAccount.account_number, legalEntity.id);
                                if (oldAccount == null)
                                {
                                    newAccount.created_by = Context.AppUserId;
                                    newAccount.created_on = DateTime.Now;
                                    newAccount.owner_id = legalEntity.owner_id;

                                    db.Insert(newAccount);
                                }
                                else
                                {
                                    oldAccount.account_name = data.AccountName;
                                    oldAccount.account_level = data.AccountLevel;
                                    oldAccount.account_group_id = newAccount.account_group_id;
                                    oldAccount.is_normal_debit = newAccount.is_normal_debit;
                                    oldAccount.report_balance = newAccount.report_balance;
                                    oldAccount.report_profit_loss = newAccount.report_profit_loss;

                                    db.Update(oldAccount);
                                }
                            }

                            tx.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        appLogger.Debug(db.LastCommand);
                        appLogger.Error(ex.ToString());
                    }
                }
            }

            return result;
        }

        public DataResponse GetListCoaBalanceKendoUIGrid(DataRequest dtRequest, String legalEntityId)
        {
            DataResponse result = null;
            try
            {
                String OptionalFilters = string.Empty;
                String OptionalParameters = string.Empty;
                String OptionalQueryBuilder = string.Empty;

                //OptionalFilters = "  r.organization_id=@0 AND r.legal_entity_id=@1 AND r.account_level = 5 AND ftt.transaction_type='BEGINNING BALANCE' ";
                //OptionalParameters = Context.OrganizationId;
                OptionalQueryBuilder = String.Format(" r.organization_id='{0}' AND r.legal_entity_id='{1}' AND r.account_level = 5 AND ft.self_reference='TX-0000'  ",//--ftt.transaction_type='BEGINNING BALANCE'
                    Context.OrganizationId, legalEntityId);
                result = Context.GetListKendoUIGridNoBaseEntity<vw_coa_balance>(dtRequest, OptionalFilters, OptionalParameters, OptionalQueryBuilder);
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
