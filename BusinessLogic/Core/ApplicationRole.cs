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
using Newtonsoft.Json;
namespace BusinessLogic.Core
{
    public partial class ApplicationRole : ApplicationRoleService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public ApplicationRole(DataContext AppUserData) : base(AppUserData) { }

        public override bool Delete(string Id)
        {
            bool result = false;
            using (var db = new PhoenixERPRepo())
                try
                {
                    result = (db.Delete<application_role>(" WHERE id=@0", Id) > 0);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }

            return result;
        }

        public override bool Delete(string[] Ids, Action<string> ActionContinuation = null)
        {
            var log = string.Empty;
            bool result = true;
            var success = 0;
            var total = Ids.Length;
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
                try
                {
                    foreach (var id in Ids)
                    {
                        var record = application_role.FirstOrDefault(" WHERE id=@0", id);
                        if (record == null) continue;
                        appLogger.Debug("PROCESS PERMANENT DELETE APPLICATION ROLE");
                        appLogger.Debug(JsonConvert.SerializeObject(record));
                        result &= Delete(record.id);
                        if (result)
                        {
                            appLogger.Debug($"SUCCESS DELETE PERMANENT APPLICATION ROLE ID {id}");
                            success++;
                        }
                    }
                    log = $"Successfully deleted {success} from {total} records";
                    ActionContinuation?.Invoke(log);
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

            return result;
        }

        public override bool SaveEntity(application_roleDTO RecordDto, ref bool isNew)
        {
            bool result = false;
            if (RecordDto == null) return result;

            try
            {
                RecordDto.organization_id = Context.OrganizationId;
                result = Svc.SaveEntity(RecordDto, ref isNew);
                if (result)
                {
                    var roleAccessService = new RoleAccess(Context);
                    appLogger.Debug("ID ROLE : " + RecordDto.id);
                    result &= roleAccessService.RegisterRoleAccessEntity(RecordDto.id);
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
