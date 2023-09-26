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
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Linq;
using DataAccess.PhoenixERP.General;
using DataAccess.Dto.Custom.Finance;
using CommonTool.JEasyUI.DataGrid;

namespace BusinessLogic.Finance
{
    public partial class BankPayment : BankPaymentService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public BankPayment(DataContext AppUserData) : base(AppUserData)
        {

        }

        public bool SaveTransaction(BankPaymentDetailsDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var BankPaymentRecord = RecordDto.bank_payment;
                    var TransactionDetailsRecord = RecordDto.details;

                    if (BankPaymentRecord == null)
                        throw new Exception("Bank Payment request is required");
                    if (TransactionDetailsRecord == null)
                        throw new Exception("Transaction detail request is required");

                    var paymentNumber = string.IsNullOrEmpty(BankPaymentRecord.payment_number) ?
                        GetNextAutoNumber() : BankPaymentRecord.payment_number;
                    if (BankPaymentRecord.approval_id == "")
                    {
                        BankPaymentRecord.approval_id = null;
                    }
                    #region Save Financial Transaction

                    // Create FinancialTransactionDetails AS RecordDTO
                    var financialDTO = new FinancialTransactionDetails();

                    #region Header

                    var FinancialTransactionRecord = new financial_transactionDTO();
                    var _FinancialTransactionService = new FinancialTransaction(Context);

                    #region Get Company Name

                    if (BankPaymentRecord.paid_to_vendor_id != null)
                    {
                        var _company = company.FirstOrDefault(" WHERE id=@0", BankPaymentRecord.paid_to_vendor_id);
                        if(_company != null)
                        {
                            FinancialTransactionRecord.company_id = BankPaymentRecord.paid_to_vendor_id;
                            FinancialTransactionRecord.company_name = _company.company_name;
                        }
                    }

                    #endregion

                    #region Get Transaction Type Id

                    var ftt = new FinancialTransactionType(Context);
                    FinancialTransactionRecord.transaction_type_id = 
                        ftt.GetTransactionTypeId(financial_transaction_type.MUTATION);

                    #endregion

                    var isExist = bank_payment.FirstOrDefault(" WHERE id = @0 ",BankPaymentRecord.id);
                    if (isExist != null)
                    {
                        var ft_record = financial_transaction.FirstOrDefault(" WHERE id = @0 ",isExist.financial_transaction_id);
                        FinancialTransactionRecord.id = ft_record.id;
                    }
                    FinancialTransactionRecord.transaction_datetime = BankPaymentRecord.requested_on ?? DateTime.Now;
                    //FinancialTransactionRecord.transaction_type_id = "9256a1e8-ae87-4d23-8baf-40714c1d35b5"; /*Mutation*/
                    FinancialTransactionRecord.legal_entity_id = BankPaymentRecord.legal_entity_id;
                    FinancialTransactionRecord.affiliation_id = BankPaymentRecord.affiliation_id;   
                    FinancialTransactionRecord.business_unit_id = BankPaymentRecord.business_unit_id;
                    FinancialTransactionRecord.reference = paymentNumber;
                    financialDTO.transaction = FinancialTransactionRecord;

                    #endregion

                    #region Detail

                    financialDTO.details = TransactionDetailsRecord;

                    #endregion

                    result = _FinancialTransactionService.SaveToDraft(financialDTO, ref isNew);

                    if (result)
                    {

                        //result &= Svc.SaveEntity(BankPaymentRecord, ref isNew);
                        BankPaymentRecord.payment_number = paymentNumber;
                        BankPaymentRecord.financial_transaction_id = financialDTO.transaction.id;
                        result = Svc.SaveEntity(BankPaymentRecord, ref isNew);

                        if (result)
                        {
                            scope.Complete();
                        }
                        else
                        {
                            throw new Exception("Transaction Aborted");
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
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
                        foreach (var col in bank_payment.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(bank_payment.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");

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

                            sql.Append(" ORDER BY r.bank_payment ");
                            result = db.Page<vw_bank_payment>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(bank_payment.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.payment_number ");
                            result = db.Page<vw_bank_payment>(1, rowPerPage, sql);
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

        public DgResponse GetDataGridEasyUI(DgRequest DTRequest, String BankPaymentId)
        {
            DgResponse result = null;
            try
            {
                var bankPaymentRecord = bank_payment.FirstOrDefault(" WHERE id=@0", BankPaymentId);
                if (bankPaymentRecord == null)
                    throw new Exception("Bank Payment not found");

                String FinancialTransactionId = bankPaymentRecord.financial_transaction_id;
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

        public string GetNextAutoNumber()
        {
            return Context.GenerateAutoNumber("[fn].[sq_bank_payment]", "PR");
        }
    }
}
