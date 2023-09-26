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
using CodeMarvel.Infrastructure.DefaultConfiguration;
using DataAccess;

namespace BusinessLogic.Services
{
    public partial class InvoicePayable : InvoicePayableService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public InvoicePayable(DataContext AppUserData) : base(AppUserData)
        {

        }

        public new vw_InvoicePayableDTO Details(String Id)
        {
            vw_InvoicePayableDTO result = new vw_InvoicePayableDTO();

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var sql = Sql.Builder.Append(invoice_payable.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_invoice_payable>(sql);
                    if (view != null)
                    {
                        result.Details = (vw_invoice_payableDTO)((new vw_invoice_payableDTO()).InjectFrom(view));

                        sql = Sql.Builder.Append(invoice_payable_attachment.DefaultView);
                        sql.Append(" WHERE r.is_active = 1 ");
                        sql.Append(" AND r.invoice_payable_id = @0", Id);

                        var viewAttachment = db.Fetch<vw_invoice_payable_attachment>(sql);
                        if (viewAttachment != null)
                        {
                            result.attachment = viewAttachment.
                                Select(x => new vw_invoice_payable_attachmentDTO().InjectFrom(x)).Cast<vw_invoice_payable_attachmentDTO>().ToList();
                        }

                        sql = Sql.Builder.Append(invoice_payable_approval.DefaultView);

                        var viewApproval = db.Fetch<vw_invoice_payable_approval>(sql);
                        sql.Append(" WHERE r.is_active = 1 ");
                        sql.Append(" AND r.record_id = @0", Id);

                        if (viewApproval != null)
                        {
                            result.approval = viewApproval.
                                Select(x => new vw_invoice_payable_approvalDTO().InjectFrom(x)).Cast<vw_invoice_payable_approvalDTO>().ToList();
                        }
                    }
                    else
                    {
                        result = null;
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

        public bool SaveInvoice(InvoicePayableDTO RecordDto, ref bool isNew, ref string id)
        {
            bool result = false;
            bool isFrontDesk = false;

            if (RecordDto == null) return result;

            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var invoicePayable = RecordDto.invoice_payable;

                        var invoiceApproval = RecordDto.invoice_approval;
                        if (string.IsNullOrEmpty(invoiceApproval.invoice_payable_status_id)) throw new Exception("you dont have role to save this invoice");

                        var userSession = Context.userSession.AppRole;
                        if (userSession.Any(e => (e.Id == ApplicationRole.ADMIN_FA)))
                        {
                            isFrontDesk = false;
                        }
                        else if (userSession.Any(e => (e.Id == ApplicationRole.FRONT_DESK)))
                        {
                            isFrontDesk = true;
                        }

                        if (invoice_payable.FirstOrDefault(" WHERE id=@0 ", invoicePayable.id) == null && !isFrontDesk)
                            throw new Exception("Admin FA cant create invoice first, please ask frontdesk team");

                        invoicePayable.id = string.IsNullOrEmpty(invoicePayable.id) ?
                            Guid.NewGuid().ToString() : invoicePayable.id;
                        invoicePayable.organization_id = Context.OrganizationId;
                        //invoicePayable.owner_id = isOwner;
                        invoicePayable.owner_id = Context.AppUserId;
                        invoicePayable.invoice_number = string.IsNullOrEmpty(invoicePayable.invoice_number) ?
                            GetNextAutoNumber() : invoicePayable.invoice_number;
                        result = Svc.SaveEntity(invoicePayable, ref isNew);

                        if (result)
                        {
                            id = invoicePayable.id;
                            var attachService = new InvoicePayableAttachmentService(Context);
                            int delete = db.Delete<invoice_payable_attachment>(" WHERE financial_document_type_id NOT IN (@0) AND invoice_payable_id=@1 ", RecordDto.invoice_attachment.Select(x => x.financial_document_type_id).ToArray(), invoicePayable.id);
                            appLogger.Debug(db.LastCommand);
                            appLogger.Info(delete);
                            foreach (var item in RecordDto.invoice_attachment)
                            {
                                item.invoice_payable_id = invoicePayable.id;
                                result &= attachService.Svc.SaveEntity(item, ref isNew);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
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

        public bool SubmitInvoicePayable(string InvoicePayableId, InvoicePayableDTO invoice, ref bool isNew)
        {
            bool result = false;
            bool isFrontDesk = false;
            object appEntity = DataContext.GetBaseEntity<vw_invoice_payable>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var invoiceApproval = invoice.invoice_approval;
                    var record = invoice_payable.FirstOrDefault(" WHERE id=@0 ", InvoicePayableId);
                    if (record == null)
                        throw new ArgumentException("Record not found");
                    if (string.IsNullOrEmpty(invoiceApproval.invoice_payable_status_id)) throw new Exception("you dont have role to submit this invoice");

                    var userSession = Context.userSession.AppRole;
                    if (userSession.Any(e => (e.Id == ApplicationRole.ADMIN_FA)))
                    {
                        isFrontDesk = false;
                    }
                    else if (userSession.Any(e => (e.Id == ApplicationRole.FRONT_DESK)))
                    {
                        isFrontDesk = true;
                    }

                    int can_update = db.ExecuteScalar<int>("SELECT dbo.ufn_can_update(@0,@1,@2,@3) ", Context.AppUserId, ((IEntity)appEntity).GetEntityId(), record.id, record.owner_id);
                    if (can_update == 0) throw new ArgumentException("Cant submit in currently user");

                    var sql = Sql.Builder.Append(invoice_payable_status.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.status_level = @0 ", invoiceApproval.invoice_payable_status_id);
                    var status = db.FirstOrDefault<invoice_payable_status>(sql);

                    sql = Sql.Builder.Append("SELECT TOP 1 invoice_payable_status_id from fn.invoice_payable_approval ");
                    sql.Append(" WHERE record_id = @0 ", InvoicePayableId);
                    sql.Append(" ORDER BY created_on DESC ");

                    var isApprove = db.ExecuteScalar<string>(sql);

                    var isOwner = "";
                    if ((isApprove == null || status.status_level == 100) && isFrontDesk)
                    {
                        isOwner = "84dcef78-d2f8-4f97-a4ab-973258a3ff25"; // admin fa team   
                    }
                    else if (status.status_level == 1000 && !isFrontDesk)
                    {
                        isOwner = "84dcef78-d2f8-4f97-a4ab-973258a3ff25"; // admin fa team   
                        //record.is_locked = true;
                    }
                    else
                    {
                        isOwner = "ef263763-72cc-4864-a4aa-2d6f62c9bb3c"; // frontdesk team
                    }

                    record.owner_id = isOwner;
                    var RecordDto = new invoice_payableDTO();
                    RecordDto.InjectFrom(record);
                    result = Svc.SaveEntity(RecordDto, ref isNew);

                    if (result)
                    {
                        var approvalService = new InvoicePayableApprovalService(Context);
                        var attachService = new InvoicePayableAttachmentService(Context);
                        int delete = db.Delete<invoice_payable_attachment>(" WHERE financial_document_type_id NOT IN (@0) AND invoice_payable_id=@1 ", invoice.invoice_attachment.Select(x => x.financial_document_type_id).ToArray(), InvoicePayableId);
                        appLogger.Debug(db.LastCommand);
                        appLogger.Info(delete);
                        foreach (var item in invoice.invoice_attachment)
                        {
                            item.invoice_payable_id = InvoicePayableId;
                            result &= attachService.Svc.SaveEntity(item, ref isNew);
                        }

                        invoiceApproval.record_id = InvoicePayableId;
                        invoiceApproval.invoice_payable_status_id = status.id;
                        result &= approvalService.Svc.SaveEntity(invoiceApproval, ref isNew);
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
                        foreach (var col in invoice_payable.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(invoice_payable.DefaultView);
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

                            sql.Append(" ORDER BY r.created_on DESC ");
                            result = db.Page<vw_invoice_payable>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(invoice_payable.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.created_on DESC ");
                            result = db.Page<vw_invoice_payable>(1, rowPerPage, sql);

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
            return Context.GenerateAutoNumber("[fn].[sq_invoice_payable]", "INVP");
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
                var sql = Sql.Builder.Append(invoice_payable.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_invoice_payable>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.invoice_number ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_invoice_payable>(pageDraw, rowPerPage, sql);
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
