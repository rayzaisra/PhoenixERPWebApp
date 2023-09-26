using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Linq;
using BusinessLogic.Services.General;
using BusinessLogic.Services.Finance;
using NLog;
using DataAccess.PhoenixERP.General;
using DataAccess.PhoenixERP.Finance;
using DataAccess.PhoenixERP;
using NPoco;
using Phoenix.Shared.Core.Areas.General.Dtos;
using Microsoft.AspNetCore.Http;
using BusinessLogic.Core;
using System.Threading.Tasks;
using System.Transactions;
using CommonTool.KendoUI.Grid;

namespace BusinessLogic.Services
{
    public partial class PettyCash: PettyCashService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        private readonly FileConfig _fileconfig;
        private const String entityId = "9e027081-dd59-cf65-14ef-dbb0abb93ae9";

        public PettyCash(DataContext AppUserData, FileConfig settings) : base(AppUserData)
        {
            _fileconfig = settings;
        }

        public override bool SaveEntity(petty_cashDTO RecordDto, ref bool isNew)
        {
            bool result = false;
            if (RecordDto == null) return result;
            try
            {
                var _petty_cash = RecordDto;

                _petty_cash.settlement_number = string.Format("PC-{0}",
                            CommonTool.Helper.RandomString.Generate(5).ToUpper());
                result = Svc.SaveEntity(_petty_cash, ref isNew);
                
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            return result;
        }

        public bool SavePettyCash(PettyCashDetailsDTO RecordDto, IFormFileCollection files,ref bool isNew)
        {
            bool result = false;
            //bool isNew = false;
            if (RecordDto == null) return result;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var _petty_cash = RecordDto.PettyCash;
                    var details = RecordDto.Details;

                    _petty_cash.settlement_number = string.Format("PC-{0}",
                                CommonTool.Helper.RandomString.Generate(5).ToUpper());
                    result = Svc.SaveEntity(_petty_cash, ref isNew);
                    if (result)
                    {
                        var detailService = new PettyCashDetailService(Context);
                        var fileMaster = new FileMaster(Context, _fileconfig);
                        var record = new DamUpload();
                        
                        for (int i = 0; i < RecordDto.Details.Count; i++)
                        {
                            var isUpload = filemaster.Fetch(" WHERE record_id= @0 ", details[i].id);
                            if (files.Count > 0 && isUpload.Count == 0)
                            {
                                record.file = files[i];
                                result &= fileMaster.Save(record, entityId, files[i].Name).Result;
                            }
                            details[i].petty_cash_id = _petty_cash.id;
                            details[i].file_name = files[i].FileName;
                            result &= detailService.Svc.SaveEntity(details[i], ref isNew);

                        }
                        if (result)
                        {
                            scope.Complete();
                        }
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
                        foreach (var col in petty_cash.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(petty_cash.DefaultView);
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

                            sql.Append(" ORDER BY r.PettyCash_name ");
                            result = db.Page<vw_petty_cash>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(petty_cash.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.settlement_number ");
                            result = db.Page<vw_petty_cash>(1, rowPerPage, sql);

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
