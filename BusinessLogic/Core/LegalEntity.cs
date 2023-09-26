using System;
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
    public partial class LegalEntity : LegalEntityService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public LegalEntity(DataContext AppUserData) : base(AppUserData)
        {
        }

        public override bool SaveEntity(legal_entityDTO RecordDto, ref bool isNew)
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

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                //var searchValue = dtRequest.Filter.Filters[0].Value;
                var sql = Sql.Builder.Append(legal_entity.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_legal_entity>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.legal_entity_name ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_legal_entity>(pageDraw, rowPerPage, sql);
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

        public dynamic Lookup(string Id = null, int Page = 1, int Length = 100,
        string OptionalFilters = null, string OptionalParameters = null)
        {
            dynamic result = null;

            try
            {
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(OptionalFilters))
                    {
                        foreach (var col in legal_entity.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(legal_entity.DefaultView);
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

                            sql.Append(" ORDER BY r.legal_entity_name ");
                            result = db.Page<vw_legal_entity>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(legal_entity.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.legal_entity_name ");

                            result = db.Page<vw_legal_entity>(1, rowPerPage, sql);
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
