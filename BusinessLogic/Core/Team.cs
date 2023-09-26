using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using NLog;
using NPoco;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace BusinessLogic.Core
{
    public partial class Team : TeamService
    {
        private readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public Team(DataContext AppUserData) : base(AppUserData)
        {
        }

        public override bool SaveEntity(teamDTO RecordDto, ref bool isNew)
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

        public override vw_teamDTO Details(string Id)
        {
            vw_teamDTO result = null;

            try
            {
                using (var db = new PhoenixERPRepo())
                {
                    var qry = "";
                    qry = team.DefaultView;
                    qry += " WHERE r.id IS NOT NULL  ";
                    qry += " AND r.id=@0";
                    var view = db.FirstOrDefault<vw_team>(qry, Id);
                    if (view != null)
                    {
                        result = (vw_teamDTO)((new vw_teamDTO()).InjectFrom(view));
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

        public dynamic Lookup(String Id = null, int page = 1, int length = 100, String optionalFilters = null, String optionalParameters = null)
        {
            dynamic result = null;
            try
            {
                appLogger.Debug("MASUK BU LOOKUP");
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";
                    #region Use filters
                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in team.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(team.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            if (!String.IsNullOrEmpty(filters))
                            {
                                if (String.IsNullOrEmpty(parameters))
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters));
                                }
                                else
                                {
                                    sql.Append(String.Format(" AND ( {0} ) ", filters),
                                        parameters.Split(",".ToCharArray()));
                                }
                            }

                            sql.Append(" ORDER BY r.team_name ");
                            result = db.Page<vw_team>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(team.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.team_name ");
                            result = db.Page<vw_team>(1, rowPerPage, sql);

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


        public override bool Delete(string Id)
        {
            bool result = false;
            try
            {
                result = this.Delete(new string[] { Id });
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
            bool result = false;
            var log = "";
            int DeletedRecords = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
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
                                result = Context.DeleteEntity<team>(id);
                                if (result)
                                {
                                    var member = db.Fetch<team_member>(" WHERE team_id=@0 ", id);
                                    if (member.Count > 0)
                                    {
                                        foreach (var m in member)
                                        {
                                            result = Context.DeleteEntity<team_member>(m.id);
                                        }
                                    }
                                    //var delete = db.UpdateMany<team_member>().Where(x=>x.team_id==id)
                                    //    .OnlyFields(o=>new {o.is_active,o.is_deleted,o.deleted_by,o.dele });
                                    DeletedRecords++;
                                }

                            }

                            scope.Complete();

                            log = $"Successfully deleted {DeletedRecords} from {total} records";
                            ActionContinuation?.Invoke(log);
                            appLogger.Debug(log);
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


    }
}
