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

namespace BusinessLogic.Core
{
    public partial class BusinessUnit : BusinessUnitService
    {
        private static readonly Logger appLogger = LogManager.GetCurrentClassLogger();

        public BusinessUnit(DataContext AppUserData) : base(AppUserData)
        {
        }

        public override bool Delete(string[] Ids, Action<string> ActionContinuation = null)
        {
            bool result = false;
            var log = "";
            int DeletedRecords = 0;
            int Failed = 0;

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
                                result = Context.DeleteEntity<business_unit>(id);
                                if (result)
                                {
                                    Team _teamservice = new Team(Context);
                                    var defaultTeam = db.FirstOrDefault<business_unit>(" WHERE id=@0 ", id).default_team;
                                    result = _teamservice.Delete(defaultTeam);
                                    DeletedRecords++;
                                }
                                else
                                    Failed++;

                            }
                            if (Failed > 0)
                            {
                                log = $"Canceled deleting {Failed} from {total} records";
                                ActionContinuation?.Invoke(log);
                                appLogger.Debug(log);
                                result = false;
                            }
                            else
                            {
                                log = $"Successfully deleted {DeletedRecords} from {total} records";
                                ActionContinuation?.Invoke(log);
                                appLogger.Debug(log);
                                scope.Complete();
                                result = true;
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

        public override bool SaveEntity(business_unitDTO RecordDto, ref bool isNew)
        {
            bool result = false;

            if (RecordDto == null) return result;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var db = new PhoenixERPRepo())
                {
                    try
                    {
                        var qry = string.Empty;
                        var recordTeam = new team();
                        if (String.IsNullOrEmpty(RecordDto.id))
                        {
                            //CHECK TEAM EXIST
                            recordTeam = team.FirstOrDefault(" WHERE team_name=@0 and organization_id=@1",
                                RecordDto.unit_name, Context.OrganizationId);

                            if (recordTeam == null)
                            {
                                recordTeam = new team();
                                recordTeam.id = Guid.NewGuid().ToString();
                                recordTeam.team_name = RecordDto.unit_name;
                                recordTeam.team_leader = null; // buatkan function untuk get user yang status nya is_administrator
                                recordTeam.organization_id = Context.OrganizationId;
                                result = Context.SaveEntity<team>(recordTeam, true);
                            }
                            else
                            {
                                //Random Team Base32
                                var gen = CommonTool.Helper.RandomBase32.Generate(6);

                                //Get Parent unit name
                                qry = string.Empty;
                                qry = business_unit.DefaultView;
                                qry += " WHERE r.id=@0 AND r.organization_id=@1 ";
                                Sql sql = Sql.Builder.Append(qry, RecordDto.parent_unit, Context.OrganizationId);

                                var parentBu = db.FirstOrDefault<vw_business_unit>(sql);
                                recordTeam = new team();
                                recordTeam.id = Guid.NewGuid().ToString();
                                var teamNameGen = (parentBu != null ?
                                    string.Format("{0}.{1}.{2}", parentBu.unit_name, RecordDto.unit_name, gen)
                                    : string.Format("{0}.{1}", RecordDto.unit_name, gen));

                                recordTeam.team_name = teamNameGen;
                                recordTeam.team_leader = null; // buatkan function untuk get user yang status nya is_administrator
                                recordTeam.organization_id = Context.OrganizationId;
                                result = Context.SaveEntity<team>(recordTeam, true);
                            }

                            if (!result) throw new Exception("Aborted, business unit on creating a team");

                            RecordDto.default_team = recordTeam.id;
                            if (string.IsNullOrEmpty(RecordDto.parent_unit))
                            {
                                RecordDto.parent_unit = null;
                            }
                            RecordDto.organization_id = Context.OrganizationId;
                        }

                        result = Svc.SaveEntity(RecordDto, ref isNew);
                        if (!result) throw new Exception("Aborted, Business Unit on saving");

                        scope.Complete();

                        //var recordTeam = new team();
                        //CHECK TEAM EXIST
                        //recordTeam = team.FirstOrDefault(" WHERE team_name=@0 and organization_id=@1", 
                        //    RecordDto.unit_name, Context.OrganizationId);

                        //recordTeam = team.FirstOrDefault(" WHERE id=@0 and organization_id=@1",
                        //      RecordDto.default_team, Context.OrganizationId);

                        //recordBusinessUnit = business_unit.FirstOrDefault(" WHERE unit_name=@0 ", RecordDto.unit_name);

                        //if (recordTeam == null)
                        //{
                        //    isNewRecord = true;
                        //    recordTeam = new team();
                        //    recordTeam.id = Guid.NewGuid().ToString();
                        //    recordTeam.team_name = RecordDto.unit_name;
                        //    recordTeam.team_leader = null; // buatkan function untuk get user yang status nya is_administrator
                        //    recordTeam.organization_id = Context.OrganizationId;
                        //    teamValidate = Context.SaveEntity<team>(recordTeam, isNewRecord);
                        //}
                        //else
                        //{
                        //    //check team has used in business unit

                        //    qry = string.Empty;
                        //    qry = business_unit.DefaultView;
                        //    qry += " WHERE r.default_team=@0 AND r.organization_id=@1 ";
                        //    qry = String.Format("SELECT COUNT(*) FROM ( {0} ) AS dt", qry);
                        //    var countUsedTeamInBusinessUnit = db.ExecuteScalar<int>(qry, recordTeam.id, Context.OrganizationId);
                        //    if (countUsedTeamInBusinessUnit > 0)
                        //    {
                        //        //Random Team Base32
                        //        var gen = CommonTool.Helper.RandomBase32.Generate(6);

                        //        //Get Parent unit name
                        //        qry = string.Empty;
                        //        qry = business_unit.DefaultView;
                        //        qry += " WHERE r.id=@0 AND r.organization_id=@1 ";
                        //        Sql sql = Sql.Builder.Append(qry, RecordDto.parent_unit, Context.OrganizationId);

                        //        var parentBu = db.FirstOrDefault<vw_business_unit>(sql);
                        //        isNewRecord = true;
                        //        recordTeam = new team();
                        //        recordTeam.id = Guid.NewGuid().ToString();
                        //        var teamNameGen = (parentBu != null ?
                        //            string.Format("{0}.{1}.{2}", parentBu.unit_name, RecordDto.unit_name, gen)
                        //            : string.Format("{0}.{1}", RecordDto.unit_name, gen));

                        //        recordTeam.team_name = teamNameGen;
                        //        recordTeam.team_leader = null; // buatkan function untuk get user yang status nya is_administrator
                        //        recordTeam.organization_id = Context.OrganizationId;
                        //        appLogger.Debug(db.LastCommand);

                        //        teamValidate = Context.SaveEntity<team>(recordTeam, isNewRecord);
                        //    }
                        //}

                        ////CHECK Business Unit EXIST
                        //if (teamValidate)
                        //{
                        //    RecordDto.default_team = recordTeam.id;
                        //    RecordDto.organization_id = Context.OrganizationId;
                        //    if (string.IsNullOrEmpty(RecordDto.parent_unit))
                        //    {
                        //        RecordDto.parent_unit = null;
                        //    }
                        //    result = Svc.SaveEntity(RecordDto, ref isNew);
                        //    if (result)
                        //    {
                        //        scope.Complete();
                        //    }
                        //}
                        //else
                        //{
                        //    throw new ArgumentException("Failed validate default team of business unit ");
                        //}
                    }
                    catch (Exception ex)
                    {
                        appLogger.Error(ex);
                        appLogger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(RecordDto));
                        appLogger.Debug(db.LastCommand);
                        throw;
                    }
                }
            }

            return result;
        }

        public dynamic Lookup(String Id = null, int page = 1, int length = 100, String optionalFilters = null, String optionalParameters = null)
        {
            dynamic result = null;
            try
            {
                if (String.IsNullOrEmpty(Id))
                {
                    var filters = "";
                    var parameters = "";
                    #region Use filters
                    if (!string.IsNullOrEmpty(optionalFilters))
                    {
                        foreach (var col in business_unit.DefaultViewColumns)
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
                            var sql = Sql.Builder.Append(business_unit.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 AND r.organization_id=@0 ", Context.OrganizationId);
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

                            sql.Append(" ORDER BY r.unit_name ");
                            result = db.Page<vw_business_unit>(page, rowPerPage, sql);

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
                            var sql = Sql.Builder.Append(business_unit.DefaultView);
                            sql.Append(" WHERE r.is_active = 1 ");
                            sql.Append(" AND r.id = @0 ", Id);
                            sql.Append(" AND r.organization_id = @0 ", Context.OrganizationId);
                            sql.Append(" ORDER BY r.unit_name ");
                            result = db.Page<vw_business_unit>(1, rowPerPage, sql);

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