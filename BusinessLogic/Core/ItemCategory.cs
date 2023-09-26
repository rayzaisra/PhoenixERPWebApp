using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;
using NPoco;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace BusinessLogic.Core
{
    public partial class ItemCategory : ItemCategoryService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ItemCategory(DataContext AppUserData) : base(AppUserData)
        {

        }

        public override bool SaveEntity(item_categoryDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = Context.OrganizationId;
                appLogger.Info(JsonConvert.SerializeObject(RecordDto));
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public override vw_item_categoryDTO Details(string Id)
        {
            vw_item_categoryDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var sql = Sql.Builder.Append(item_category.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_item_category>(sql);
                    if (view != null)
                    {
                        result = (vw_item_categoryDTO)((new vw_item_categoryDTO()).InjectFrom(view));
                    }
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
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
                        foreach (var col in item_category.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(item_category.DefaultView);
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

                            sql.Append(" ORDER BY r.category_name ");
                            result = db.Page<vw_item_category>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(item_category.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.category_name ");

                            result = db.Page<vw_item_category>(1, rowPerPage, sql);
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
                var sql = Sql.Builder.Append(item_category.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_item_category>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }

                #endregion
                sql.Append(" ORDER BY r.category_name");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_item_category>(pageDraw, rowPerPage, sql);
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
