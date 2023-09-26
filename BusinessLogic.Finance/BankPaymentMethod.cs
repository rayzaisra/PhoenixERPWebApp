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
    public partial class BankPaymentMethod : BankPaymentMethodService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public BankPaymentMethod(DataContext AppUserData) : base(AppUserData)
        {
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var org = db.FirstOrDefault<organization>("WHERE id = @0", "9EE4834225E334380667DDCF2F60F6DA");

                    foreach (var val in bank_payment_method.DefaultValues)
                    {
                        var sql = Sql.Builder.Append("WHERE organization_id = @0", "9EE4834225E334380667DDCF2F60F6DA");
                        //var sql = Sql.Builder.Append("");
                        sql.Append("AND method_name = @0", val.Key);
                        sql.Append("AND is_deleted IS NULL");

                        var defaultRecord = db.FirstOrDefault<bank_payment_method>(sql);
                        appLogger.Debug(db.LastCommand);

                        if (org != null && defaultRecord == null)
                        {
                            defaultRecord = new bank_payment_method()
                            {
                                id = Guid.NewGuid().ToString(),
                                created_by = org.created_by,
                                created_on = DateTime.Now,
                                is_active = true,
                                is_locked = true,
                                is_default = true,
                                owner_id = org.owner_id,
                                organization_id = org.id,
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

        public override bool SaveEntity(bank_payment_methodDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = "9EE4834225E334380667DDCF2F60F6DA";
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
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
                        foreach (var col in bank_payment_method.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(bank_payment_method.DefaultView);
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

                            sql.Append(" ORDER BY r.method_name ");
                            result = db.Page<vw_bank_payment_method>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(bank_payment_method.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.method_name ");
                            result = db.Page<vw_bank_payment_method>(1, rowPerPage, sql);

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
