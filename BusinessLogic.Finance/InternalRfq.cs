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
using DataAccess.Dto.Custom.Finance;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Services
{
    public partial class InternalRfq: InternalRfqService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InternalRfq(DataContext AppUserData) : base(AppUserData)
        {

        }

        public bool SaveRfq(InternalRfqDetailsDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                
                var _rfq = RecordDto.InternalRfq;
               _rfq.internal_rfq_number = string.Format("RFQ-{0}",
                            CommonTool.Helper.RandomString.Generate(5).ToUpper());
                
                result = Svc.SaveEntity(_rfq, ref isNew);
                if (result)
                {
                    var rfqDetailsService = new InternalRfqDetailService(Context);
                    foreach (var detail in RecordDto.Details)
                    {
                        detail.internal_rfq_id = _rfq.id;
                        
                        result &= rfqDetailsService.Svc.SaveEntity(detail,ref isNew);
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


        public bool createVendorQuotation(String rfqId, String vendorId, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = internal_rfq.FirstOrDefault(" WHERE id=@0 ", rfqId);

                    if (record == null)
                        throw new Exception("Record not found");

                    var detailService = new RequestForQuotationDetailService(Context);
                    var details = request_for_quotation_detail.Fetch(" WHERE internal_rfq_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                    if (details == null)
                        throw new Exception("Canceled create vendor quotation, Record dont have request for quotation detail");

                    var vendorRecord = vendor_quotation.FirstOrDefault(" WHERE rfq_id =@0 ", rfqId);
                    if (vendorRecord!=null)
                        throw new Exception("Record Already created vendor quotation");

                    #region vendorservice
                    var vendorService = new VendorQuotationService(Context);
                    var vendorDetailService = new VendorQuotationDetailService(Context);
                    var vendorDto = new vendor_quotationDTO();
                    #endregion
                    vendorDto.id = Guid.NewGuid().ToString();
                    vendorDto.rfq_id = record.id;
                    vendorDto.vendor_reference_number = record.internal_rfq_number;
                    vendorDto.vendor_id = vendorId;
                    vendorDto.vendor_quotation_number = Context.GenerateAutoNumber("[fn].[sq_vendor_quotation]", "VQ");

                    result = vendorService.Svc.SaveEntity(vendorDto, ref isNew);
                    if (result)
                    {
                        var detailsDto = new List<internal_rfq_detailDTO>();

                        detailsDto.InjectFrom(details);
                        foreach (var detail in detailsDto)
                        {
                            var vendorDetailDto = new vendor_quotation_detailDTO();
                            vendorDetailDto.item_id = detail.item_id;
                            vendorDetailDto.task_detail = detail.item_type;
                            //vendorDetailDto.vendor_quotation_id = vendorDto.id;
                            vendorDetailDto.qty = detail.qty;
                            vendorDetailDto.unit_price = detail.unit_price;
                            vendorDetailDto.amount = detail.amount;
                            result &= vendorDetailService.Svc.SaveEntity(vendorDetailDto, ref isNew);
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

        public bool SubmitRfq(String RecordId, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = internal_rfq.FirstOrDefault(" WHERE id=@0 ",RecordId);
                    if (record == null)
                        throw new Exception("Record not found");

                    var detailService = new InternalRfqDetailService(Context);
                    var details = internal_rfq_detail.Fetch(" WHERE internal_rfq_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                    if (details == null)
                        throw new Exception("Canceled submiting data, Record dont have internal request for quotation detail");

                    record.is_locked = true;
                    var recordDto = new internal_rfqDTO();
                    recordDto.InjectFrom(record);
                    result = Svc.SaveEntity(recordDto, ref isNew);
                    if (result)
                    {
                        var detailsDto = new List<internal_rfq_detailDTO>();
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
                        foreach (var col in internal_rfq.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(internal_rfq.DefaultView);
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

                            sql.Append(" ORDER BY r.internal_rfq_number ");
                            result = db.Page<vw_internal_rfq>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(internal_rfq.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.internal_rfq_number ");
                            result = db.Page<vw_internal_rfq>(1, rowPerPage, sql);

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
    }
}
