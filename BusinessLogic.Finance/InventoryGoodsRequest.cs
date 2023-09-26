using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.General;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using NPoco;
using System.Transactions;
using DataAccess.Dto.Custom.Finance;
using Omu.ValueInjecter;
using CommonTool.KendoUI.Grid;
using CodeMarvel.Infrastructure.ModelShared;
using Newtonsoft.Json;

namespace BusinessLogic.Services
{
    public partial class InventoryGoodsRequest : InventoryGoodsRequestService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InventoryGoodsRequest(DataContext AppUserData) : base(AppUserData) { }

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

        public bool SaveInventoryGoodsRe(InventoryGoodsReceiptDetails RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    //var _goods_receipt = RecordDto.good_receipt;
                    //_goods_receipt.organization_id = Context.OrganizationId;
                    //appLogger.Debug(JsonConvert.SerializeObject(_goods_receipt));
                    //if (!Context.userSession.IsAdminProcurement && string.IsNullOrEmpty(_goods_receipt.id))
                    //{
                    //    _goods_receipt.legal_entity_id = Context.userSession.LegalEntityId;
                    //    _goods_receipt.business_unit_id = Context.userSession.BusinessUnitId;
                    //}
                    //appLogger.Debug(JsonConvert.SerializeObject(_goods_receipt));
                    //if (string.IsNullOrEmpty(_goods_receipt.goods_receipt_number))
                    //{
                    //    _goods_receipt.goods_receipt_number = string.Format("INVGOODREQ-{0}",
                    //             CommonTool.Helper.RandomString.Generate(5).ToUpper());
                    //}
                    //result = Svc.SaveEntity(_goods_receipt, ref isNew);


                    if (result)
                    {
                        //var goodReceiptDetailService = new InventoryGoodsReceiptDetail(Context);
                        //foreach (var detail in RecordDto.good_receipt_details)
                        //{
                        //    detail.inventory_goods_receipt_id = _goods_receipt.id;

                        //    result &= goodReceiptDetailService.Svc.SaveEntity(detail, ref isNew);
                        //}
                    }

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
