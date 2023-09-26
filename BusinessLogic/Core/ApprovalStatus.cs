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

namespace BusinessLogic.Core
{
    public partial class ApprovalStatus : ApprovalStatusService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ApprovalStatus(DataContext AppUserData) : base(AppUserData)
        {
            using(var db = new PhoenixERPRepo())
            {
                try
                {
                    var org = db.FirstOrDefault<organization>("WHERE id = @0", Context.OrganizationId);

                    foreach(var val in approval_status.DefaultValues)
                    {
                        var sql = Sql.Builder.Append("WHERE organization_id = @0", org.id);                        
                        sql.Append("AND status_name = @0", val.Key);
                        sql.Append("AND is_deleted IS NULL");

                        var defaultStatus = db.FirstOrDefault<approval_status>(sql);
                        appLogger.Debug(db.LastCommand);

                        if (org != null && defaultStatus == null)
                        {
                            defaultStatus = new approval_status()
                            {
                                id = Guid.NewGuid().ToString(),
                                created_by = org.created_by,
                                created_on = DateTime.Now,
                                is_active = true,
                                is_locked = true,
                                is_default = true,
                                owner_id = org.owner_id,
                                organization_id = org.id,
                                status_name = val.Key,
                                status_level = val.Value 
                            };

                            db.Insert(defaultStatus);
                            appLogger.Debug(db.LastCommand);
                        }
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Debug(db.LastCommand);
                    appLogger.Error(ex.ToString());
                }
            }
        }

        public override bool SaveEntity(approval_statusDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = string.IsNullOrEmpty(RecordDto.organization_id) ?
                    Context.OrganizationId : RecordDto.organization_id;
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public approval_statusDTO GetFirstStatus()
        {
            approval_statusDTO result = null;

            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append("WHERE organization_id = @0", Context.OrganizationId);
                    sql.Append("AND status_name = @0", approval_status.REQUESTED);
                    sql.Append("AND is_deleted IS NULL");
                    var record = db.FirstOrDefault<approval_status>(sql);
                    appLogger.Debug(db.LastCommand);

                    if(record != null)
                    {
                        result = (approval_statusDTO)(new approval_statusDTO()).InjectFrom(record);
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Debug(db.LastCommand);
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

                            sql.Append(" ORDER BY r.status_name ");
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
                            sql.Append(" ORDER BY r.status_name ");

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
    }
}
