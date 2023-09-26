using BusinessLogic.Services.Finance;
using DataAccess.PhoenixERP.Finance;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Omu.ValueInjecter;
using DataAccess.PhoenixERP;
using NPoco;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System.Linq;
using DataAccess.PhoenixERP.General;
using DataAccess.Dto.Custom.Finance;

namespace BusinessLogic.Finance
{
    public partial class CashAdvanceStatus : CashAdvanceStatusService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public CashAdvanceStatus(DataContext AppUserData) : base(AppUserData)
        {
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var org = db.FirstOrDefault<organization>("WHERE id = @0", Context.OrganizationId);
                    appLogger.Debug(db.LastCommand);

                    foreach (var val in cash_advance_status.DefaultValues)
                    {
                        var sql = Sql.Builder.Append("WHERE organization_id = @0", org.id);
                        sql.Append("AND status_name = @0", val.Key);
                        sql.Append("AND is_deleted IS NULL");

                        var defaultRecord = db.FirstOrDefault<cash_advance_status>(sql);
                        appLogger.Debug(db.LastCommand);

                        if (org != null && defaultRecord == null)
                        {
                            defaultRecord = new cash_advance_status()
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

                            db.Insert(defaultRecord);
                            appLogger.Debug(db.LastCommand);
                        }
                    }
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex.ToString());
                    throw;
                }
            }
        }        

        public cash_advance_statusDTO GetFirstStatus()
        {
            cash_advance_statusDTO result = null;

            using(var db = new PhoenixERPRepo())
            {
                try
                {
                    var record = db.FirstOrDefault<cash_advance_status>(
                        "WHERE status_name = @0 AND is_deleted IS NULL", 
                        cash_advance_status.REQUESTED);
                    appLogger.Debug(db.LastCommand);

                    if (record != null)
                    {
                        result = (cash_advance_statusDTO)(new cash_advance_statusDTO()).InjectFrom(record);
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

        public override bool SaveEntity(cash_advance_statusDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = Context.OrganizationId;
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
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
                        foreach (var col in cash_advance_status.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(cash_advance_status.DefaultView);
                            sql.Append(" WHERE r.organization_id = @0 ", Context.OrganizationId);
                            sql.Append(" AND COALESCE(r.is_deleted, 0) = 0 ");
                            sql.Append(" AND COALESCE(r.is_active, 0) = 1 ");

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

                            sql.Append(" ORDER BY r.status_name ");
                            result = db.Page<vw_cash_advance_status>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(cash_advance_status.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.status_name ");
                            result = db.Page<vw_cash_advance_status>(1, rowPerPage, sql);

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
