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
using Newtonsoft.Json;

namespace BusinessLogic.Finance
{
    public partial class BankPaymentDetail : BankPaymentDetailService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public BankPaymentDetail(DataContext AppUserData) : base(AppUserData)
        {
        }

        public override bool SaveEntity(bank_payment_detailDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using(var db = new PhoenixERPRepo())
            {
                try
                {
                    var sql = Sql.Builder.Append("SELECT r.id, r.account_code,");
                    sql.Append("bp.legal_entity_id AS le1, r.legal_entity_id AS le2");
                    sql.Append("FROM fn.account r");
                    sql.Append("INNER JOIN fn.bank_payment_detail bpd ON bpd.account_id = r.id");
                    sql.Append("INNER JOIN fn.bank_payment bp ON bp.id = bpd.bank_payment_id");
                    dynamic acc = db.FirstOrDefault<dynamic>(sql);
                    appLogger.Debug(db.LastCommand);

                    if (acc != null)
                    {
                        if(acc.le1 != acc.le2)
                        {
                            appLogger.Debug(JsonConvert.SerializeObject(acc));
                            throw new Exception(string.Format("Account id {0} belongs to different legal entity",
                                RecordDto.account_id));
                        }

                        result = Svc.SaveEntity(RecordDto, ref isNew);
                    }
                    else
                    {
                        throw new Exception(string.Format("Account id {0} does not exist", 
                            RecordDto.account_id));
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
                        foreach (var col in bank_payment_detail.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(bank_payment_detail.DefaultView);
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

                            sql.Append(" ORDER BY bp.payment_number ");
                            result = db.Page<vw_bank_payment_detail>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(bank_payment_detail.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY bp.payment_number ");
                            result = db.Page<vw_bank_payment_detail>(1, rowPerPage, sql);
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
