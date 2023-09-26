using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using BusinessLogic.DataContextExtension;
using NPoco;
using System.Transactions;

#region Nested Approval DataContext
namespace BusinessLogic.DataContextExtension
{
    public class ApprovalExtension
    {
        static NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        private static DataContext _dataContext;
        public ApprovalExtension(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public approval_template GetTemplate<TEntity>()
        {
            var result = new approval_template();

            try
            {
                var appEntity = Activator.CreateInstance<TEntity>();

                if (typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
                {
                    using (var db = PhoenixERPRepo.GetInstance())
                    {
                        result = approval_template.FirstOrDefault(" WHERE organization_id=@0 AND application_entity_id=@1 AND COALESCE(is_deleted,0)=0 ",
                            _dataContext.OrganizationId, ((IEntity)appEntity).GetEntityId());

                        if (result == null) throw new Exception("Entity doesn't have approval template, Please setup approval template. And try again");

                        return result;
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;

        }

        public List<approval_template_detail> GetTemplateDetail<TEntity>(String approvalTemplateId)
        {
            var result = new List<approval_template_detail>();

            try
            {
                var appEntity = Activator.CreateInstance<TEntity>();

                if (typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
                {
                    using (var db = PhoenixERPRepo.GetInstance())
                    {
                        result = approval_template_detail.Fetch(" WHERE approval_template_id=@0 ", approvalTemplateId);
                        if (result == null) throw new Exception("Approval template doesn't have detail approval, Please setup approval template. And try again");

                        return result;
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;

        }
        public StatusExt Status = new StatusExt();
        public class StatusExt
        {
            public List<approval_status> Get
            {
                get
                {
                    return approval_status.Fetch("WHERE organization_id=@0 AND COALESCE(is_active,0)=1", _dataContext.OrganizationId);
                }
            }

            public string GetRequestId
            {
                get
                {
                    return approval_status.FirstOrDefault("WHERE organization_id=@0 AND COALESCE(is_active,0)=1 AND status_name='REQUESTED' ", _dataContext.OrganizationId).id;
                }
            }

            public string GetIdByStatusName(string statusName)
            {
                return approval_status.FirstOrDefault("WHERE organization_id=@0 AND COALESCE(is_active,0)=1 AND status_name=@1", _dataContext.OrganizationId, statusName).id;
            }

        }

        public approval SetApproval<TEntity>(String recordId)
        {
            var result = new approval();
            var hasComplete = false;
            try
            {
                if (typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
                {
                    var appEntity = Activator.CreateInstance<TEntity>();
                    var template = GetTemplate<TEntity>();
                    var templateDetail = GetTemplateDetail<TEntity>(template.id);
                    using (var scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        using (var db = PhoenixERPRepo.GetInstance())
                        {
                            try
                            {
                                var oldRecord = approval.FirstOrDefault("WHERE application_entity_id=@0 AND record_id=@1 AND COALESCE(is_deleted,0)=0 ",
                                                ((IEntity)appEntity).GetEntityId(), recordId);
                                if (oldRecord != null) throw new ArgumentException($"This record has been set approval");

                                var record = new approval();
                                record.application_entity_id = ((IEntity)appEntity).GetEntityId();
                                record.record_id = recordId;
                                hasComplete = _dataContext.SaveEntity<approval>(record, true);
                                if (!hasComplete)
                                    throw new Exception("Failed assign approval template");

                                foreach (var d in templateDetail)
                                {
                                    var approverRecord = new approval_detail();
                                    approverRecord.approval_id = record.id;
                                    approverRecord.approver_id = d.approver_id;
                                    approverRecord.approval_level = d.approval_level;
                                    approverRecord.approval_status_id = Status.GetRequestId;
                                    approverRecord.is_auto_approved = false;
                                    approverRecord.is_active = true;
                                    hasComplete &= _dataContext.SaveEntity<approval_detail>(approverRecord, true);
                                }

                                if (hasComplete)
                                {
                                    scope.Complete();
                                }
                            }
                            catch
                            {
                                appLogger.Debug(db.LastCommand);
                                throw;
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException($"{typeof(TEntity).Name} is not entity");
                }
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

#endregion


namespace BusinessLogic
{
    public partial class DataContext
    {
        public ApprovalExtension Approval
        {
            get
            {
                return new ApprovalExtension(this);
            }
        }

    }
}
