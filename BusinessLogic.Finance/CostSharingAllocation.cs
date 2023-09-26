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

namespace BusinessLogic.Services
{
    public partial class CostSharingAllocation : CostSharingAllocationService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public CostSharingAllocation(DataContext AppUserData) : base(AppUserData)
        {

        }

        public bool SetNotActive(string Id)
        {
            bool result = false;
            try
            {
                result = this.SetNotActive(new string[] { Id });
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public bool SetNotActive(string[] Ids, Action<string> ActionContinuation = null)
        {
            bool result = false;
            var log = "";
            int DeletedRecords = 0;
            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var db = new PhoenixERPRepo())
                    {
                        try
                        {
                            if (Ids == null || Ids.Length < 1)
                                return result;

                            var total = Ids.Length;
                            foreach (var id in Ids)
                            {
                                var record = cost_sharing_allocation.FirstOrDefault(" WHERE id=@0 ", id);
                                if (record == null) throw new ArgumentException("Record not found !");
                                if (!record.is_active ?? false) {
                                    scope.Complete();
                                    return true;
                                }
                                //throw new ArgumentException("This current Cost Sharing has been removed or disabled by User ");

                                record.is_active = false;
                                result = Context.SaveEntity<cost_sharing_allocation>(record,false);
                                if (result)
                                {
                                    var detail = db.Fetch<cost_sharing_allocation_detail>(" WHERE cost_sharing_allocation_id=@0 ", id);
                                    if (detail.Count > 0)
                                    {
                                        foreach (var m in detail)
                                        {
                                            m.is_active = false;
                                            result &= Context.SaveEntity<cost_sharing_allocation_detail>(m,false);
                                        }
                                    }
                                    DeletedRecords++;
                                }

                            }
                            if (result)
                            {
                                scope.Complete();
                                log = $"Successfully not active cost sharing detail {DeletedRecords} from {total} records";
                                ActionContinuation?.Invoke(log);
                                appLogger.Debug(log);
                            }

                        }
                        catch (Exception ex)
                        {
                            appLogger.Error(ex);
                            throw;
                        }
                        
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



        public override bool SaveEntity(cost_sharing_allocationDTO RecordDto, ref bool isNew)
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
                        foreach (var col in cost_sharing_allocation.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(cost_sharing_allocation.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 AND r.organization_id =@0 ",Context.OrganizationId);
                            if (!String.IsNullOrEmpty(filters))
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

                            sql.Append(" ORDER BY r.cost_sharing_allocation_name ");
                            result = db.Page<vw_cost_sharing_allocation>(Page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(cost_sharing_allocation.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" AND r.organization_id = @0 ", Context.OrganizationId);
                            sql.Append(" ORDER BY r.cost_sharing_allocation_name ");
                            result = db.Page<vw_cost_sharing_allocation>(1, rowPerPage, sql);

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
