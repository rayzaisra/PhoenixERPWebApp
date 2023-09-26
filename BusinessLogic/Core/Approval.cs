using System;
using System.Collections.Generic;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Omu.ValueInjecter;
using NLog;
using System.Transactions;
using System.Dynamic;
using System.Linq;
using BusinessLogic.Services.General;
using NPoco;
using DataAccess.PhoenixERP.General;
using DataAccess.Dto.Custom.General;
using CommonTool.JEasyUI.DataGrid;
using Newtonsoft.Json;

namespace BusinessLogic.Core
{
    public partial class Approval : ApprovalService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Approval(DataContext AppUserData) : base(AppUserData)
        {
        }

        public bool AssignApprovalToEntity(String EntityId, String RecordId, String ApprovalId)
        {
            var success = false;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var entity = application_entity.FirstOrDefault(" WHERE id=@0", EntityId);
                    if (entity == null)
                        throw new Exception($"Entity Id {EntityId} not found ");

                    var qry = $" UPDATE {entity.entity_name} SET approval_id=@0 WHERE id=@1 ";
                    appLogger.Debug(qry);
                    if (db.Execute(qry, ApprovalId, RecordId) > 0)
                    {
                        success = true;
                    }
                    appLogger.Debug(db.LastCommand);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }
            return success;
        }

        public bool removeApprovalEntity(String RecordId, String ApprovalId, String EntityId)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var entity = application_entity.FirstOrDefault(" WHERE id=@0", EntityId);
                        if (entity == null)
                            throw new Exception($"Entity Id {EntityId} not found ");

                        var _approval = approval.FirstOrDefault(" WHERE id=@0", ApprovalId);
                        if (_approval == null)
                            throw new Exception($"Approval id {ApprovalId} not found ");

                        var qry = string.Empty;
                        #region Check Submited
                        qry = $"SELECT cast(COALESCE(is_locked,0) as bit) as is_locked ,cast(COALESCE(is_active,0) as bit) as is_active FROM {entity.entity_name} WHERE id=@0";
                        var oldRecord = db.FirstOrDefault<dynamic>(qry, RecordId);
                        appLogger.Debug(db.LastCommand);
                        if (oldRecord == null)
                            throw new Exception($"Record not found ");

                        if (oldRecord.is_locked && oldRecord.is_active)
                            throw new Exception($"Can't release approval. Entity has been submited !");
                        #endregion

                        qry = string.Empty;
                        qry = $"UPDATE {entity.entity_name} SET approval_id=null WHERE id=@0";
                        appLogger.Debug(qry);
                        if (db.Execute(qry, RecordId) > 0)
                        {
                            success = (db.Delete(_approval) > 0);
                        }
                        appLogger.Debug(db.LastCommand);
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        throw;
                    }
                    finally
                    {
                        if (success)
                        {
                            scope.Complete();
                        }
                    }
                }
                return success;
            }
        }

        public bool SetApprovalByTemplate(String RecordId, String EntityId)
        {
            var result = false;
            using (var scope = new TransactionScope())
            {
                var template = approval_template.FirstOrDefault(" WHERE application_entity_id = @0 AND is_active = 1 ", EntityId);
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var EntityExist = application_entity.Exists(EntityId);
                        if (EntityExist)
                        {
                            var oldApproval = db.ExecuteScalar<int>("SELECT COUNT(id) FROM dbo.approval WHERE application_entity_id=@0 AND record_id=@1 ", EntityId, RecordId);
                            if (oldApproval > 0)
                                throw new Exception("Approval Already Exists");
                            
                            var template_detail = approval_template_detail.Fetch(" WHERE approval_template_id=@0 AND is_active = 1 ", template.id);
                            if (template_detail != null && template_detail.Count == 0)
                            {
                                throw new Exception("Template not found");
                            }

                            var approvalRecord = new approvalDTO();
                            approvalRecord.application_entity_id = EntityId;
                            approvalRecord.record_id = RecordId;
                            bool isNew = false;
                            result = Svc.SaveEntity(approvalRecord, ref isNew);

                            if (result)
                            {
                                var approvalDetailService = new ApprovalDetailService(Context);
                                var status = approval_status.FirstOrDefault(" WHERE status_name=@0 AND is_active=1 ", "REQUESTED");
                                foreach (var temp in template_detail)
                                {
                                    var approvalDetailRecord = new approval_detailDTO();
                                    approvalDetailRecord.approval_id = approvalRecord.id;
                                    approvalDetailRecord.approver_id = temp.approver_id;
                                    approvalDetailRecord.approval_status_id = status.id;
                                    approvalDetailRecord.approval_level = temp.approval_level;
                                    result &= approvalDetailService.Svc.SaveEntity(approvalDetailRecord, ref isNew);
                                }
                                result &= AssignApprovalToEntity(EntityId, RecordId, approvalRecord.id);
                                if (result)
                                {
                                    scope.Complete();
                                }
                            }

                        }
                        else
                        {
                            throw new Exception("Entity not found");
                        }


                    }
                    catch (Exception ex)
                    {
                        appLogger.Error($"Entity Id {EntityId} | Record Id {RecordId} | Template Id {template.id}");
                        appLogger.Error(ex);
                        throw;
                    }
                }

            }
            return result;
        }

        public List<vw_approval_detail> GetApprovalDetails(string RecordId)
        {
            List<vw_approval_detail> result = null;

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append(approval_detail.DefaultView);
                    sql.Append("WHERE r.approval_id IN (");
                    sql.Append("SELECT id FROM approval");
                    sql.Append("WHERE record_id = @0)", RecordId);

                    result = db.Fetch<vw_approval_detail>(sql);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex.ToString());
                    throw;
                }
            }

            return result;
        }

        public dynamic Lookup(string Id = null, int Page = 1, int Length = 100,
            string OptionalFilters = null, string OptionalParameters = null)
        {
            dynamic result = null;

            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(OptionalFilters))
                    {
                        foreach (var col in approval_status.DefaultViewColumns)
                        {
                            filters = OptionalFilters.Replace(" " + col.Key + " ", " " + col.Value + " ");
                        }
                        filters = filters.Replace(";", "--");
                    }

                    parameters = OptionalParameters;
                    int pageDraw = Page;
                    int rowPerPage = Length;

                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            var sql = Sql.Builder.Append(approval_status.DefaultView);
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

                            sql.Append(" ORDER BY r.created_on ");
                            result = db.Page<vw_approval_status>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(approval_status.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.created_on ");

                            result = db.Page<vw_approval_status>(1, rowPerPage, sql);
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

        public dynamic GetApprovalStatusGroupLevel(List<string> ApprovalIds)
        {
            var result = new List<approval_detail_data_by_group_idDTO>();
            using (var db = new PhoenixERPRepo())
            {
                try
                { 
                    var qry = " SELECT "
                                + " r.id, "
                                + " r.previous_approval, "
                                + " r.approval_level, "
                                + " r.expiry_datetime, "
                                + " r.is_auto_approved, "
                                + " r.approver_id, "
                                + " u4.app_fullname, "
                                + " r.approval_status_id, "
                                + " [dbo].[ufn_current_approval_status](r.id) AS status_name, "
                                + " r.approval_id "
                                + " FROM            dbo.approval_detail AS r "
                                + " LEFT OUTER JOIN dbo.application_user AS u4 ON r.approver_id = u4.id "
                                + " LEFT OUTER JOIN dbo.approval_status AS a2 ON r.approval_status_id = a2.id "
                                + " LEFT OUTER JOIN dbo.approval AS a1 ON r.approval_id = a1.id ";
                    var sql = Sql.Builder.Append(qry);
                    sql.Append(string.Format("WHERE r.approval_id IN ( {0} )",
                        string.Join(",", Array.ConvertAll(ApprovalIds.ToArray(),
                        x => string.Format("'{0}'", x.ToString())))));
                    var data = db.Fetch<approval_detail_dataDTO>(sql);

                    result = data.GroupBy(g => new { g.approval_id }, (key, group) => new approval_detail_data_by_group_idDTO()
                    {
                        approval_id = key.approval_id,
                        data = group.GroupBy(g=>g.approval_level, (keyLevel, groupLevel) => new approval_detail_data_by_group_levelDTO() {
                            approval_level = keyLevel.Value,
                            details = groupLevel.ToList()
                        }).OrderBy(o=>o.approval_level).ToList()
                    }).ToList();

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

            return result;
        }


        public dynamic GetApprovalStatus(List<string> ApprovalIds)
        {
            var result = new List<approval_detail_data_by_groupDTO>();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var qry = " SELECT "
                                + " r.id, "
                                + " r.previous_approval, "
                                + " r.approval_level, "
                                + " r.expiry_datetime, "
                                + " r.is_auto_approved, "
                                + " r.approver_id, "
                                + " u4.app_fullname, "
                                + " r.approval_status_id, "
                                + " a2.status_name, "
                                + " r.approval_id "
                                + " FROM            dbo.approval_detail AS r "
                                + " LEFT OUTER JOIN dbo.application_user AS u4 ON r.approver_id = u4.id "
                                + " LEFT OUTER JOIN dbo.approval_status AS a2 ON r.approval_status_id = a2.id "
                                + " LEFT OUTER JOIN dbo.approval AS a1 ON r.approval_id = a1.id ";
                    var sql = Sql.Builder.Append(qry);
                    sql.Append(string.Format("WHERE r.approval_id IN ( {0} )",
                        string.Join(",", Array.ConvertAll(ApprovalIds.ToArray(),
                        x => string.Format("'{0}'", x.ToString())))));
                    var data = db.Fetch<approval_detail_dataDTO>(sql);

                    result = data.GroupBy(g => new { g.approval_id }, (key, group) => new approval_detail_data_by_groupDTO()
                    {
                        approval_id = key.approval_id,
                        details = group.ToList()
                    }).ToList();

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

            return result;
        }

        public bool SetApprovalStatus(string ApprovalStatusId, String ApprovalDetailId)
        {
            var result = false;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var getApprovalStatus = db.FirstOrDefault<vw_approval_status>(" WHERE status_name=@0", ApprovalStatusId);
                    if (getApprovalStatus == null)
                        throw new Exception("Approval Status not found !");

                    var ApprovalRequested = db.FirstOrDefault<approval_detail>(" WHERE id=@0 ", ApprovalDetailId);
                    if(ApprovalRequested==null)
                        throw new Exception("Approval Requested not found !");

                    //SET Approval Status ID
                    ApprovalRequested.approval_status_id = getApprovalStatus.id;
                    result = Context.SaveEntity<approval_detail>(ApprovalRequested,false);
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

            return result;
        }

        public string GetNextAutoNumber()
        {
            return Context.GenerateAutoNumber("[dbo].[sq_aproval]", "APRV");
        }
    }
}
