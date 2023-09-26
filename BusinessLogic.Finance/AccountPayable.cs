using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.General;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP.General;
using NPoco;
using System.Transactions;
using DataAccess.Dto.Custom.Finance;
using DataAccess.Dto.Custom.General;
using Omu.ValueInjecter;
using CommonTool.KendoUI.Grid;
using CodeMarvel.Infrastructure.ModelShared;
using Newtonsoft.Json;
using DataAccess.PhoenixERP;

namespace BusinessLogic.Services
{
    public partial class AccountPayable : AccountPayableService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public AccountPayable(DataContext AppUserData) : base(AppUserData) { }

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String OptionalFilters = "",
            String OptionalParameters = "", String OptionalQueryBuilder = "")
        {
            DataResponse result = null;
            try
            {
                var qry = String.Format(" r.organization_id ='{0}' ", Context.OrganizationId);
                result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public bool SaveAccountPayable(AccountPayableDetails RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    using (var db = new PhoenixERPRepo())
                    {
                        var _account_payable = RecordDto.account_payable;
                        appLogger.Debug(JsonConvert.SerializeObject(_account_payable));

                        if (string.IsNullOrEmpty(_account_payable.account_payable_number))
                        {
                            _account_payable.account_payable_number = string.Format("ACPAY-{0}",
                                     CommonTool.Helper.RandomString.Generate(5).ToUpper());
                        }
                            result = Svc.SaveEntity(_account_payable, ref isNew);

                        if (result && RecordDto.account_payable_details != null)
                        {
                            var accountpayableDetailService = new AccountPayableDetail(Context);
                            var FixedAssetService = new FixedAsset(Context);
                            var itemService = new ItemService(Context);

                            foreach (var detail in RecordDto.account_payable_details)
                            {
                                var accountPayableDTO = new account_payable_detailDTO();

                                accountPayableDTO.account_payable_id = _account_payable.id;
                                accountPayableDTO.amount_paid = detail.amount_paid;
                                accountPayableDTO.discount = detail.discount;
                                accountPayableDTO.ending_balance = detail.ending_balance;
                                accountPayableDTO.evident_number = detail.evident_number;
                                accountPayableDTO.id = Guid.NewGuid().ToString();
                                accountPayableDTO.payable_balance = detail.payable_balance;
                                accountPayableDTO.row_index = detail.row_index;
                                accountPayableDTO.tax = detail.tax;

                                result &= accountpayableDetailService.Svc.SaveEntity(accountPayableDTO, ref isNew);

                            }
                        }

                        if (result)
                        {
                            scope.Complete();
                        }
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

        public ApiResponse createRfq(String purchaseRequestId, List<String> prDetailId, ref bool isNew)
        {
            ApiResponse result = new ApiResponse();
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = purchase_request.FirstOrDefault(" WHERE id=@0 AND is_active=1 AND COALESCE(is_deleted,0)=0 ", purchaseRequestId);

                    if (record == null)
                        throw new Exception("Record not found");

                    var details = purchase_request_detail.Fetch(" WHERE purchase_request_id = @0 AND is_active=1 AND COALESCE(is_deleted,0)=0  ", record.id);
                    appLogger.Debug("DETAILS : " + Newtonsoft.Json.JsonConvert.SerializeObject(details));
                    if (details == null)
                        throw new Exception("Canceled create vendor quotation, Record dont have purchase request detail");

                    #region rfqservice
                    var rfqService = new RequestForQuotation(Context);
                    var rfqDetailService = new RequestForQuotationDetailService(Context);
                    var rfqDto = new request_for_quotationDTO();
                    #endregion
                    #region prService
                    var prDetailService = new PurchaseRequestDetail(Context);
                    var prDetailRecordDto = new purchase_request_detailDTO();
                    #endregion
                    rfqDto.id = Guid.NewGuid().ToString();
                    rfqDto.purchase_request_id = record.id;
                    rfqDto.request_for_quotation_number = Context.GenerateAutoNumber("[fn].[sq_rfq]", "RFQ");
                    rfqDto.legal_entity_id = record.legal_entity_id;
                    rfqDto.affiliation_id = record.affiliation_id;
                    rfqDto.business_unit_id = record.business_unit_id;
                    rfqDto.request_for_quotation_date = DateTime.Now;

                    result.Status.Success = rfqService.Svc.SaveEntity(rfqDto, ref isNew);
                    if (result.Status.Success)
                    {
                        foreach (var detailId in prDetailId)
                        {
                            var recordDetail = purchase_request_detail.FirstOrDefault(" WHERE id=@0 AND is_active=1 AND COALESCE(is_deleted,0)=0 ", detailId);
                            prDetailRecordDto.InjectFrom(recordDetail);
                            var rfqDetailDto = new request_for_quotation_detailDTO();
                            rfqDetailDto.request_for_quotation_id = rfqDto.id;
                            rfqDetailDto.item_id = prDetailRecordDto.item_id;
                            rfqDetailDto.item_name = prDetailRecordDto.item_name;
                            rfqDetailDto.item_type = prDetailRecordDto.item_type_id;
                            rfqDetailDto.qty = prDetailRecordDto.quantity;
                            rfqDetailDto.unit_price = prDetailRecordDto.unit_price;
                            rfqDetailDto.amount = prDetailRecordDto.amount;
                            rfqDetailDto.description = prDetailRecordDto.description;
                            rfqDetailDto.uom_id = prDetailRecordDto.uom_id;
                            result.Status.Success &= rfqDetailService.Svc.SaveEntity(rfqDetailDto, ref isNew);
                            if (result.Status.Success)
                            {
                                prDetailRecordDto.rfq_id = rfqDto.id;
                                prDetailService.Svc.SaveEntity(prDetailRecordDto, ref isNew);
                            }
                        }
                        result.Data = new
                        {
                            recordID = rfqDto.id,
                            PurchaseRequestId = purchaseRequestId
                        };
                    }
                }
                catch (Exception ex)
                {
                    result.Status.Success = false;
                    appLogger.Error(ex);
                    throw;
                }
                finally
                {
                    if (result.Status.Success)
                    {
                        scope.Complete();
                        result.Status.Message = "Success created RFQ";
                    }
                }
            }
            return result;
        }

    }
}
