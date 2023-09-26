using System;
using DataAccess;
using DataAccess.PhoenixERP;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Omu.ValueInjecter;
using BusinessLogic;
using System.Transactions;
using Newtonsoft.Json;

namespace BusinessLogic.Services
{
    public partial class Service<T1, T2, T3>
    {
        protected DataContext context;

        protected NLog.Logger appLogger = NLog.LogManager.GetCurrentClassLogger();

        public Service(DataContext AppUserData)
        {
            context = AppUserData;
        }

        public virtual T1 Detail(string Id)
        {
            T1 result = default(T1);

            try
            {
                result = (T1)context.GetEntityById<T1>(Id);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public virtual T2 Details(string Id)
        {
            T2 result = default(T2);

            try
            {
                var record = context.GetEntityDetailsById<T2>(Id);
                if (record != null)
                {
                    result = (T2)(Activator.CreateInstance<T2>().InjectFrom(record));
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public virtual bool Delete(string Id)
        {
            string log = null;
            bool result = false;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    if (string.IsNullOrEmpty(Id))
                        return result;

                    result = context.DeleteEntity<T1>(Id);

                    scope.Complete();

                    log = $"Successfully deleted record Id = {Id}";
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public virtual bool Delete(string[] Ids, ref int DeletedRecords)
        {
            var log = "";
            bool result = false;
            DeletedRecords = 0;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    if (Ids == null || Ids.Length < 1)
                        return result;

                    var total = Ids.Length;
                    foreach (var id in Ids)
                    {
                        result = context.DeleteEntity<T1>(id);
                        if (result) DeletedRecords++;
                    }

                    scope.Complete();

                    log = $"Successfully deleted {DeletedRecords} from {total} records";
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public virtual bool Delete(string[] Ids, Action<string> ActionContinuation = null)
        {
            var log = "";
            bool result = false;
            int success = 0;

            appLogger.Debug("Delete " + JsonConvert.SerializeObject(Ids));

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    if (Ids == null || Ids.Length < 1)
                    {
                        appLogger.Debug("Delete parameter is empty");
                        return result;
                    }
                        

                    var total = Ids.Length;
                    foreach (var id in Ids)
                    {
                        result = context.DeleteEntity<T1>(id);
                        if (result) success++;                        
                    }

                    scope.Complete();

                    log = $"Successfully deleted {success} from {total} records";
                    ActionContinuation?.Invoke(log);
                    appLogger.Debug(log);
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                    throw;
                }
            }

            return result;
        }

        public virtual DataTablesResponseNetCore GetListDataTables(IDataTablesRequest dataTablesRequest)
        {
            try
            {
                return context.GetListDataTablesMarvelBinder<T2>(dataTablesRequest);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
        }

        public virtual bool SaveEntity(T3 RecordDto, ref bool isNew)
        {
            bool success = false;

            try
            {
                T1 record = Activator.CreateInstance<T1>();
                record.InjectFrom(RecordDto);

                T1 recordEntity = (T1)(((IBaseRecord)record).MapToEntity<T1>(ref isNew));
                if (recordEntity != null)
                {
                    if (isNew)
                    {
                        success = context.SaveEntity<T1>(recordEntity, isNew);
                    }
                    else
                    {
                        if (!context.IsSystemAdministrator)
                        {
                            if (((IBaseRecord)recordEntity).is_default ?? false)
                            {
                                throw new Exception("Default record is not editable");
                            }
                            else if (((IBaseRecord)recordEntity).is_locked ?? false)
                            {
                                throw new Exception("Locked record is not editable");
                            }
                        }

                        success = context.SaveEntity<T1>(recordEntity, isNew);
                    }
                }
                RecordDto.InjectFrom(recordEntity);

            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            

            return success;
        }
    }
}
