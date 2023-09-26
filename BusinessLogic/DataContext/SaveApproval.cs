using BusinessLogic.Services.General;
using DataAccess;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BusinessLogic
{
    public partial class DataContext
    {
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

        public bool SaveApproval<T>(String RecordId)
        {
            var result = false;
            object appEntity = DataContext.GetBaseEntity<T>();
            var EntityId = ((IEntity)appEntity).GetEntityId();
            using (var db = new PhoenixERPRepo() )
            {
                var template = approval_template.FirstOrDefault(" WHERE application_entity_id = @0 AND is_active = 1 ", EntityId);
                if (template == null) throw new Exception("Template not found");
                using (var scope = db.GetTransaction())
                {
                    try
                    {
                        var EntityExist = application_entity.Exists(EntityId);
                        if (EntityExist)
                        {
                            var oldApproval = db.ExecuteScalar<int>("SELECT COUNT(id) FROM dbo.approval WHERE application_entity_id=@0 AND record_id=@1 ", EntityId, RecordId);
                            if (oldApproval > 0)
                                return result = true;

                            var template_detail = approval_template_detail.Fetch(" WHERE approval_template_id=@0 AND is_active = 1 ", template.id);
                            if (template_detail != null && template_detail.Count == 0)
                            {
                                throw new Exception("Template doesnt have approver");
                            }

                            var approvalRecord = new approval
                            {
                                application_entity_id = EntityId,
                                record_id = RecordId
                            };
                            result = SaveEntity<approval>(approvalRecord, true);

                            if (result)
                            {
                                var status = approval_status.FirstOrDefault(" WHERE status_name=@0 AND is_active=1 ", "REQUESTED");
                                foreach (var temp in template_detail)
                                {
                                    var approvalDetailRecord = new approval_detail
                                    {
                                        approval_id = approvalRecord.id,
                                        approver_id = temp.approver_id,
                                        approval_status_id = status.id,
                                        approval_level = temp.approval_level
                                    };
                                    result &= SaveEntity<approval_detail>(approvalDetailRecord, true);
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
                        result = false;
                        appLogger.Error($"Entity Id {EntityId} | Record Id {RecordId} | Template Id {template.id}");
                        appLogger.Error(ex);
                    }
                }

            }
            return result;
        }
    }
}
