using BusinessLogic.Services.General;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using NLog;
using NPoco;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Transactions;

namespace BusinessLogic.Core
{
    public partial class TeamMember : TeamMemberService
    {
        private readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public TeamMember(DataContext AppUserData) : base(AppUserData)
        {
        }

        public DataTablesResponseNetCore GetListByTeamId(IDataTablesRequest DTRequest, string TeamId)
        {
            DataTablesResponseNetCore result = null;
            string OptionalFilters = string.Empty;
            string OptionalParameters = string.Empty;
            string OptionalQueryBuilder = string.Empty;

            try
            {
                OptionalFilters = " r.team_id = @0 ";
                OptionalParameters = TeamId;
                result = Context.GetListDataTablesMarvelBinder<vw_team_member>(DTRequest, 
                    OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public DataTablesResponseNetCore GetAssignableTeamMember(IDataTablesRequest DTRequest, String TeamId)
        {
            DataTablesResponseNetCore result = null;
            string OptionalFilters = string.Empty;
            string OptionalParameters = string.Empty;
            string OptionalQueryBuilder = string.Empty;

            try
            {
                OptionalFilters = " r.id NOT IN ( SELECT m.application_user_id FROM team_member m WHERE m.team_id = @0 ) ";
                OptionalParameters = TeamId;
                result = Context.GetListDataTablesMarvelBinder<vw_application_user>(DTRequest, 
                    OptionalFilters, OptionalParameters, OptionalQueryBuilder);
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }

            return result;
        }

        public dynamic AssignMember(string[] Ids, string TeamId, ref bool isComplete)
        {
            dynamic result = new ExpandoObject();
            var log = "";

            using (var scope = new TransactionScope())
            {
                try
                {
                    var total = Ids.Length;
                    int success = 0, fail = 0;
                    
                    foreach (var id in Ids)
                    {
                        bool status = false;
                        var record = new team_member();
                        record.application_user_id = id;
                        record.team_id = TeamId;

                        status = Context.SaveEntity<team_member>(record, true);
                        if (status) success++;
                        else fail++;
                    }

                    if (fail == 0)
                    {
                        scope.Complete();
                        isComplete = true;
                    }

                    result.Success = success;
                    result.Fail = fail;

                    log = $"Assigned Member to Team {TeamId}, Success:{success} | Failed:{fail}";
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

        public dynamic Lookup(string Id = null, int page = 1, int length = 100, 
            string optionalFilters = null, string optionalParameters = null)
        {
            dynamic result = null;

            try
            {
                appLogger.Trace("Enter TeamMember.Lookup ...");

                if (string.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";

                    #region Use filters

                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in team_member.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(team_member.DefaultView);
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

                            sql.Append(" ORDER BY team_member_name ");
                            result = db.Page<vw_team_member>(page, rowPerPage, sql);
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
                            var sql = Sql.Builder.Append(team_member.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" ORDER BY r.team_name ");
                            result = db.Page<vw_team_member>(1, rowPerPage, sql);
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