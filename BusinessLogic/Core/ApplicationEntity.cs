using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PhoenixERP.General;
using System.Dynamic;
using NPoco;
using NLog;
using DataAccess.PhoenixERP;
using DataAccess;
using CommonTool;
using BusinessLogic.Services.General;
using CommonTool.KendoUI.Grid;
namespace BusinessLogic.Core
{
    public partial class ApplicationEntity : ApplicationEntityService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ApplicationEntity(DataContext AppUserData) : base(AppUserData)
        {
        }

        public DataResponse GetListKendoUIGrid(DataRequest DTRequest)
        {
            DataResponse result = null;
            try
            {
                result = Context.GetListKendoUIGrid<vw_application_entity>(DTRequest);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic RegisterAllEntity()
        {
            dynamic results = new ExpandoObject();
            int New = 0, Update = 0, Fail = 0;
           
            using (var db = new PhoenixERPRepo())
            {
                using (var scope = db.GetTransaction())
                {
                    try
                    {
                        var qry = @"SELECT TABLE_CATALOG,TABLE_SCHEMA,TABLE_NAME,TABLE_TYPE
		                    FROM  INFORMATION_SCHEMA.TABLES
		                    WHERE TABLE_TYPE='BASE TABLE'
		                    ORDER BY TABLE_NAME ";

                        var data = db.Fetch<dynamic>(qry);
                        foreach (var i in data)
                        {
                            try
                            {
                                var isNew = false;
                                var id = GUIDHash.ConvertToMd5HashGUID(
                                    i.TABLE_SCHEMA + "." + i.TABLE_NAME).ToString();
                                var record = db.FirstOrDefault<application_entity>("WHERE id = @0", id);
                                if(record == null)
                                {
                                    record = new application_entity();
                                    record.id = id;
                                    isNew = true;
                                }

                                record.organization_id = Context.OrganizationId;
                                record.entity_name = i.TABLE_NAME;
                                record.display_name = i.TABLE_NAME;

                                if (isNew)
                                {
                                    db.Insert(record);
                                    New++;
                                }
                                else
                                {
                                    db.Update(record);
                                    Update++;
                                }
                            }
                            catch (Exception ex)
                            {
                                appLogger.Error(ex);
                                Fail++;
                                continue;
                            }
                        }

                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        throw;
                    }
                }
            }

            results.New = New;
            results.Update = Update;
            results.Fail = Fail;

            return results;
        }

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(application_entity.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_application_entity>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }
                #endregion
                sql.Append(" ORDER BY r.entity_name ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        result = db.Page<vw_application_entity>(pageDraw, rowPerPage, sql);
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
