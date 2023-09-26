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
    public partial class InventoryGoodsReceipt : InventoryGoodsReceiptService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InventoryGoodsReceipt(DataContext AppUserData) : base(AppUserData) { }

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

        public bool SaveInventoryGoodsReceipt(InventoryGoodsReceiptDetails RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    using (var db = new PhoenixERPRepo())
                    {
                        var _goods_receipt = RecordDto.good_receipt;
                        _goods_receipt.organization_id = Context.OrganizationId;
                        appLogger.Debug(JsonConvert.SerializeObject(_goods_receipt));
                        List<inventory_goods_receipt_detailDTO> ListGoodsReceiptDetail = new List<inventory_goods_receipt_detailDTO>();

                        if (!Context.userSession.IsAdminProcurement && string.IsNullOrEmpty(_goods_receipt.id))
                        {
                            _goods_receipt.legal_entity_id = Context.userSession.LegalEntityId;
                            _goods_receipt.business_unit_id = Context.userSession.BusinessUnitId;
                        }

                        appLogger.Debug(JsonConvert.SerializeObject(_goods_receipt));

                        if (string.IsNullOrEmpty(_goods_receipt.goods_receipt_number))
                        {
                            _goods_receipt.goods_receipt_number = string.Format("INVGOODREQ-{0}",
                                     CommonTool.Helper.RandomString.Generate(5).ToUpper());
                        }
                        result = Svc.SaveEntity(_goods_receipt, ref isNew);

                        if (result)
                        {
                            var goodReceiptDetailService = new InventoryGoodsReceiptDetail(Context);
                            var FixedAssetService = new FixedAsset(Context);
                            var itemService = new ItemService(Context);

                            foreach (var detail in RecordDto.good_receipt_details)
                            {
                                if (string.IsNullOrEmpty(detail.item_code) && detail.received_qty != null && detail.received_qty > 0)
                                {
                                    throw new Exception("Item code cannot empty.");
                                }

                                if (detail.received_qty != null && detail.received_qty > 0)
                                {
                                    var goodReceiptDetailDTO = new inventory_goods_receipt_detailDTO();

                                    goodReceiptDetailDTO.inventory_goods_receipt_id = _goods_receipt.id;
                                    goodReceiptDetailDTO.item_id = detail.item_id;
                                    goodReceiptDetailDTO.item_name = detail.item_name;
                                    goodReceiptDetailDTO.item_type_id = detail.item_type_id;
                                    goodReceiptDetailDTO.item_type_name = detail.item_type_name;
                                    goodReceiptDetailDTO.purchased_qty = detail.purchased_qty;
                                    goodReceiptDetailDTO.received_qty = detail.received_qty;
                                    goodReceiptDetailDTO.remark = detail.remark;
                                    goodReceiptDetailDTO.total_received_qty = detail.total_received_qty;
                                    goodReceiptDetailDTO.unit_name = detail.unit_name;
                                    goodReceiptDetailDTO.uom_id = detail.uom_id;
                                    goodReceiptDetailDTO.purchase_order_detail_id = detail.purchase_order_detail_id;
                                    goodReceiptDetailDTO.item_code = detail.item_code;
                                    goodReceiptDetailDTO.id = detail.id;

                                    string query = string.Format(" select fn.GetSumReceivedItemFromGoodsReceived('{0}') ", detail.purchase_order_detail_id);
                                    var sql = Sql.Builder.Append(query);
                                    var total = db.Fetch<int>(sql);
                                    int total_received = total[0];

                                    var po_detail_svc = new PurchaseOrderDetailService(Context);
                                    var PO_detail = purchase_order_detail.FirstOrDefault(" WHERE id = @0 AND COALESCE(is_deleted,0)=0  ", detail.purchase_order_detail_id);
                                    if ((total_received + goodReceiptDetailDTO.received_qty) == goodReceiptDetailDTO.purchased_qty && PO_detail != null)
                                    {
                                        PO_detail.is_delivered = true;
                                        result &= Context.SaveEntity<purchase_order_detail>(PO_detail, false);
                                    }

                                    var item = new itemDTO();
                                    var FixAsset = new fixed_assetDTO();
                                    if (result && detail.item_id == null)
                                    {
                                        // insert to table dbo.item
                                        item.id = Guid.NewGuid().ToString();
                                        item.item_name = detail.item_name;
                                        item.item_type_id = detail.item_type_id;
                                        item.item_code = detail.item_code;
                                        item.uom_id = detail.uom_id;
                                        item.organization_id = Context.OrganizationId;

                                        result &= itemService.Svc.SaveEntity(item, ref isNew);

                                        if (result && PO_detail != null)
                                        {
                                            PO_detail.item_id = item.id;
                                            result &= Context.SaveEntity<purchase_order_detail>(PO_detail, false);
                                        }

                                        goodReceiptDetailDTO.item_id = item.id;
                                        goodReceiptDetailDTO.item_name = item.item_name;
                                    }

                                    if (result && detail.item_id != null && detail.item_type_name.ToLower() == "fixed asset" &&
                                        !string.IsNullOrEmpty(detail.item_code))
                                    {
                                        FixAsset.item_id = detail.item_id;
                                        FixAsset.fixed_asset_name = detail.item_name;
                                        FixAsset.item_type_id = detail.item_type_id;
                                        FixAsset.purchase_order_id = _goods_receipt.purchase_order_id;
                                        FixAsset.registration_date = DateTime.Now;
                                        FixAsset.serial_number = detail.item_code;
                                        FixAsset.unit_count = 1;
                                        result &= FixedAssetService.Svc.SaveEntity(FixAsset, ref isNew);
                                    }
                                    else if (result && detail.item_id == null && detail.item_type_name.ToLower() == "fixed asset" &&
                                        !string.IsNullOrEmpty(detail.item_code))
                                    {
                                        FixAsset.item_id = item.id;
                                        FixAsset.fixed_asset_name = item.item_name;
                                        FixAsset.item_type_id = item.item_type_id;
                                        FixAsset.purchase_order_id = _goods_receipt.purchase_order_id;
                                        FixAsset.registration_date = DateTime.Now;
                                        FixAsset.serial_number = item.item_code;
                                        FixAsset.unit_count = 1;
                                        result &= FixedAssetService.Svc.SaveEntity(FixAsset, ref isNew);
                                    }

                                    if (result)
                                    {
                                        result &= goodReceiptDetailService.Svc.SaveEntity(goodReceiptDetailDTO, ref isNew);
                                        if (result)
                                            ListGoodsReceiptDetail.Add(goodReceiptDetailDTO);
                                    }
                                }
                            }
                        }

                        if (result)
                        {
                            var TrxDto = new inventory_transactionDTO();
                            var TrxSvc = new InventoryTransactionService(Context);

                            TrxDto.id = Guid.NewGuid().ToString();
                            TrxDto.transaction_datetime = DateTime.Now;
                            TrxDto.organization_id = Context.OrganizationId;
                            TrxDto.affiliation_id = _goods_receipt.affiliation_id;
                            TrxDto.company_name = _goods_receipt.vendor_name;

                            if (!Context.userSession.IsAdminProcurement)
                            {
                                TrxDto.legal_entity_id = Context.userSession.LegalEntityId;
                                TrxDto.business_unit_id = Context.userSession.BusinessUnitId;
                            }
                            appLogger.Debug(JsonConvert.SerializeObject(_goods_receipt));
                            if (string.IsNullOrEmpty(TrxDto.transaction_number))
                            {
                                TrxDto.transaction_number = string.Format("TRXNUM-{0}",
                                         CommonTool.Helper.RandomString.Generate(5).ToUpper());
                            }
                            result = TrxSvc.SaveEntity(TrxDto, ref isNew);

                            if (result)
                            {
                                var TrxDetailDto = new inventory_transaction_detailDTO();
                                var TrxDetailSvc = new InventoryTransactionDetailService(Context);

                                foreach (var detail in ListGoodsReceiptDetail)
                                {
                                    TrxDetailDto.inventory_transaction_id = TrxDto.id;
                                    TrxDetailDto.id = Guid.NewGuid().ToString();
                                    TrxDetailDto.affiliation_id = TrxDto.affiliation_id;
                                    TrxDetailDto.business_unit_id = TrxDto.business_unit_id;
                                    TrxDetailDto.company_name = TrxDto.company_name;
                                    TrxDetailDto.debit = detail.total_received_qty;
                                    TrxDetailDto.item_id = detail.item_id;
                                    if (!Context.userSession.IsAdminProcurement)
                                    {
                                        TrxDetailDto.legal_entity_id = Context.userSession.LegalEntityId;
                                        TrxDetailDto.business_unit_id = Context.userSession.BusinessUnitId;
                                    }
                                    TrxDetailDto.uom_id = detail.uom_id;
                                    result &= TrxDetailSvc.SaveEntity(TrxDetailDto, ref isNew);
                                }
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
