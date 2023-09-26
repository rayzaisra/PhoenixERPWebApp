using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.General;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;
using System.Transactions;
using DataAccess.Dto.Custom.Finance;
using DataAccess.PhoenixERP.General;
using BusinessLogic.Core;
using CommonTool.JEasyUI.DataGrid;
using Omu.ValueInjecter;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Services
{
    public partial class PurchaseOrder : PurchaseOrderService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public PurchaseOrder(DataContext AppUserData) : base(AppUserData)
        {

        }

        public bool SaveToDraft(PurchaseOrderDetails RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var _purchase_order = RecordDto.purchase_order;
                    var _purchase_order_details = RecordDto.purchase_order_details;
                    var isExist = purchase_order.Exists(_purchase_order.id);
                    //if (!isExist)
                    //{
                    _purchase_order.id = string.IsNullOrEmpty(_purchase_order.id) ?
                        Guid.NewGuid().ToString() : _purchase_order.id;
                    _purchase_order.organization_id = string.IsNullOrEmpty(_purchase_order.organization_id) ?
                        Context.OrganizationId : _purchase_order.organization_id;
                    _purchase_order.purchase_order_number = string.IsNullOrEmpty(_purchase_order.purchase_order_number) ?
                        string.Format("PO-{0}", CommonTool.Helper.RandomString.Generate(5)) : _purchase_order.purchase_order_number;
                    result = Svc.SaveEntity(_purchase_order, ref isNew);
                    //}
                    //else
                    //    return result = true;

                    if (result)
                    {
                        var _puchase_order_detailsService = new PurchaseOrderDetailService(Context);

                        var isNewDetails = false;
                        // setup approval
                        var isNewApproval = false;
                        var approvalDto = new approvalDTO();
                        var approvalService = new ApprovalService(Context);
                        foreach (var item in _purchase_order_details)
                        {
                            item.purchase_order_id = _purchase_order.id;
                            result &= _puchase_order_detailsService.SaveEntity(item, ref isNewDetails);
                        }

                        if (!string.IsNullOrEmpty(RecordDto.approverId))
                        {
                            var isApproval = approval.FirstOrDefault(" WHERE record_id = @0 ", _purchase_order.id);
                            if (isApproval == null)
                            {
                                approvalDto.id = Guid.NewGuid().ToString();
                            }
                            else
                            {
                                approvalDto.id = isApproval.id;
                            }


                            approvalDto.record_id = _purchase_order.id;
                            approvalDto.application_entity_id = purchase_order.EntityId;
                            result &= approvalService.Svc.SaveEntity(approvalDto, ref isNewApproval);
                            // setup approvaldetail
                            var approvalStatusSvc = new ApprovalStatus(Context);
                            var approvalStatus = approvalStatusSvc.GetFirstStatus();
                            if (approvalStatus == null)
                                throw new ArgumentException("Approval status {0} is not found", approval_status.REQUESTED);

                            int approvalLevel = 1;

                            var approvalDetail = new ApprovalDetailService(Context);
                            var _approval_details = new approval_detailDTO();
                            var isNewApproveDetails = false;
                            if (!isNewApproval)
                            {
                                var isApprovalDetail = approval_detail.FirstOrDefault(" WHERE approval_id = @0 ", approvalDto.id);
                                _approval_details.id = isApprovalDetail.id;
                            }
                            _approval_details.approval_level = approvalLevel;
                            _approval_details.approver_id = RecordDto.approverId;
                            _approval_details.owner_id = RecordDto.approverId;
                            _approval_details.approval_status_id = approvalStatus.id;
                            _approval_details.approval_id = approvalDto.id;
                            result &= approvalDetail.SaveEntity(_approval_details, ref isNewApproveDetails);

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

        public bool SubmitPurchaseOrder(String RecordId, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = purchase_order.FirstOrDefault(" WHERE id=@0 ", RecordId);
                    var approvalRecord = approval.FirstOrDefault(" WHERE record_id=@0 ", RecordId);
                    if (record == null)
                        throw new Exception("Record not found");

                    var _purchaseDetailService = new PurchaseOrderDetailService(Context);
                    var details = purchase_order_detail.Fetch(" WHERE purchase_order_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                    if (details == null)
                        throw new Exception("Canceled submiting data, you don't have purchase order detail");

                    record.is_locked = true;
                    record.approval_id = approvalRecord.id;
                    var recordDto = new purchase_orderDTO();
                    recordDto.InjectFrom(record);
                    result = Svc.SaveEntity(recordDto, ref isNew);
                    if (result)
                    {
                        var detailsDto = new List<purchase_order_detailDTO>();
                        detailsDto.InjectFrom(details);
                        foreach (var detail in detailsDto)
                        {
                            detail.is_locked = true;
                            result &= _purchaseDetailService.Svc.SaveEntity(detail, ref isNew);
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


        public dynamic Lookup(String Id = null, int page = 1, int length = 100, String optionalFilters = null, String optionalParameters = null)
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
                        foreach (var col in purchase_order.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(purchase_order.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            if (!String.IsNullOrEmpty(filters))
                            {
                                if (String.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.purchase_order_number DESC ");
                            result = db.Page<vw_purchase_order>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(purchase_order.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.purchase_order_number DESC ");
                            result = db.Page<vw_purchase_order>(1, rowPerPage, sql);
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

        public virtual vw_purchase_orderDetailsDTO DetailsPurchaseOrder(String PurchaseOrderId)
        {
            vw_purchase_orderDetailsDTO result = null;

            using (var db = new PhoenixERPRepo())
            {
                try
                {

                    var po_record = purchase_order.FirstOrDefault(" WHERE id=@0 ", PurchaseOrderId);
                    if (po_record == null) return result;

                    string qry = "";
                    result = new vw_purchase_orderDetailsDTO();
                    var _purchaseOrderService = new PurchaseOrderService(Context);
                    var po_details = _purchaseOrderService.Details(po_record.id);
                    appLogger.Debug(po_details);
                    result.purchase_order = po_details;
                    if (result.purchase_order != null)
                    {
                        var approvalId = approval.FirstOrDefault(" WHERE record_id=@0", PurchaseOrderId);
                        if (approvalId != null)
                        {
                            var approvalDetails = approval_detail.FirstOrDefault(" WHERE approval_id=@0", approvalId.id);
                            qry = application_user.DefaultView;
                            qry += " WHERE r.id=@0 ";
                            var data = db.FirstOrDefault<application_user>(qry, approvalDetails.approver_id);
                            appLogger.Debug(db.LastCommand);
                            result.approverId = data.id;
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

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(purchase_order.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_purchase_order>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.purchase_order_number ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_purchase_order>(pageDraw, rowPerPage, sql);
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
    }
}
