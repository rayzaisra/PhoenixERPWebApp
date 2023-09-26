using BusinessLogic.Services.General;
using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP;
using CommonTool.Helper;
using NPoco;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Linq;
using DataAccess.PhoenixERP.General;
using DataAccess.Dto.Custom.Finance;
using BusinessLogic.Core;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Finance
{
    public partial class CashAdvance : CashAdvanceService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public CashAdvance(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(cash_advanceDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.ca_number = string.IsNullOrEmpty(RecordDto.ca_number) ?
                            GetNextAutoNumber() : RecordDto.ca_number;
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            return result;
        }

        public bool AddCashAdvanceWithApprovals(CashAdvanceWithApprovals RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var db = new PhoenixERPRepo())
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        var cashAdvanceStatusSvc = new CashAdvanceStatus(Context);
                        var cashAdvanceStatus = cashAdvanceStatusSvc.GetFirstStatus();
                        if (cashAdvanceStatus == null)
                            throw new ArgumentException("Cash Advance status {0} is not found", cash_advance_status.REQUESTED);

                        var cashAdvance = RecordDto.cashAdvance;
                        cashAdvance.id = string.IsNullOrEmpty(cashAdvance.id) ?
                            Guid.NewGuid().ToString() : cashAdvance.id;
                        //cashAdvance.approval_id = string.IsNullOrEmpty(cashAdvance.approval_id) ?
                        //    Guid.NewGuid().ToString() : cashAdvance.approval_id;
                        cashAdvance.ca_status_id = cashAdvanceStatus.id;
                        cashAdvance.ca_number = string.IsNullOrEmpty(cashAdvance.ca_number) ?
                            GetNextAutoNumber() : cashAdvance.ca_number;

                        result = Svc.SaveEntity(cashAdvance, ref isNew);

                        if (result)
                        {
                            var isNewDetail = false;
                            var cashAdvanceDetails = RecordDto.cashAdvanceDetails;
                            var cashAdvanceDetailService = new CashAdvanceDetailService(Context);

                            foreach (var _details in cashAdvanceDetails)
                            {
                                _details.cash_advance_id = cashAdvance.id;
                                result &= cashAdvanceDetailService.Svc.SaveEntity(_details, ref isNewDetail);
                            }

                            //var sql = Sql.Builder.Append("DELETE FROM approval");
                            //sql.Append("WHERE id = @0", cashAdvance.approval_id);
                            //db.Execute(sql);

                            //var isNewApproval = false;
                            //var approvalDto = new approvalDTO();
                            //var approvalService = new ApprovalService(Context);
                            //approvalDto.record_id = cashAdvance.id;
                            //approvalDto.application_entity_id = cash_advance.EntityId;
                            //result &= approvalService.Svc.SaveEntity(approvalDto, ref isNewApproval);

                            //if (result)
                            //{
                            //    var approvalStatusSvc = new ApprovalStatus(Context);
                            //    var approvalStatus = approvalStatusSvc.GetFirstStatus();
                            //    if (approvalStatus == null)
                            //        throw new ArgumentException("Approval status {0} is not found", approval_status.REQUESTED);

                            //    int approvalLevel = 1;
                            //    foreach (var approverId in RecordDto.approverIds)
                            //    {
                            //        var approvalDetail = new approval_detail()
                            //        {
                            //            id = Guid.NewGuid().ToString(),
                            //            created_by = Context.AppUserId,
                            //            created_on = DateTime.Now,
                            //            is_active = true,
                            //            owner_id = approverId.ToString(),
                            //            approval_id = approvalDto.id.ToString(),
                            //            approval_level = approvalLevel,
                            //            approval_status_id = approvalStatus.id
                            //        };

                            //        result &= (db.Insert(approvalDetail) != null);
                            //        approvalLevel++;

                            //        if (!result) break;
                            //    }
                            //}
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

        public bool SubmitCashAdvance(String RecordId, ref bool isNew)
        {
            bool result = false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = cash_advance.FirstOrDefault(" WHERE id=@0 ", RecordId);
                    var approvalRecord = approval.FirstOrDefault(" WHERE record_id=@0 ", RecordId);
                    if (record == null)
                        throw new Exception("Record not found");

                    var _cashAdvanceDetailService = new CashAdvanceDetailService(Context);
                    var details = cash_advance_detail.Fetch(" WHERE cash_advance_id = @0 AND COALESCE(is_deleted,0)=0 ", record.id);
                    if (details == null)
                        throw new Exception("Canceled submiting data, you don't have cash advance detail");

                    record.is_locked = true;
                    //record.approval_id = approvalRecord.id;
                    var recordDto = new cash_advanceDTO();
                    recordDto.InjectFrom(record);
                    result = Svc.SaveEntity(recordDto, ref isNew);
                    if (result)
                    {
                        var detailsDto = new List<cash_advance_detailDTO>();
                        detailsDto.InjectFrom(details);
                        foreach (var detail in detailsDto)
                        {
                            detail.is_locked = true;
                            result &= _cashAdvanceDetailService.Svc.SaveEntity(detail, ref isNew);
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
                        foreach (var col in cash_advance.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(cash_advance.DefaultView);
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

                            sql.Append(" ORDER BY r.ca_number DESC ");
                            result = db.Page<vw_cash_advance>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(cash_advance.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.ca_number DESC ");
                            result = db.Page<vw_bank>(1, rowPerPage, sql);

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
            return Context.GenerateAutoNumber("[fn].[sq_cash_advance]", "CA");
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