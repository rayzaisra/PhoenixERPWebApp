using System;
using System.Collections.Generic;
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
    public partial class RequestForQuotation : RequestForQuotationService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public RequestForQuotation(DataContext AppUserData) : base(AppUserData)
        {

        }

        public bool SaveRfq(RequestForQuotationDetailsDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {

                        var _rfq = RecordDto.RequestForQuotation;
                        if (string.IsNullOrEmpty(_rfq.request_for_quotation_number))
                        {
                            _rfq.request_for_quotation_number = GetNextAutoNumber();
                        }

                        result = Svc.SaveEntity(_rfq, ref isNew);
                        if (!result) throw new Exception("Failed save request for quotation");

                        if (RecordDto.VendorList != null)
                        {
                            var rfqVendorService = new RequestForQuotationVendor(Context);
                            result &= rfqVendorService.DeleteChildPermanent(_rfq.id);
                            if (!result) throw new Exception("Failed delete permanent child request for quotation vendor");

                            foreach (var i in RecordDto.VendorList)
                            {
                                var refIsNew = false;
                                var record = new request_for_quotation_vendorDTO();
                                var companyData = company.FirstOrDefault(" WHERE id=@0", i);
                                if (companyData != null)
                                {
                                    record.email_recipient = companyData.email;
                                }
                                record.request_for_quotation_id = _rfq.id;
                                record.vendor_id = i;

                                result &= rfqVendorService.Svc.SaveEntity(record, ref refIsNew);
                            }
                        }

                        if (!result) throw new Exception("Failed on process re-create RFQ vendor list");


                        if (RecordDto.Details != null)
                        {
                            var rfqDetailsService = new RequestForQuotationDetailService(Context);
                            foreach (var detail in RecordDto.Details)
                            {
                                detail.request_for_quotation_id = _rfq.id;
                                result &= rfqDetailsService.Svc.SaveEntity(detail, ref isNew);
                            }
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
            }


            return result;
        }

        public bool SubmitRfq(String RecordId, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = request_for_quotation.FirstOrDefault(" WHERE id=@0 ", RecordId);
                    if (record == null)
                        throw new Exception("Record not found");

                    var detailService = new RequestForQuotationDetailService(Context);
                    var details = request_for_quotation_detail.Fetch(" WHERE request_for_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                    if (details == null)
                        throw new Exception("Canceled submiting data, Record dont have request for quotation detail");

                    record.is_locked = true;
                    var recordDto = new request_for_quotationDTO();
                    recordDto.InjectFrom(record);
                    result = Svc.SaveEntity(recordDto, ref isNew);
                    if (result)
                    {
                        var detailsDto = new List<request_for_quotation_detailDTO>();
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
                        foreach (var col in request_for_quotation.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(request_for_quotation.DefaultView);
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

                            sql.Append(" ORDER BY r.request_for_quotation_number ");
                            result = db.Page<vw_request_for_quotation>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(request_for_quotation.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.request_for_quotation_number ");
                            result = db.Page<vw_request_for_quotation>(1, rowPerPage, sql);

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

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest, bool isInternal)
        {
            DataResponse result = null;
            try
            {
                var qry = "";
                if (isInternal)
                {
                    qry = String.Format(" r.job_id IS NULL");
                }
                else
                {
                    qry = String.Format(" r.job_id IS NOT NULL");
                }

                result = GetListKendoGrid(DTRequest);
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
            return Context.GenerateAutoNumber("[fn].[sq_rfq]", "RFQ");
        }

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(request_for_quotation.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_request_for_quotation>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.request_for_quotation_number ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_request_for_quotation>(pageDraw, rowPerPage, sql);
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

        public ApiResponse createVendorQuotation(String rfqId, ref bool isNew)
        {
            ApiResponse result = new ApiResponse();
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var record = request_for_quotation.FirstOrDefault(" WHERE id=@0 ", rfqId);

                        if (record == null)
                            throw new Exception("Record not found");

                        var hasBidding = purchase_bidding.FirstOrDefault(" WHERE rfq_id=@0 AND is_active=1 ", rfqId);
                        if (hasBidding != null)
                            throw new Exception("RFQ has releated Purchase Bidding, Cann't RE-Create Vendor Quotation");

                        var hasPO = purchase_order.FirstOrDefault("WHERE purchase_request_id=@0 AND is_active=1 ", record.purchase_request_id);
                        if (hasPO != null)
                            throw new Exception("RFQ has releated Purchase Order, Cann't RE-Create Vendor Quotation");

                        var details = request_for_quotation_detail.Fetch(" WHERE request_for_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                        if (details.Count == 0)
                            throw new Exception("Canceled create vendor quotation, Record dont have request for quotation detail");

                        var vendorRecord = vendor_quotation.FirstOrDefault(" WHERE rfq_id =@0 AND COALESCE(is_locked,0)=1", rfqId);
                        if (vendorRecord != null)
                            throw new Exception("Record vendor quotation is locked");

                        var isRFQVendorExist = request_for_quotation_vendor.Fetch(" WHERE request_for_quotation_id = @0 ", record.id);

                        if (isRFQVendorExist.Count == 0)
                            throw new Exception("Record doesnt have vendor");

                        int deleted = 0;
                        vendorRecord = vendor_quotation.FirstOrDefault(" WHERE rfq_id =@0 AND COALESCE(is_active,0)=1", rfqId);
                        if (vendorRecord != null)
                        {
                            var vendorDetailRecord = vendor_quotation_detail.FirstOrDefault(" WHERE vendor_quotation_id =@0 AND COALESCE(is_active,0)=1", vendorRecord.id);
                            deleted = db.Delete<vendor_quotation_detail>(" WHERE vendor_quotation_id = @0 AND COALESCE(is_locked,0)=0", vendorRecord.id);
                            deleted = db.Delete<vendor_quotation>(" WHERE rfq_id = @0 AND COALESCE(is_locked,0)=0", record.id);
                        }

                        if (deleted >= 0)
                        {
                            foreach (var RFQVendor in isRFQVendorExist)
                            {
                                #region vendorservice
                                var vendorService = new VendorQuotationService(Context);
                                var vendorDetailService = new VendorQuotationDetailService(Context);
                                var vendorDto = new vendor_quotationDTO();                                
                                #endregion

                                vendorDto.id = Guid.NewGuid().ToString();
                                vendorDto.rfq_id = record.id;
                                vendorDto.vendor_reference_number = record.request_for_quotation_number;
                                vendorDto.vendor_id = RFQVendor.vendor_id;
                                vendorDto.vendor_quotation_number = Context.GenerateAutoNumber("[fn].[sq_vendor_quotation]", "VQ");

                                result.Status.Success = vendorService.Svc.SaveEntity(vendorDto, ref isNew);
                                if (result.Status.Success)
                                {
                                    foreach (var detail in details)
                                    {
                                        var vendorDetailDto = new vendor_quotation_detailDTO();
                                        vendorDetailDto.rfq_detail_id = detail.id;
                                        vendorDetailDto.vendor_quotation_id = vendorDto.id;
                                        vendorDetailDto.item_id = detail.item_id;
                                        vendorDetailDto.task_detail = detail.description;
                                        vendorDetailDto.item_type_id = detail.item_type;
                                        vendorDetailDto.item_name = detail.item_name;
                                        vendorDetailDto.qty = detail.qty;
                                        vendorDetailDto.unit_price = detail.unit_price;
                                        vendorDetailDto.currency_id = detail.currency_id;
                                        vendorDetailDto.exchange_rate = detail.exchange_rate;
                                        vendorDetailDto.amount = detail.amount;
                                        vendorDetailDto.uom_id = detail.uom_id;
                                        result.Status.Success &= vendorDetailService.Svc.SaveEntity(vendorDetailDto, ref isNew);                                        
                                    }
                                    result.Data = new
                                    {
                                        recordID = vendorDto.id,
                                        RequestForQuotationId = rfqId
                                    };
                                }
                            }
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
            }
            return result;
        }

        public ApiResponse CreateQuotationByVendorId(String requestForQuotationId, String[] vendorId, ref bool isNew)
        {
            ApiResponse result = new ApiResponse();
            List<dynamic> listNewQuotation = new List<dynamic>();
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var record = request_for_quotation.FirstOrDefault(" WHERE id=@0 ", requestForQuotationId);
                        if (record == null)
                            throw new Exception("Record not found");


                        var details = request_for_quotation_detail.Fetch(" WHERE request_for_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                        if (details == null)
                            throw new Exception("This request doesn't have item(s) for creating quotation.");

                        var qry = Sql.Builder.Append(request_for_quotation_vendor.DefaultView);
                        qry.Append(" WHERE request_for_quotation_id = @0 AND vendor_id in (@1) ", record.id, vendorId.ToArray());
                        var vendorTarget = db.Fetch<vw_request_for_quotation_vendor>(qry);
                        if (vendorTarget == null)
                            throw new Exception("Vendor not registered on this Request");


                        var hasBidding = purchase_bidding.FirstOrDefault(" WHERE rfq_id=@0 AND is_active=1 ", record.id);
                        if (hasBidding != null)
                            throw new Exception("Sorry, can't add Quotation, because this request has prosess to vendor selection");


                        foreach (var vendor in vendorTarget)
                        {
                            var oldQuotation = vendor_quotation.FirstOrDefault(" WHERE rfq_id=@0 AND vendor_id=@1 AND COALESCE(is_deleted,0)=0", record.id, vendor.vendor_id);
                            if (oldQuotation != null)
                                throw new Exception($" Vendor {vendor.vendor_name} Already Exist");

                            #region vendorservice
                            var vendorService = new VendorQuotationService(Context);
                            var vendorDto = new vendor_quotation();

                            //var vqVendorService = new VendorQuotationVendorService(Context);
                            var VQVendorDto = new vendor_quotation_vendor();
                            #endregion

                            vendorDto.id = Guid.NewGuid().ToString();
                            vendorDto.rfq_id = record.id;
                            vendorDto.vendor_reference_number = record.request_for_quotation_number;
                            vendorDto.vendor_id = vendor.vendor_id;
                            vendorDto.vendor_quotation_number = Context.GenerateAutoNumber("[fn].[sq_vendor_quotation]", "VQ");

                            result.Status.Success = Context.SaveEntity<vendor_quotation>(vendorDto, true);
                            if (result.Status.Success)
                            {
                                VQVendorDto.vendor_quotation_id = vendorDto.id;
                                VQVendorDto.amount = 0;
                                VQVendorDto.currency_id = "blm fix";
                                VQVendorDto.delivery_address = "";
                                VQVendorDto.delivery_date = DateTime.Now;
                                VQVendorDto.discount = 0;
                                VQVendorDto.exchange_rate = 1;
                                VQVendorDto.expiry_date = DateTime.Now;
                                VQVendorDto.id = Guid.NewGuid().ToString();
                                VQVendorDto.term_of_payment = "blm fix";
                                VQVendorDto.vendor_id = vendor.id;
                                VQVendorDto.vendor_quotation_id = vendorDto.id;
                                result.Status.Success &= Context.SaveEntity<vendor_quotation_vendor>(VQVendorDto, true);

                                if (result.Status.Success)
                                {
                                    foreach (var detail in details)
                                    {
                                        var vendorDetailDto = new vendor_quotation_detail();
                                        vendorDetailDto.rfq_detail_id = detail.id;
                                        vendorDetailDto.item_id = detail.item_id;
                                        vendorDetailDto.task_detail = detail.description;
                                        vendorDetailDto.item_type_id = detail.item_type;
                                        vendorDetailDto.item_name = detail.item_name;
                                        //vendorDetailDto.vendor_quotation_vendor_id = VQVendorDto.id;
                                        vendorDetailDto.qty = detail.qty;
                                        vendorDetailDto.unit_price = detail.unit_price;
                                        vendorDetailDto.currency_id = detail.currency_id;
                                        vendorDetailDto.exchange_rate = detail.exchange_rate;
                                        vendorDetailDto.amount = detail.amount;
                                        vendorDetailDto.uom_id = detail.uom_id;
                                        result.Status.Success &= Context.SaveEntity<vendor_quotation_detail>(vendorDetailDto, true);
                                    }
                                }

                                listNewQuotation.Add(new
                                {
                                    id = vendor.vendor_id,
                                    vendor.vendor_name,
                                    quotation_id = vendorDto.id
                                });
                            }
                        }

                        result.Data = listNewQuotation;


                        if (result.Status.Success)
                        {
                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Status.Success = false;
                        appLogger.Error(ex);
                        throw;
                    }
                }
            }
            return result;
        }
    }
}
