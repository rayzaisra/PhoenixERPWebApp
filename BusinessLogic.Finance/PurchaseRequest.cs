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
using BusinessLogic.Core;
using DataAccess.PhoenixERP;

namespace BusinessLogic.Services
{
    //public partial class PurchaseRequest : PurchaseRequestService
    //{
    //    private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

    //    public PurchaseRequest(DataContext AppUserData) : base(AppUserData) { }

    //    public DataResponse GetListKendoUIGrid(DataRequest DTRequest, String OptionalFilters = "", String OptionalParameters = "", String OptionalQueryBuilder = "")
    //    {
    //        DataResponse result = null;
    //        try
    //        {
    //            //var qry = String.Format(" r.organization_id ='{0}' ", Context.OrganizationId);
    //            // result = GetListKendoGrid(DTRequest, OptionalQueryBuilder: qry);
    //            result = GetListKendoGrid(DTRequest);
    //        }
    //        catch (Exception ex)
    //        {
    //            appLogger.Error(ex);
    //            throw;
    //        }

    //        return result;
    //    }

    //    public bool SaveAsDraft(PurchaseRequestDetails RecordDto, ref bool isNew)
    //    {
    //        bool result = false;

    //        if (RecordDto == null) return result;

    //        using (var scope = new PhoenixERPRepo().GetTransaction())
    //        {
    //            try
    //            {
    //                #region internal rfq service
    //                //var internalRfqService = new InternalRfq(Context);
    //                //var internalRfqDetailService = new InternalRfqDetailService(Context);
    //                //var internalrfqDto = new internal_rfqDTO();
    //                #endregion

    //                var _purchaseRequest = RecordDto.purchase_request;
    //                var purchaseDetailsService = new PurchaseRequestDetail(Context);
    //                if (string.IsNullOrEmpty(_purchaseRequest.id))
    //                {
    //                    //_purchaseRequest.organization_id = Context.OrganizationId;
    //                    _purchaseRequest.organization_id = "9EE4834225E334380667DDCF2F60F6DA";
    //                    if (!Context.userSession.IsAdminProcurement)
    //                    {
    //                        // _purchaseRequest.legal_entity_id = Context.userSession.LegalEntityId;
    //                        //_purchaseRequest.business_unit_id = Context.userSession.BusinessUnitId;
    //                    }
    //                    _purchaseRequest.request_number = string.Format("REQ-{0}",
    //                             CommonTool.Helper.RandomString.Generate(5).ToUpper());
    //                }

    //                result = Svc.SaveEntity(_purchaseRequest, ref isNew);

    //                if (result)
    //                {
    //                    foreach (var detail in RecordDto.purchase_request_details)
    //                    {
    //                        detail.purchase_request_id = _purchaseRequest.id;
    //                        detail.amount = (detail.unit_price * detail.exchange_rate) * detail.quantity;
    //                        result &= purchaseDetailsService.Svc.SaveEntity(detail, ref isNew);
    //                    }
    //                    result &= Context.SaveApproval<vw_purchase_request>(_purchaseRequest.id);
    //                }

    //                //DateTime? now = DateTime.Now;

    //                //internalrfqDto.id = Guid.NewGuid().ToString();
    //                //internalrfqDto.internal_rfq_number = Context.GenerateAutoNumber("[fn].[sq_internal_rfq]", "IRFQ");
    //                //internalrfqDto.internal_rfq_date = now;
    //                //internalrfqDto.legal_entity_id = RecordDto.purchase_request.legal_entity_id;
    //                //internalrfqDto.affiliation_id = RecordDto.purchase_request.affiliation_id;
    //                //internalrfqDto.business_unit_id = RecordDto.purchase_request.business_unit_id;

    //                //result &= internalRfqService.Svc.SaveEntity(internalrfqDto, ref isNew);

    //                //foreach (var detailId in RecordDto.purchase_request_details)
    //                //{
    //                //    var irfqDetailDto = new internal_rfq_detailDTO();
    //                //    irfqDetailDto.internal_rfq_id = internalrfqDto.id;
    //                //    irfqDetailDto.item_id = detailId.item_id;
    //                //    irfqDetailDto.item_name = detailId.item_name;
    //                //    irfqDetailDto.item_type = detailId.item_type_id;
    //                //    irfqDetailDto.uom_id = detailId.uom_id;
    //                //    irfqDetailDto.description = detailId.description;
    //                //    irfqDetailDto.qty = detailId.quantity;
    //                //    irfqDetailDto.unit_price = detailId.unit_price;
    //                //    irfqDetailDto.amount = (detailId.amount.HasValue) ? detailId.amount.Value : 0;
    //                //    result &= internalRfqDetailService.Svc.SaveEntity(irfqDetailDto, ref isNew);
    //                //    if (result)
    //                //    {
    //                //        internalrfqDto.currency_id = detailId.currency_id;
    //                //        internalRfqService.Svc.SaveEntity(internalrfqDto, ref isNew);
    //                //    }

    //                //}

    //                if (result)
    //                {
    //                    scope.Complete();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                appLogger.Error(ex);
    //                throw;
    //            }
    //        }
    //        return result;
    //    }

    //    public bool Submit(String recordId)
    //    {
    //        var result = false;
    //        using (var scope = new TransactionScope(TransactionScopeOption.Required))
    //        {
    //            try
    //            {
    //                var approvalResult = Context.Approval.SetApproval<purchase_request>(recordId);
    //                var oldRecord = purchase_request.FirstOrDefault("WHERE id=@0", recordId);
    //                oldRecord.approval_id = approvalResult.id;
    //                result = Context.SaveEntity<purchase_request>(oldRecord, false);
    //                if (result)
    //                    scope.Complete();
    //            }
    //            catch (Exception ex)
    //            {
    //                appLogger.Error(ex);
    //                throw;
    //            }
    //        }
    //        return result;
    //    }


    //    public ApiResponse createRfq(String purchaseRequestId, List<String> prDetailId, ref bool isNew)
    //    {
    //        ApiResponse result = new ApiResponse();
    //        using (var scope = new TransactionScope())
    //        {
    //            try
    //            {
    //                var record = purchase_request.FirstOrDefault(" WHERE id=@0 AND is_active=1 AND COALESCE(is_deleted,0)=0 ", purchaseRequestId);

    //                if (record == null)
    //                    throw new Exception("Record not found");

    //                var details = purchase_request_detail.Fetch(" WHERE purchase_request_id = @0 AND is_active=1 AND COALESCE(is_deleted,0)=0  ", record.id);
    //                appLogger.Debug("DETAILS : " + Newtonsoft.Json.JsonConvert.SerializeObject(details));
    //                if (details == null)
    //                    throw new Exception("Canceled create vendor quotation, Record dont have purchase request detail");

    //                #region rfqservice
    //                var rfqService = new RequestForQuotation(Context);
    //                var rfqDetailService = new RequestForQuotationDetailService(Context);
    //                var rfqDto = new request_for_quotationDTO();
    //                #endregion
                    
    //                #region purcase request Service
    //                var prDetailService = new PurchaseRequestDetail(Context);
    //                var prDetailRecordDto = new purchase_request_detailDTO();
    //                #endregion
                    
    //                rfqDto.id = Guid.NewGuid().ToString();
    //                rfqDto.purchase_request_id = record.id;
    //                rfqDto.request_for_quotation_number = Context.GenerateAutoNumber("[fn].[sq_rfq]", "RFQ");
    //                rfqDto.legal_entity_id = record.legal_entity_id;
    //                rfqDto.affiliation_id = record.affiliation_id;
    //                rfqDto.business_unit_id = record.business_unit_id;
    //                rfqDto.request_for_quotation_date = DateTime.Now;

    //                result.Status.Success = rfqService.Svc.SaveEntity(rfqDto, ref isNew);

    //                if (result.Status.Success)
    //                {
    //                    foreach (var detailId in prDetailId)
    //                    {
    //                        var recordDetail = purchase_request_detail.FirstOrDefault(" WHERE id=@0 AND is_active=1 AND COALESCE(is_deleted,0)=0 ", detailId);
    //                        prDetailRecordDto.InjectFrom(recordDetail);
    //                        var rfqDetailDto = new request_for_quotation_detailDTO();
    //                        rfqDetailDto.request_for_quotation_id = rfqDto.id;
    //                        rfqDetailDto.item_id = prDetailRecordDto.item_id;
    //                        rfqDetailDto.item_name = prDetailRecordDto.item_name;
    //                        rfqDetailDto.item_type = prDetailRecordDto.item_type_id;
    //                        rfqDetailDto.qty = prDetailRecordDto.quantity;
    //                        rfqDetailDto.currency_id = prDetailRecordDto.currency_id;
    //                        rfqDetailDto.exchange_rate = prDetailRecordDto.exchange_rate;
    //                        rfqDetailDto.unit_price = prDetailRecordDto.unit_price;
    //                        rfqDetailDto.amount = prDetailRecordDto.amount;
    //                        rfqDetailDto.description = prDetailRecordDto.description;
    //                        rfqDetailDto.uom_id = prDetailRecordDto.uom_id;
    //                        result.Status.Success &= rfqDetailService.Svc.SaveEntity(rfqDetailDto, ref isNew);
    //                        if (result.Status.Success)
    //                        {
    //                            prDetailRecordDto.rfq_id = rfqDto.id;
    //                            prDetailService.Svc.SaveEntity(prDetailRecordDto, ref isNew);
    //                        }
    //                    }

    //                    result.Data = new
    //                    {
    //                        recordID = rfqDto.id,
    //                        PurchaseRequestId = purchaseRequestId
    //                    };
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                result.Status.Success = false;
    //                appLogger.Error(ex);
    //                throw;
    //            }
    //            finally
    //            {
    //                if (result.Status.Success)
    //                {
    //                    scope.Complete();
    //                    result.Status.Message = "Success created RFQ";
    //                }
    //            }
    //        }
    //        return result;
    //    }



    //}
}
