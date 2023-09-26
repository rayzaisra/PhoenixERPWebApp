using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;
using NPoco;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Core
{
    public partial class Menu : ApplicationMenuService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Menu(DataContext AppUserData) : base(AppUserData)
        {
        }

        public override bool SaveEntity(application_menuDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            try
            {
                RecordDto.parent_menu_id = (string.IsNullOrEmpty(RecordDto.parent_menu_id) ? null : RecordDto.parent_menu_id);
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

        public override vw_application_menuDTO Details(string Id)
        {
            vw_application_menuDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var sql = Sql.Builder.Append(application_menu.DefaultView);
                    sql.Append(" WHERE r.is_active = 1 ");
                    sql.Append(" AND r.id = @0 ", Id);

                    var view = db.FirstOrDefault<vw_application_menu>(sql);
                    if (view != null)
                    {
                        result = (vw_application_menuDTO)((new vw_application_menuDTO()).InjectFrom(view));
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
