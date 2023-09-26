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

namespace BusinessLogic.Services
{
    public partial class Voucher : VoucherService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Voucher(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(voucherDTO RecordDto, ref bool isNew)
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

        public bool SubmitVoucher(string VoucherId, ref bool isNew)
        {
            bool result = false;

            using (var scope = new TransactionScope())
            {
                try
                {
                    var record = voucher.FirstOrDefault(" WHERE id=@0 ", VoucherId);
                    if (record == null)
                        throw new ArgumentException("Record not found");

                    record.is_locked = true;
                    var RecordDto = new voucherDTO();
                    RecordDto.InjectFrom(record);
                    result = Svc.SaveEntity(RecordDto, ref isNew);
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
                        foreach (var col in voucher.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(voucher.DefaultView);
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
                            result = db.Page<vw_voucher>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(voucher.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.created_on DESC ");
                            result = db.Page<vw_voucher>(1, rowPerPage, sql);

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
