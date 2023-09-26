using BusinessLogic.Services.General;
using CommonTool;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;
using NPoco;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace BusinessLogic.Core
{
    public partial class ItemType : ItemTypeService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ItemType(DataContext AppUserData) : base(AppUserData){}

        public override bool SaveEntity(item_typeDTO RecordDto, ref bool isNew)
        {
            bool result = false;
            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = Context.OrganizationId;
                RecordDto.id = GUIDHash.ConvertToMd5HashGUID(RecordDto.type_name.ToUpper()).ToString();
                result = Svc.SaveEntity(RecordDto, ref isNew);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public override vw_item_typeDTO Details(string Id)
        {
            vw_item_typeDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var sql = Sql.Builder.Append(item.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_item>(sql);
                    if (view != null)
                    {
                        result = (vw_item_typeDTO)((new vw_item_typeDTO()).InjectFrom(view));
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

        public dynamic kendoLookup(CommonTool.KendoUI.Combobox.ComboboxDataRequest dtRequest)
        {
            dynamic result = null;
            if (dtRequest != null)
            {
                #region Use filters
                var sql = Sql.Builder.Append(item_type.DefaultView);
                sql.Append(" WHERE r.is_active = 1 ");
                if (dtRequest.Filter2 != null)
                {
                    Sql filterBuilder = Context.MultiplekendoFilterable<vw_item_type>(dtRequest.Filter2);
                    if (filterBuilder != null)
                        sql.Append(filterBuilder.SQL, filterBuilder.Arguments);
                }

                #endregion
                sql.Append(" ORDER BY r.type_name ");
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        int pageDraw = dtRequest.Page;
                        int rowPerPage = dtRequest.PageSize;
                        var data = db.Page<vw_item_typeDTO>(pageDraw, rowPerPage, sql);
                        TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                        data.Items.ForEach(x => x.type_name = ti.ToTitleCase(x.type_name));
                        result = data;
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
