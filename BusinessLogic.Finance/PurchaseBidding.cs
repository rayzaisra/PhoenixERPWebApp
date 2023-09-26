using BusinessLogic.Services.Finance;
using CodeMarvel.Infrastructure.ModelShared;
using CommonTool.KendoUI.Grid;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.Finance;
using NLog;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace BusinessLogic.Finance
{
    public partial class PurchaseBidding : PurchaseBiddingService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public PurchaseBidding(DataContext AppUserData) : base(AppUserData)
        {

        }


        public List<purchase_bidding_detailDTO> getItemApproved(String biddingId)
        {
            var result = new List<purchase_bidding_detailDTO>();
            try
            {
                var data = purchase_bidding_detail.Fetch("WHERE purchase_bidding_id=@0 AND is_approved=1 ", biddingId);
                result = data.Select(x => new purchase_bidding_detailDTO().InjectFrom(x)).Cast<purchase_bidding_detailDTO>().ToList();
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
            }
            return result;
        }

        public bool saveBidding(string[] vendorQuotationIds, string biddingId)
        {
            var result = false;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var update = db.Execute("UPDATE fn.purchase_bidding_detail SET is_approved=0 WHERE purchase_bidding_id=@0 AND is_active=1 AND is_approved=1 ", biddingId);
                        result = true;
                        foreach (var id in vendorQuotationIds)
                        {
                            var biddingDetailService = new PurchaseBiddingDetailService(Context);
                            var record = purchase_bidding_detail.FirstOrDefault("WHERE vendor_quotation_detail_id=@0 AND is_active=1 ", id);
                            record.is_approved = true;
                            result &= biddingDetailService.Context.SaveEntity<purchase_bidding_detail>(record, false);
                        }

                        if (result)
                        {
                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool createBidding(String rfqId, ref bool isNew, ref string id)
        {
            bool result = false;

            if (rfqId == null) return result;
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        //has submitted
                        //var vendorRecord = vendor_quotation.FirstOrDefault(" WHERE rfq_id =@0 AND COALESCE(is_locked,0)=1", rfqId);

                        //bypass submitted
                        var vendorRecord = vendor_quotation.Fetch(" WHERE rfq_id =@0 AND COALESCE(is_locked,0)=0", rfqId);
                        if (vendorRecord.Count == 0)
                            throw new Exception("Record not found");

                        var header = request_for_quotation.FirstOrDefault(" WHERE id=@0 ", rfqId);
                        if (header.is_prefered_vendor ?? false == true)
                            throw new Exception("Cant create bidding, Record is preferred vendor");

                        var isExist = purchase_bidding.FirstOrDefault(" WHERE rfq_id =@0 ", rfqId);

                        if (isExist != null)
                            throw new Exception("Cant create bidding, Record is Exist");

                        #region bidding
                        var biddingService = new PurchaseBiddingService(Context);
                        var biddingDetailService = new PurchaseBiddingDetailService(Context);
                        var biddingDto = new purchase_biddingDTO();
                        #endregion
                        biddingDto.id = Guid.NewGuid().ToString();
                        biddingDto.rfq_id = rfqId;
                        biddingDto.bidding_date = DateTime.Now;
                        biddingDto.bidding_number = string.Format("PB-{0}",
                             CommonTool.Helper.RandomString.Generate(5).ToUpper());
                        result = biddingService.Svc.SaveEntity(biddingDto, ref isNew);
                        id = biddingDto.id;
                        if (result)
                        {
                            foreach (var v in vendorRecord)
                            {
                                var vendorDetails = vendor_quotation_detail.Fetch(" WHERE vendor_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", v.id);
                                foreach (var vDetail in vendorDetails)
                                {
                                    var bDetail = new purchase_bidding_detailDTO();
                                    bDetail.purchase_bidding_id = biddingDto.id;
                                    bDetail.rfq_id = rfqId;
                                    bDetail.vendor_quotation_detail_id = vDetail.id;
                                    bDetail.rfq_detail_id = vDetail.rfq_detail_id;
                                    result = biddingDetailService.Svc.SaveEntity(bDetail, ref isNew);


                                }
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
            }

            return result;
        }

        public ApiResponse createBiddingByVendor(String requestForQuotationId, String[] vendorIds)
        {
            var result = new ApiResponse();
            var scopeComplete = false;
            if (requestForQuotationId == null) throw new ArgumentException("Request for quotation unique id is required");
            if (vendorIds == null || vendorIds.Length == 0) throw new ArgumentException("Please choose vendor(s) for vendor selection");
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        //has submitted
                        //var vendorRecord = vendor_quotation.FirstOrDefault(" WHERE rfq_id =@0 AND COALESCE(is_locked,0)=1", rfqId);

                        //bypass submitted

                        var header = request_for_quotation.FirstOrDefault(" WHERE id=@0 ", requestForQuotationId);
                        if (header.is_prefered_vendor ?? false == true)
                            throw new Exception("Can't create bidding, Multiple vendor only");

                        //is_locked seharusnya adalah cek approval
                        var quotations = vendor_quotation.Fetch(" WHERE rfq_id =@0 AND COALESCE(is_locked,0)=1 AND COALESCE(is_deleted,0)=0 ", requestForQuotationId);
                        if (quotations == null) throw new Exception("Noting vendor(s) quotation");

                        var isExist = purchase_bidding.FirstOrDefault(" WHERE rfq_id =@0 AND COALESCE(is_deleted,0)=0 ", requestForQuotationId);
                        if (isExist != null)
                            throw new Exception("Bidding already Exist");

                        #region bidding
                        var biddingService = new PurchaseBiddingService(Context);
                        var biddingDetailService = new PurchaseBiddingDetailService(Context);
                        var biddingDto = new purchase_biddingDTO();
                        #endregion
                        biddingDto.id = Guid.NewGuid().ToString();
                        biddingDto.rfq_id = requestForQuotationId;
                        biddingDto.bidding_date = DateTime.Now;
                        biddingDto.bidding_number = string.Format("PB-{0}",
                             CommonTool.Helper.RandomString.Generate(5).ToUpper());

                        scopeComplete = Context.SaveEntity(biddingDto, true);
                        if (!scopeComplete) throw new Exception("Aborted process bidding saving");

                        foreach (var vendor in vendorIds)
                        {
                            var dataVendor = company.FirstOrDefault(" WHERE id=@0", vendor);
                            #region  Validation
                            if (dataVendor == null) throw new Exception($"vendor id {vendor} not found");
                            if (!quotations.Exists(o => o.vendor_id == vendor)) throw new ArgumentException($"vendor {dataVendor.company_name} doesn't have quotation");
                            #endregion





                        }

                    }
                    catch (Exception ex)
                    {
                        result.Status.Success = false;
                        result.Status.Message = ex.Message;
                        appLogger.Error(ex);
                    }
                }
            }

            return result;
        }

        public bool createBiddingFromRFQ(String rfqId, string[] listVendorQuotationId, ref bool isNew, ref string id)
        {
            bool result = false;

            if (rfqId == null) return result;
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        //bypass submitted
                        var vendorRecord = vendor_quotation.Fetch(" WHERE rfq_id =@0 AND COALESCE(is_locked,0)=0", rfqId);
                        if (vendorRecord.Count == 0)
                            throw new Exception("Record not found");

                        var header = request_for_quotation.FirstOrDefault(" WHERE id=@0 ", rfqId);
                        if (header.is_prefered_vendor ?? false == true)
                            throw new Exception("Cant create bidding, Record is preferred vendor");

                        var isExist = purchase_bidding.FirstOrDefault(" WHERE rfq_id =@0 ", rfqId);

                        if (isExist != null)
                            throw new Exception("Cant create bidding, Record is Exist");

                        #region bidding
                        var biddingService = new PurchaseBiddingService(Context);
                        var biddingDetailService = new PurchaseBiddingDetailService(Context);
                        var biddingDto = new purchase_biddingDTO();
                        #endregion
                        biddingDto.id = Guid.NewGuid().ToString();
                        biddingDto.rfq_id = rfqId;
                        biddingDto.bidding_date = DateTime.Now;
                        biddingDto.bidding_number = string.Format("PB-{0}",
                             CommonTool.Helper.RandomString.Generate(5).ToUpper());
                        result = biddingService.Svc.SaveEntity(biddingDto, ref isNew);
                        id = biddingDto.id;
                        if (result)
                        {
                            foreach (var v in listVendorQuotationId)
                            {
                                var vendorDetails = vendor_quotation_detail.Fetch(" WHERE vendor_quotation_id = @0 AND COALESCE(is_deleted,0)=0 ", v);
                                foreach (var vDetail in vendorDetails)
                                {
                                    var bDetail = new purchase_bidding_detailDTO();
                                    bDetail.purchase_bidding_id = biddingDto.id;
                                    bDetail.rfq_id = rfqId;
                                    bDetail.vendor_quotation_detail_id = vDetail.id;
                                    bDetail.rfq_detail_id = vDetail.rfq_detail_id;
                                    result = biddingDetailService.Svc.SaveEntity(bDetail, ref isNew);
                                }
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
            }

            return result;
        }
    }
}
