using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class NotificationService : INotificationService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public NotificationService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Notification entity)
        {
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params Notification[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Notification entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params Notification[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(Notification entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params Notification[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(Notification entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(Notification entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> Get() => await context.Notifications.AsNoTracking().ToListAsync();

        public Task<List<Notification>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<Notification> Get(string Id) => await context.Notifications.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<List<NotificationVm>> GetListNotificationByUserCode(string userCode)
        {
            var data = await (from x in context.Notifications.Where(c => c.UserId.Equals(userCode))
                              join t in context.NotificationTypes on x.NotificationTypeId equals t.Id
                              join u in context.EmployeeBasicInfos on x.CreatedBy equals u.Id
                              join y in context.TrTemplateApprovals on x.ItemId equals y.RefId
                              join z in context.TrUserApprovals on y.Id equals z.TrTempApprovalId
                              where z.StatusApproved != Models.Um.StatusApproved.Approved
                              select new NotificationVm
                              {
                                  //TemplateNotify = "<a href='javascript:directToDetailNotif(\"" + x.Id + "\",\"" + t.Href.Replace("{id}", x.ItemId).Replace("{jobVacancyId}", context.CandidateRecruitments.Where(m=>m.Id == x.ItemId).Select(m=>m.job_vacancy_id).FirstOrDefault()) + "\")' title='click here to see detail'><div><p>" + t.ShortText + "</p></div></a>",
                                  TemplateNotify = "<a href='javascript:directToDetailNotif(\"" + x.Id + "\",\"" + t.Href + "\")' title='click here to see detail'><div><p>" + t.ShortText + "</p></div></a>",
                                  CreatedOn = x.CreatedOn,
                                  Tipe = x.NotificationTypeId.Contains("approv") ? 2 : 1,
                                  CreatedBy = u.NameEmployee,
                                  Module = x.Module,
                                  IsSeen = x.IsSeen,
                                  ItemId = x.ItemId,
                              }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            data = data.GroupBy(p => p.TemplateNotify).Select(grp => grp.FirstOrDefault()).ToList();

            foreach (var item in data)
            {
                if (item.TemplateNotify.Contains("{id}"))
                {
                    item.TemplateNotify = item.TemplateNotify.Replace("{id}", item.ItemId);
                }

                if (item.TemplateNotify.Contains("{jobVacancyId}"))
                {
                    var jobVacancyId = context.CandidateRecruitments.Where(x => x.Id == item.ItemId).Select(x => x.job_vacancy_id).FirstOrDefault();
                    item.TemplateNotify = item.TemplateNotify.Replace("{jobVacancyId}", jobVacancyId);
                }
                if (item.CreatedOn != null)
                {
                    item.CreatedOnString = item.CreatedOn.Value.ToString("yyyyMMddHHmm");
                }
            }


            return data;
        }

        #region For Dashboard Service...
        public async Task<VmDsGeneral> GetDashboardGeneral(string userId, string empId)
        {
            var sqlParam = new SqlParameter[]
                {
                new SqlParameter("@UserId", SqlDbType.NVarChar) {Value = userId},
                new SqlParameter("@EmpId", SqlDbType.NVarChar) {Value = empId}
                };
            var result = await context.ExecuteStoredProcedure<VmDsGeneral>("dbo.usp_GetDashboardGeneral",sqlParam);
            return result.FirstOrDefault();
        }

        public async Task<List<VmDsKanban>> GetDashboardKanban(string userId)
        {
            var listStatus = await context.ExecuteStoredProcedure<VmJobStatus>("dbo.uspGet_JobStatus");
            var listDs = new List<VmDsKanban>();
            foreach (var stat in listStatus)
            {
                var sqlParam = new SqlParameter[]
                    {
                        new SqlParameter("@userId", SqlDbType.NVarChar) {Value = userId},
                        new SqlParameter("@jobStatusId", SqlDbType.NVarChar) {Value = stat.id}
                    };
                var model = new VmDsKanban();
                model.ListJobDetails = await context.ExecuteStoredProcedure<VmJobDetail>("dbo.uspGet_JobDetailByUserIdAndStatusId", sqlParam);
                model.id = stat.id;
                model.name = stat.name;
                listDs.Add(model);
            }
            return listDs.ToList();
        }

        public async Task<VmDsSummary> GetDashboardSummary()
        {
            var data = await context.ExecuteStoredProcedure<VmDsSummary>("dbo.uspGetDashboardSummaries");
            return data.FirstOrDefault();
        }

        public async Task<LinkToTimesheetGrid> Getlinktotimesheetgrid(string type, string userId)
        {
            LinkToTimesheetGrid resp = new LinkToTimesheetGrid();
            var result = string.Empty;
            string divisionid = string.Empty;
            DateTime? startDate = null;
            DateTime? endDate = null;
            if (type.Contains("month"))
            {
                DateTime now = DateTime.Now;
                startDate = new DateTime(Convert.ToInt32(now.Year), Convert.ToInt32(now.Month), 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);

                if (type == "month-1")
                {
                    result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=" + userId + "&divisionid=&isNonJob=1";
                }
                else if (type == "month-2")
                {
                    result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=" + userId + "&divisionid=&isNonJob=2";
                }
                else
                {
                    result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=" + userId + "&divisionid=&isNonJob=0";
                }
            }
            else if (type == "week")
            {
                startDate = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
                endDate = startDate.Value.AddDays(6);

                result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=" + userId + "&divisionid=";
            }
            else if (type.Contains("team"))
            {
                DateTime now = DateTime.Now;
                startDate = new DateTime(Convert.ToInt32(now.Year), Convert.ToInt32(now.Month), 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
                divisionid = await (from x in context.TmUserApps
                                    join t in context.EmployeeBasicInfos on x.EmployeeBasicInfoId equals t.Id
                                    join y in context.BusinessUnitJobLevels on t.BusinessUnitJobLevelId equals y.Id
                                    where x.EmployeeBasicInfoId == userId
                                    select y.BusinessUnitDivisionId

                                   ).FirstOrDefaultAsync();

                if (type == "team-1")
                {
                    result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=&divisionid=" + divisionid + "&isNonJob=1";
                }
                else if (type == "team-2")
                {
                    result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=&divisionid=" + divisionid + "&isNonJob=2";
                }
                else
                {
                    result = "/finance/timesheetgridreport?startDate=" + startDate.ToString() + "&endDate=" + endDate.ToString() + "&userid=&divisionid=" + divisionid + "&isNonJob=0";
                }

            }
            else if (type.Contains("task"))
            {
                DateTime now = DateTime.Now;
                startDate = new DateTime(Convert.ToInt32(now.Year), Convert.ToInt32(now.Month), 1);
                endDate = startDate.Value.AddMonths(1).AddDays(-1);
                divisionid = await (from x in context.TmUserApps
                                    join t in context.EmployeeBasicInfos on x.EmployeeBasicInfoId equals t.Id
                                    join y in context.BusinessUnitJobLevels on t.BusinessUnitJobLevelId equals y.Id
                                    where x.EmployeeBasicInfoId == userId
                                    select y.BusinessUnitDivisionId

                                   ).FirstOrDefaultAsync();
                if (type == "task-mine")
                {
                    result = "/pm/reporttask?startDate=" + startDate.ToString() + "&endDate=&userTask="+ userId + "&divisionid=&legalEntityId=";
                }
                else if (type == "task-all")
                {
                    result = "/pm/reporttask?startDate=" + startDate.ToString() + "&endDate=&userTask=&divisionid=" + divisionid + "&legalEntityId=";
                }
            }
            
            resp.link = result;
            return resp ;
        }

        public async Task<VmDsNewPm> GetDsNewPm(string userId)
        {
            var result = new VmDsNewPm();
            vmHomePM dataHome = new vmHomePM();
            List<VmMostUpdateJob> listJob = new List<VmMostUpdateJob>();
            List<VmProjectType> listProjectType = new List<VmProjectType>();
            List<VmRevenuePce> listRevenue = new List<VmRevenuePce>();
            List<VmJobTaskThisWeek> listJobThisWeek = new List<VmJobTaskThisWeek>();
            List<VmJobDivisionThisWeek> listDivisionThisWeek= new List<VmJobDivisionThisWeek>();
            List<VmJobGrade> listJobGrade = new List<VmJobGrade>();

            var sqlParam = new SqlParameter[]
                   {
                        new SqlParameter("@email", SqlDbType.NVarChar) {Value = userId},
                   };
            var ds = await context.ExecuteStoredProcedureDataSet("dbo.uspHomeProjectManagement", sqlParam);
            foreach (DataRow dr in ds.Tables[0].Rows) {
                dataHome.my_hours_this_week = dr["my_hours_this_week"].ToString();
                dataHome.my_hours_this_week_percent = dr["my_hours_this_week_percent"].ToString();
                dataHome.my_hours_this_month = dr["my_hours_this_month"].ToString();
                dataHome.my_hours_this_month_percent = dr["my_hours_this_month_percent"].ToString();
                dataHome.my_billable_hours = dr["my_billable_hours"].ToString();
                dataHome.my_billable_hours_percent = dr["my_billable_hours_percent"].ToString();
                dataHome.my_non_billable_hours = dr["my_non_billable_hours"].ToString();
                dataHome.my_non_billable_hours_percent = dr["my_non_billable_hours_percent"].ToString();
                dataHome.team_billable_hours = dr["team_billable_hours"].ToString();
                dataHome.team_billable_hours_percent = dr["team_billable_hours_percent"].ToString();
                dataHome.team_non_billable_hours = dr["team_non_billable_hours"].ToString();
                dataHome.team_non_billable_hours_percent = dr["team_non_billable_hours_percent"].ToString();
                dataHome.mine_job = dr["mine_job"].ToString();
                dataHome.div_job = dr["div_job"].ToString();
                dataHome.mine_job_percent = dr["mine_job_percent"].ToString();
                dataHome.div_job_percent = dr["div_job_percent"].ToString();
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr2 in ds.Tables[1].Rows)
                {
                    VmMostUpdateJob r = new VmMostUpdateJob();
                    r.Deadline = dr2["Deadline"].ToString();
                    r.Status = dr2["Status"].ToString();
                    r.StartJob = dr2["StartJob"].ToString();
                    r.ProjectName = dr2["ProjectName"].ToString();
                    r.job_number = dr2["job_number"].ToString();
                    listJob.Add(r);
                }
            }
            else
            {
                VmMostUpdateJob r = new VmMostUpdateJob()
                {
                    Deadline = "",
                    ProjectName = "",
                    StartJob = "",
                    Status = ""
                };
                listJob.Add(r);
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                foreach (DataRow dr3 in ds.Tables[2].Rows)
                {
                    VmProjectType r = new VmProjectType();
                    r.job_type_name = dr3["job_type_name"].ToString();
                    r.job_type_count = dr3["job_type_count"].ToString();


                    listProjectType.Add(r);
                }
            }
            else
            {
                VmProjectType r = new VmProjectType()
                {
                    job_type_count = "0",
                    job_type_name = ""

                };
                listProjectType.Add(r);
            }

            if (ds.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr4 in ds.Tables[3].Rows)
                {
                    VmRevenuePce r = new VmRevenuePce();
                    r.division_name = dr4["division_name"].ToString();
                    r.total_division_pce = dr4["total_division_pce"].ToString();


                    listRevenue.Add(r);
                }
            }
            else
            {
                VmRevenuePce r = new VmRevenuePce()
                {
                    division_name = "",
                    total_division_pce = "0"
                };
                listRevenue.Add(r);
            }

            if (ds.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr5 in ds.Tables[4].Rows)
                {
                    VmJobTaskThisWeek r = new VmJobTaskThisWeek();
                    r.my_job_task_this_week = dr5["my_job_task_this_week"].ToString();
                    r.my_job_task_count_this_week = dr5["my_job_task_count_this_week"].ToString();


                    listJobThisWeek.Add(r);
                }
            }
            else
            {
                VmJobTaskThisWeek r = new VmJobTaskThisWeek()
                {
                    my_job_task_this_week = "",
                    my_job_task_count_this_week = "0"
                };
                listJobThisWeek.Add(r);
            }

            if (ds.Tables[5].Rows.Count >0)
            {
                foreach (DataRow dr6 in ds.Tables[5].Rows)
                {
                    VmJobDivisionThisWeek r = new VmJobDivisionThisWeek();
                    r.section_division_job = dr6["section_division_job"].ToString();
                    r.section_division_job_count_this_week = dr6["section_division_job_count_this_week"].ToString();


                    listDivisionThisWeek.Add(r);
                }
            }
            else
            {
                VmJobDivisionThisWeek r = new VmJobDivisionThisWeek()
                {
                    section_division_job = "",
                    section_division_job_count_this_week = "0"
                };
                listDivisionThisWeek.Add(r);
            }

            if (ds.Tables[6].Rows.Count > 0)
            {
                foreach (DataRow dr7 in ds.Tables[6].Rows)
                {
                    VmJobGrade r = new VmJobGrade();
                    r.is_job_grade_id_e1 = dr7["is_job_grade_id_e1"].ToString();
                    r.is_show_revenue = dr7["is_show_revenue"].ToString();


                    listJobGrade.Add(r);
                }
            }
            else
            {
                VmJobGrade r = new VmJobGrade()
                {
                    is_job_grade_id_e1 = "",
                    is_show_revenue = ""
                };
                listJobGrade.Add(r);
            }

            result.Data = dataHome;
            result.DataMostUpdateJob = listJob;
            result.DataProjectType = listProjectType;
            result.DataRevenue = listRevenue;
            result.DataJobThisWeek = listJobThisWeek;
            result.DataDivisionThisWeek = listDivisionThisWeek;
            result.DataJobGrade = listJobGrade;


            return result;
        }

        public async Task<List<TrainingSchedule>> GetDsHris(string userId)
        {

            var sqlParam = new SqlParameter[]
                {
                        new SqlParameter("@userId", SqlDbType.NVarChar) {Value = userId},
            };

            var x = await context.ExecuteStoredProcedure<TrainingSchedule>("dbo.uspHomeHrisCalender", sqlParam);
            var result = x.ToList();
            return result;
        }

        public async Task<List<VwDsPmApproval>> GetDashboardPmApproval()
        {
            var data = await context.ExecuteStoredProcedure<VwDsPmApproval>("dbo.uspGetDashboardPmApproval");
            return data.ToList();
        }

        public async Task<List<VwDsPmApproval>> GetDashboardPmMyTask(string userid)
        {
            var sqlParam = new SqlParameter[]
                    {
                        new SqlParameter("@employeeId", SqlDbType.NVarChar) {Value = userid}
                    };
            var data = await context.ExecuteStoredProcedure<VwDsPmApproval>("dbo.uspGetDashboardMyTask", sqlParam);
            return data.ToList();
        }

        public async Task<List<VmTableOfApp>> GetListOfTable()
        {
            var data = await context.ExecuteStoredProcedure<VmTableOfApp>("dbo.uspGetListOfTable");
            return data.ToList();
        }

        private async Task<DataTable> GetColumnsTable(string schemaName, string tableName)
        {
            var sqlParam = new SqlParameter[]
                {
                        new SqlParameter("@SchemaName", SqlDbType.NVarChar) {Value = schemaName},
                        new SqlParameter("@TableName", SqlDbType.NVarChar) {Value = tableName}
                };
            return await context.ExecuteStoredProcedure("dbo.uspGetColumnsNameOfTable", sqlParam);
        }

        public async Task<VmDataOfTable> GetRowsTable(string schemaName, string tableName)
        {
            var data = await GetColumnsTable(schemaName,tableName);
            var columns = "";
            var countRow = data.Rows.Count;
            var xx = 0;
            var model = new VmDataOfTable();
            string[] cols = new string[countRow];

            foreach (DataRow dr in data.Rows)
            {
                cols[xx] = dr["COLUMN_NAME"].ToString();
                xx++;
                columns = columns + dr["COLUMN_NAME"].ToString();
                if (countRow != xx) columns = columns + ",";
            }
            model.ListOfColumns = cols;

            var sqlParam = new SqlParameter[]
                {
                        new SqlParameter("@cols", SqlDbType.NVarChar) {Value = columns},
                        new SqlParameter("@sc", SqlDbType.NVarChar) {Value = schemaName},
                        new SqlParameter("@tb", SqlDbType.NVarChar) {Value = tableName}
                };
            model.DataTables =  await context.ExecuteStoredProcedure("dbo.uspExecDataTableForPivot", sqlParam);
            return model;
        }

        #endregion

        #region For Import Data Tools
        public async Task<DataTable> GetTemplateDataForImportData(string schemaName, string tableName)
        {
            var query = "SELECT TOP 1 * FROM " + schemaName +"."+ tableName;
            var sqlParam = new SqlParameter[]
                {
                        //new SqlParameter("@Query", SqlDbType.NVarChar) {Value = query}
                        new SqlParameter("@Query",query)
                };
            return await context.ExecuteStoredProcedure("dbo.uspGetDataForTemplateImportDataTools", sqlParam);
        }

        public async Task<int> InsertDataFromImportExcel(DataTable data, string schemaName, string tableName)
        {

            var listCols = new string[data.Columns.Count];
            var x = 0;
            foreach (System.Data.DataColumn column in data.Columns)
            {
                listCols[x] = column.ColumnName;
                x++;
            }
            x = 0;
            foreach (DataRow _Row in data.Rows)
            {
                var commandText = "INSERT INTO " + schemaName + "." + tableName + " VALUES (";
                SqlParameter[] _Parameters = new SqlParameter[data.Columns.Count];
                for (var i = 0; i < listCols.Length; i++)
                {
                    if (listCols[i].ToLower() == "id")
                        _Parameters[i] = new SqlParameter("@" + listCols[i], Guid.NewGuid().ToString());
                    else
                    {
                        if (listCols[i].ToLower() == "createdby" || listCols[i].ToLower() == "created_by")
                            _Parameters[i] = new SqlParameter("@" + listCols[i], context.GetUserId());
                        else {
                            if (listCols[i].ToLower() == "createdon" || listCols[i].ToLower() == "created_on")
                                _Parameters[i] = new SqlParameter("@" + listCols[i], DateTime.Now);
                            else {
                                _Parameters[i] = new SqlParameter("@" + listCols[i], _Row[listCols[i]].ToString());
                            }
                        }
                    }

                    if (i == (listCols.Length - 1))
                        commandText = commandText + "@" + listCols[i];
                    else
                        commandText = commandText + "@" + listCols[i] + ",";
                }
                commandText = commandText + ")";
                context.Database.ExecuteSqlCommand(commandText, _Parameters);
            }
            return await Task.FromResult(1);
        }
        #endregion
    }
}

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}
