using System;
using DataAccess.PhoenixERP;
using NLog;
using System.Transactions;
using System.Dynamic;
using System.Linq;
using BusinessLogic.Services.General;
using NPoco;
using DataAccess.PhoenixERP.General;
using Omu.ValueInjecter;

namespace BusinessLogic.Core
{
    public partial class ApplicationMenuCategory : ApplicationMenuCategoryService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();
        
        public ApplicationMenuCategory(DataContext AppUserData) : base(AppUserData)
        {            
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
                        foreach (var col in application_menu_category.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(application_menu_category.DefaultView);
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
                            result = db.Page<vw_application_menu_category>(Page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(application_menu_category.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.category_name ");

                            result = db.Page<vw_application_menu_category>(1, rowPerPage, sql);
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

        public override bool SaveEntity(application_menu_categoryDTO RecordDto, ref bool isNew)
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

        public override vw_application_menu_categoryDTO Details(string Id)
        {
            vw_application_menu_categoryDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var sql = Sql.Builder.Append(application_menu_category.DefaultView);
                        sql.Append(" WHERE r.is_active = 1 ");
                        sql.Append(" AND r.id = @0 ", Id);

                        var view = db.FirstOrDefault<vw_application_menu_category>(sql);
                        if (view != null)
                        {
                            result = (vw_application_menu_categoryDTO)((new vw_application_menu_categoryDTO()).InjectFrom(view));
                        }
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        appLogger.Debug(db.LastCommand);
                        throw;
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
    }
}