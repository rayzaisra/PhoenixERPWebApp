using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;
using System.Transactions;
using Omu.ValueInjecter;
using CommonTool.KendoUI.Grid;
using CodeMarvel.Infrastructure.ModelShared;

namespace BusinessLogic.Services
{
    public partial class VendorQuotation : VendorQuotationService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public VendorQuotation(DataContext AppUserData) : base(AppUserData)
        {

        }

        public bool SaveVendorQuotation(vendor_quotation_detailsDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {

                var _vquotation = RecordDto.VendorQuotation;
                if (string.IsNullOrEmpty(_vquotation.vendor_quotation_number))
                {
                    _vquotation.vendor_quotation_number = Context.GenerateAutoNumber("[fn].[sq_vendor_quotation]", "VQ");
                }
                //_vquotation.amount_final = _vquotation.amount - _vquotation.discount;
                _vquotation.amount = _vquotation.amount_final;
                result = Svc.SaveEntity(_vquotation, ref isNew);
                if (result)
                {
                    var rfqDetailsService = new VendorQuotationDetailService(Context);
                    foreach (var detail in RecordDto.Details)
                    {
                        detail.vendor_quotation_id = _vquotation.id;

                        result &= rfqDetailsService.Svc.SaveEntity(detail, ref isNew);
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public bool SubmitVq(String RecordId, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = vendor_quotation.FirstOrDefault(" WHERE id=@0 ", RecordId);
                    if (record == null)
                        throw new Exception("Record not found");

                    var detailService = new VendorQuotationDetailService(Context);
                    var details = vendor_quotation_detail.Fetch(" WHERE vendor_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                    if (details == null)
                        throw new Exception("Canceled submiting data, Record dont have request for quotation detail");

                    record.is_locked = true;
                    var recordDto = new vendor_quotationDTO();
                    recordDto.InjectFrom(record);
                    result = Svc.SaveEntity(recordDto, ref isNew);
                    if (result)
                    {
                        var detailsDto = new List<vendor_quotation_detailDTO>();
                        detailsDto.InjectFrom(details);
                        foreach (var detail in detailsDto)
                        {
                            detail.is_locked = true;
                            result &= detailService.Svc.SaveEntity(detail, ref isNew);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
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
                        foreach (var col in vendor_quotation.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(vendor_quotation.DefaultView);
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

                            sql.Append(" ORDER BY r.vendor_quotation_number ");
                            result = db.Page<vw_vendor_quotation>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(vendor_quotation.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.vendor_quotation_number ");
                            result = db.Page<vw_vendor_quotation>(1, rowPerPage, sql);

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

        public string GetNextAutoNumber()
        {
            return Context.GenerateAutoNumber("[fn].[sq_vendor_quotation]", "VQN");
        }

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest)
        {
            DataResponse result = null;
            try
            {
                result = GetListKendoGrid(DTRequest);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        //public DataResponse GetListKendoWithDetailUIGrid(DataRequest DTRequest)
        //{
        //    DataResponse result = null;
        //    try
        //    {

        //        result = GetListWithDetailKendoGrid(DTRequest);
        //    }
        //    catch (Exception ex)
        //    {
        //        appLogger.Error(ex);
        //        throw;
        //    }

        //    return result;
        //}

        public List<vw_vendor_quotation_detailsDto> GetListVendorQuotation(String rfqId)
        {
            List<vw_vendor_quotation_detailsDto> result = new List<vw_vendor_quotation_detailsDto>();
            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var sql = Sql.Builder.Append(vendor_quotation.DefaultView);
                        sql.Append(" WHERE r.rfq_id = @0 AND COALESCE(r.is_deleted,0)=0", rfqId);

                        var view = db.Fetch<vw_vendor_quotation>(sql);

                        if (view != null)
                        {

                            foreach (var viewHeader in view)
                            {
                                sql = Sql.Builder.Append(vendor_quotation_detail.DefaultView);
                                vw_vendor_quotation_detailsDto vw_vendor = new vw_vendor_quotation_detailsDto();
                                sql.Append(" WHERE r.vendor_quotation_id= @0 AND COALESCE(r.is_deleted,0)=0", viewHeader.id);
                                var viewDetails = db.Fetch<vw_vendor_quotation_detail>(sql);
                                vw_vendor.VendorQuotation = (vw_vendor_quotationDTO)((new vw_vendor_quotationDTO()).InjectFrom(viewHeader));
                                vw_vendor.Details = viewDetails.Select(x => new vw_vendor_quotation_detailDTO().InjectFrom(x)).Cast<vw_vendor_quotation_detailDTO>().ToList();
                                result.Add(vw_vendor);
                            }
                        }
                        else
                        {
                            result = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        appLogger.Debug(db.LastCommand);
                        appLogger.Error(ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public bool createPoFromBidding(String biddingId, List<string> ListVQDetail_id, ref bool isNew)
        {
            bool result = false;
            List<string> listPO_id = new List<string>();

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var biddingRecord = purchase_bidding.FirstOrDefault(" WHERE id = @0 AND COALESCE ( is_deleted, 0 ) =0 ", biddingId);
                            if (biddingRecord == null) throw new Exception("Record Not Found");

                            var sql = Sql.Builder.Append(vendor_quotation_detail.DefaultView);
                            sql.Append(" WHERE r.id in (@0) AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0  ", ListVQDetail_id);
                            var vendorQuotationDetail = db.Fetch<vw_vendor_quotation_detail>(sql);
                            if (vendorQuotationDetail == null) throw new Exception("Vendor quotation detail not found");
                            List<string> ListVQ_id = new List<string>();

                            foreach (var detail in vendorQuotationDetail)
                            {
                                if (!ListVQ_id.Any(x => x == detail.vendor_quotation_id))
                                {
                                    ListVQ_id.Add(detail.vendor_quotation_id);
                                }
                            }

                            sql = Sql.Builder.Append(vendor_quotation.DefaultView);
                            sql.Append(" WHERE r.id in (@0) AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0  ", ListVQ_id);
                            var vendorQuotation = db.Fetch<vw_vendor_quotation>(sql);
                            if (vendorQuotation == null) throw new Exception("Vendor quotation not found");

                            #region PO Service
                            var PoService = new PurchaseOrderService(Context);
                            var PoDetailService = new PurchaseOrderDetailService(Context);

                            #endregion

                            sql = Sql.Builder.Append(request_for_quotation.DefaultView);
                            sql.Append(" WHERE r.id=@0 ", biddingRecord.rfq_id);
                            var RFQ = db.FirstOrDefault<vw_request_for_quotation>(sql);
                            if (RFQ == null) throw new Exception($"RFQ id {biddingRecord.rfq_id} in purchase bidding id {biddingRecord.id} not found ");


                            foreach (var vendor in vendorQuotation)
                            {
                                try
                                {
                                    sql = Sql.Builder.Append(purchase_bidding_detail.DefaultView);
                                    //sql.Append(" WHERE r.purchase_bidding_id=@0 AND vq.vendor_id=@1 AND is_approved=1  AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0 ",
                                    //    biddingRecord.id, vendor.vendor_id);

                                    sql.Append(" WHERE r.purchase_bidding_id=@0 AND vq.vendor_id=@1 AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0 ", biddingRecord.id, vendor.vendor_id);
                                    var BiddingDetailsApproved = db.Fetch<vw_purchase_bidding_detail>(sql);
                                    if (BiddingDetailsApproved == null || BiddingDetailsApproved.Count == 0) continue;

                                    var PoDto = new purchase_orderDTO();
                                    decimal? TotalAmountNet = 0;
                                    PoDto.purchase_request_id = RFQ.purchase_request_id;
                                    PoDto.purchase_request_number = RFQ.request_number;
                                    PoDto.purchase_order_number = string.Format("PO-{0}",
                                             CommonTool.Helper.RandomString.Generate(5).ToUpper());
                                    PoDto.legal_entity_id = RFQ.legal_entity_id;
                                    PoDto.organization_id = Context.OrganizationId;
                                    PoDto.affiliation_id = RFQ.affiliation_id;
                                    PoDto.vendor_quotation_id = vendor.id;
                                    PoDto.company_id = vendor.vendor_id;
                                    PoDto.business_unit_id = RFQ.business_unit_id;
                                    PoDto.currency_id = vendor.currency_id;
                                    PoDto.delivery_date = vendor.delivery_date;
                                    PoDto.discount_amount = vendor.discount;
                                    PoDto.exchange_rate = vendor.exchange_rate;
                                    PoDto.tax_amount = vendor.tax_base;
                                    PoDto.job_id = RFQ.job_id;

                                    result = PoService.Svc.SaveEntity(PoDto, ref isNew);

                                    if (result)
                                    {
                                        if (string.IsNullOrEmpty(vendor.purchase_order_id))
                                        {
                                            vendor.purchase_order_id = PoDto.id;
                                            var record = new vendor_quotation();
                                            record.InjectFrom(vendor);
                                            record.vendor_quotation_number = vendor.quotation_number;
                                            result &= Context.SaveEntity<vendor_quotation>(record, false);
                                        }
                                        else
                                        {
                                            var v = vendorQuotation.Where(x => !string.IsNullOrEmpty(x.purchase_order_id)).Select(x => x.vendor_name).Aggregate((a, b) => a + ", " + b);
                                            throw new Exception($"Purchase Order for vendor {v} has created");
                                        }

                                        foreach (var item in BiddingDetailsApproved)
                                        {
                                            try
                                            {
                                                var poDetailDto = new purchase_order_detailDTO();
                                                poDetailDto.item_id = item.item_id;
                                                poDetailDto.item_description = item.task_detail;
                                                poDetailDto.item_type = item.item_type_id;
                                                poDetailDto.item_name = item.item_name;
                                                poDetailDto.purchase_order_id = PoDto.id;
                                                poDetailDto.qty = item.qty ?? 0;
                                                poDetailDto.discount_amount = item.discount_amount ?? 0;
                                                poDetailDto.tax_percent = item.tax_base;
                                                poDetailDto.unit_price = item.unit_price ?? 0;
                                                poDetailDto.amount = item.amount ?? 0;
                                                poDetailDto.amount_net = (poDetailDto.amount + (poDetailDto.amount * poDetailDto.tax_percent)) ?? 0;

                                                TotalAmountNet += poDetailDto.amount_net ?? 0;
                                                poDetailDto.uom_id = item.uom_id;
                                                result &= PoDetailService.Svc.SaveEntity(poDetailDto, ref isNew);
                                            }
                                            catch (Exception)
                                            {
                                                throw;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }

                            }

                            if (result)
                            {
                                scope.Complete();
                            }

                            #region Old Code
                            //var sql = Sql.Builder.Append(purchase_bidding_detail.DefaultView);
                            //sql.Append(" WHERE r.purchase_bidding_id =@0 AND COALESCE ( r.is_approved, 0 ) =1 ", biddingId);
                            //var view = db.Fetch<vw_purchase_bidding_detail>(sql);

                            //foreach (var vq in view)
                            //{
                            //    sql = Sql.Builder.Append(request_for_quotation.DefaultView);
                            //    sql.Append(" WHERE r.id=@0 ", vq.rfq_id);
                            //    var viewHeader = db.Fetch<vw_request_for_quotation>(sql);

                            //    #region poservice
                            //    var PoService = new PurchaseOrderService(Context);
                            //    var PoDetailService = new PurchaseOrderDetailService(Context);
                            //    var PoDto = new purchase_orderDTO();
                            //    #endregion


                            //    sql = Sql.Builder.Append(vendor_quotation_detail.DefaultView);
                            //    sql.Append(" WHERE r.rfq_detail_id=@0 AND r.is_active=1 AND COALESCE(r.is_deleted,0)=0 ", vq.rfq_detail_id);

                            //    var recordVendorDetail = db.FirstOrDefault<vw_vendor_quotation_detail>(sql);

                            //    var VendorQuotationService = new VendorQuotationService(Context);
                            //    var VendorQuotationDTO = new vendor_quotationDTO();
                            //    foreach (var item in viewHeader)
                            //    {
                            //        PoDto.id = Guid.NewGuid().ToString();
                            //        PoDto.purchase_request_id = item.purchase_request_id;
                            //        PoDto.purchase_request_number = item.request_number;
                            //        PoDto.purchase_order_number = string.Format("PO-{0}",
                            //                 CommonTool.Helper.RandomString.Generate(5).ToUpper());
                            //        PoDto.legal_entity_id = item.legal_entity_id;
                            //        PoDto.organization_id = Context.OrganizationId;
                            //        PoDto.affiliation_id = item.affiliation_id;
                            //        PoDto.vendor_quotation_id = recordVendorDetail.vendor_quotation_id;
                            //        PoDto.company_id = recordVendorDetail.vendor_id;
                            //        PoDto.business_unit_id = item.business_unit_id;

                            //        result = PoService.Svc.SaveEntity(PoDto, ref isNew);
                            //        if (result)
                            //        {
                            //            VendorQuotationDTO.id = recordVendorDetail.vendor_quotation_id;
                            //            VendorQuotationDTO.purchase_order_id = PoDto.id;
                            //            result = VendorQuotationService.Svc.SaveEntity(VendorQuotationDTO, ref isNew);
                            //            if (result)
                            //            {
                            //                foreach (var detail in view)
                            //                {
                            //                    var details = vendor_quotation_detail.FirstOrDefault(" WHERE id = @0 AND COALESCE(is_deleted,0)=0 ", detail.vendor_quotation_detail_id);
                            //                    var poDetailDto = new purchase_order_detailDTO();
                            //                    poDetailDto.item_id = details.item_id;
                            //                    poDetailDto.item_description = details.task_detail;
                            //                    poDetailDto.item_type = details.item_type_id;
                            //                    poDetailDto.item_name = details.item_name;
                            //                    poDetailDto.purchase_order_id = PoDto.id;
                            //                    poDetailDto.qty = detail.qty ?? 0;
                            //                    poDetailDto.unit_price = detail.unit_price ?? 0;
                            //                    poDetailDto.amount = details.amount ?? 0;
                            //                    poDetailDto.uom_id = details.uom_id;
                            //                    result &= PoDetailService.Svc.SaveEntity(poDetailDto, ref isNew);
                            //                }
                            //            }
                            //        }
                            //        listPO_id.Add(PoDto.id);
                            //    }
                            //    poId = listPO_id;
                            //}
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            appLogger.Debug(db.LastCommand);
                            appLogger.Error(ex);
                            throw;
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

        public ApiResponse createPO(String vendorquotationid, ref bool isNew)
        {
            ApiResponse result = new ApiResponse();
            using (var scope = new TransactionScope())
            {
                try
                {
                    var rfqService = new RequestForQuotationService(Context);
                    var recordVendor = vendor_quotation.FirstOrDefault(" WHERE id=@0 ", vendorquotationid);
                    var record = rfqService.Svc.Details(recordVendor.rfq_id);

                    if (record == null)
                        throw new Exception("Record not found");
                    if (record.purchase_request_id == null)
                        throw new Exception("Canceled create Purchase Order, Record dont have purchase request");

                    var details = vendor_quotation_detail.Fetch(" WHERE vendor_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", recordVendor.id);

                    if (details.Count == 0)
                        throw new Exception("Canceled create Purchase Order, Record dont have vendor quotation detail");

                    //var vendorRecord = vendor_quotation.FirstOrDefault(" WHERE rfq_id =@0 ", rfqId);
                    //if (vendorRecord != null)
                    //    throw new Exception("Record Already created vendor quotation");

                    #region poservice
                    var PoService = new PurchaseOrderService(Context);
                    var PoDetailService = new PurchaseOrderDetailService(Context);
                    var PoDto = new purchase_orderDTO();
                    decimal? TotalAmountNet = 0;
                    #endregion

                    PoDto.id = Guid.NewGuid().ToString();
                    PoDto.purchase_request_id = record.purchase_request_id;
                    PoDto.purchase_request_number = record.request_number;
                    PoDto.purchase_order_number = string.Format("PO-{0}",
                             CommonTool.Helper.RandomString.Generate(5).ToUpper());
                    PoDto.legal_entity_id = record.legal_entity_id;
                    PoDto.organization_id = "9EE4834225E334380667DDCF2F60F6DA";
                    PoDto.affiliation_id = record.affiliation_id;
                    PoDto.vendor_quotation_id = recordVendor.id;
                    PoDto.company_id = recordVendor.vendor_id;
                    PoDto.business_unit_id = record.business_unit_id;

                    result.Status.Success = PoService.Svc.SaveEntity(PoDto, ref isNew);
                    recordVendor.purchase_order_id = PoDto.id;
                    if (result.Status.Success)
                    {
                        foreach (var detail in details)
                        {
                            var poDetailDto = new purchase_order_detailDTO();
                            poDetailDto.item_id = detail.item_id;
                            poDetailDto.item_description = detail.task_detail;
                            poDetailDto.item_type = detail.item_type_id;
                            poDetailDto.item_name = detail.item_name;
                            poDetailDto.purchase_order_id = PoDto.id;
                            poDetailDto.qty = detail.qty ?? 0;
                            poDetailDto.discount_amount = detail.discount_amount ?? 0;
                            poDetailDto.tax_percent = detail.tax_base;
                            poDetailDto.unit_price = detail.unit_price ?? 0;
                            poDetailDto.amount = detail.amount ?? 0;
                            poDetailDto.amount_net = (poDetailDto.amount + (poDetailDto.amount * poDetailDto.tax_percent)) ?? 0;

                            TotalAmountNet += poDetailDto.amount_net ?? 0;
                            poDetailDto.uom_id = detail.uom_id;

                            //poDetailDto.item_id = detail.item_id;
                            //poDetailDto.item_description = detail.task_detail;
                            //poDetailDto.item_type = detail.item_type_id;
                            //poDetailDto.item_name = detail.item_name;
                            //poDetailDto.purchase_order_id = PoDto.id;
                            //poDetailDto.qty = detail.qty ?? 0;
                            //poDetailDto.unit_price = detail.unit_price ?? 0;
                            //poDetailDto.amount = detail.amount ?? 0;
                            //poDetailDto.uom_id = detail.uom_id;
                            result.Status.Success &= PoDetailService.Svc.SaveEntity(poDetailDto, ref isNew);
                        }
                        result.Data = new
                        {
                            recordID = PoDto.id,
                            vendorquotationid = recordVendor.id
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
                    }
                }
            }
            return result;
        }
    }
}
