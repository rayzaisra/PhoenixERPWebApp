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
    public partial class PaymentMethod : PaymentMethodService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public PaymentMethod(DataContext AppUserData) : base(AppUserData)
        {
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    //var org = db.FirstOrDefault<organization>("WHERE id = @0", Context.OrganizationId);

                    foreach (var val in payment_method.DefaultValues)
                    {
                        //var sql = Sql.Builder.Append("WHERE organization_id = @0", org.id);
                        var sql = Sql.Builder.Append("WHERE method_name = @0", val.Key);
                        sql.Append("AND is_deleted = 0");

                        var defaultRecord = db.FirstOrDefault<payment_method>(sql);
                        appLogger.Debug(db.LastCommand);
                        //if (org != null && defaultRecord == null)
                        if (defaultRecord == null)
                        {
                            defaultRecord = new payment_method()
                            {
                                id = Guid.NewGuid().ToString(),
                                created_by = "189f3ca3-83b0-41aa-8af6-9c0a346d087c",
                                created_on = DateTime.Now,
                                is_active = true,
                                is_locked = true,
                                is_default = true,
                                owner_id = "189f3ca3-83b0-41aa-8af6-9c0a346d087c",
                                organization_id = "9EE4834225E334380667DDCF2F60F6DA",
                                method_name = val.Key
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

        public override bool SaveEntity(payment_methodDTO RecordDto, ref bool isNew)
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
                        foreach (var col in payment_method.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(payment_method.DefaultView);
                            sql.Append("WHERE r.organization_id = @0", Context.OrganizationId);
                            sql.Append("AND COALESCE(r.is_deleted, 0) = 0");
                            sql.Append("AND COALESCE(r.is_active, 0) = 1");

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

                            sql.Append(" ORDER BY r.method_name ");
                            result = db.Page<vw_payment_method>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(payment_method.DefaultView);
                            sql.Append("WHERE COALESCE(r.is_active, 0) = 1");
                            sql.Append("AND COALESCE(r.is_deleted, 0) = 0");
                            sql.Append("AND r.id = @0 ", Id);
                            sql.Append("ORDER BY r.method_name");
                            result = db.Page<vw_payment_method>(1, rowPerPage, sql);

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

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(payment_method.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_payment_method>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.method_name ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_payment_method>(pageDraw, rowPerPage, sql);
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