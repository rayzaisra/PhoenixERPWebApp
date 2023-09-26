using Phoenix.Data.Models.Finance;
using Phoenix.Data.Models.Finance.Report;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Phoenix.Data.Services.Finance.Report
{
    public interface ITImeSheetReportServices
    {
        Task<List<TimeSheetByJob>> TimeSheetByJob(string job_numr, string userId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId, string divisionId, string isNonJob);
        Task<List<ReportTimeSheetClient>> GetReportClient(string client_id);
        Task<List<ReportTimeSheetByBusinessUnitNew>> GetReportClientByBusinessUnit(string jobId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId);
        Task<List<TimeSheetReportByBusinessUnit>> GetListTimeSheetByBusinessUnit();
        Task<TimeSheetReportByBusinessUnit> GetListTimeSheetByBusinessUnitById(string Id);
        Task<int> DeleteReportTimeSheetByBusinessUnit(TimeSheetReportByBusinessUnit entity);
        Task<int> AddAsync(TimeSheetReportByBusinessUnit entity);
        Task<int> EditAsync(TimeSheetReportByBusinessUnit entity);

    }
    public class TImeSheetReportServices : ITImeSheetReportServices
    {
        readonly DataContext context;
        public TImeSheetReportServices(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TimeSheetReportByBusinessUnit entity)
        {
            return await AddEdit(entity, false);
        }

        public async Task<int> EditAsync(TimeSheetReportByBusinessUnit entity)
        {
            return await AddEdit(entity, true);
        }

        private async Task<int> AddEdit(TimeSheetReportByBusinessUnit entity, bool isEdit)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var timReport = await context.TimeSheetReportByBusinessUnit.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                    if (!string.IsNullOrEmpty(entity.Id))
                    {
                        var detail = context.TimeSheetReportByBusinessUnitDetail.AsNoTracking().Where(x => x.TimesheetReportByBusinessUnitId == entity.Id).ToList();
                        if (detail.Count > 0)
                        {
                            context.TimeSheetReportByBusinessUnitDetail.RemoveRange(detail);
                        }
                    }

                    if (isEdit)
                    {
                        context.PhoenixEdit(entity);
                    }
                    else
                    {
                        entity.Id = await TransID.GetTransId(context, "TRBU", DateTime.Today);
                        await context.PhoenixAddAsync(entity);
                        await context.SaveChangesAsync();
                    }

                   
                    if (entity.Detail != null)
                    {
                        foreach (var item in entity.Detail)
                        {
                            item.Id = Guid.NewGuid().ToString();
                            item.TimesheetReportByBusinessUnitId = entity.Id;
                            await context.PhoenixAddAsync(item);
                        }
                    }

                    var save = await context.SaveChangesAsync();
                    transaction.Commit();
                    return save;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<List<ReportTimeSheetClient>> GetReportClient(string client_id)
        {
            try
            {
                List<ReportTimeSheetClient> list = new List<ReportTimeSheetClient>();
                var param = new SqlParameter[]
                        {
                        new SqlParameter("@p_client_id",client_id),

                        };
                list = await context.ExecuteStoredProcedure<ReportTimeSheetClient>("pm.report_timesheet_client", param);
                return list;
            }
            catch (Exception e)
            {
                throw new Exception("Error !, " + e.Message);
            }
        }

        public async Task<List<ReportTimeSheetByBusinessUnitNew>> GetReportClientByBusinessUnit(string jobId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId)
        {
            try
            {
                List<ReportTimeSheetByBusinessUnitNew> list = new List<ReportTimeSheetByBusinessUnitNew>();
                
                var param = new SqlParameter[]
                        {
                        new SqlParameter("@jobId",jobId),
                         new SqlParameter("@clientId",clientId),
                         new SqlParameter("@startDate",startDate),
                           new SqlParameter("@endDate",endDate),
                             new SqlParameter("@legalEntityId",legalEntityId)
                        };
                list = await context.ExecuteStoredProcedure<ReportTimeSheetByBusinessUnitNew>("uspTimesheetReportByBusinessUnit_new", param);
                return list;
            }
            catch (Exception e)
            {
                throw new Exception("Error !, " + e.Message);
            }
        }

        public async Task<List<TimeSheetByJob>> TimeSheetByJob(string job_numr, string userId, string clientId, DateTime startDate, DateTime endDate, string legalEntityId, string divisionId, string isNonJob)
        {
            try
            {
                List<TimeSheetByJob> list = new List<TimeSheetByJob>();
                var param = new SqlParameter[]
                        {
                        new SqlParameter("@p_job_id",job_numr),
                        new SqlParameter("@p_client_id",clientId),
                        new SqlParameter("@p_user_id",userId),
                        new SqlParameter("@p_start_date",startDate),
                        new SqlParameter("@p_end_date",endDate),
                        new SqlParameter("@p_legal_entity_id",legalEntityId),
                        new SqlParameter("@p_division_id",divisionId),
                        new SqlParameter("@p_isnonjob",isNonJob)
                        };
                list = await context.ExecuteStoredProcedure<TimeSheetByJob>("pm.report_timesheet_job", param);
                return list;
            }
            catch (Exception e)
            {
                throw new Exception("Error !, " + e.Message);
            }

        }

        public async Task<List<TimeSheetReportByBusinessUnit>> GetListTimeSheetByBusinessUnit()
        {
            var query = await (from q in context.TimeSheetReportByBusinessUnit
                               join r in context.LegalEntity on q.LegalEntityId equals r.Id into lqr
                               from qr in lqr.DefaultIfEmpty()
                               join s in context.EmployeeBasicInfos on q.CreatedBy equals s.Id
                               where q.IsDeleted == false && q.StartDate != null
                               select new TimeSheetReportByBusinessUnit
                               {
                                   Id = q.Id,
                                   StartDate = q.StartDate,
                                   EndDate = q.EndDate,
                                   LegalEntityId = q.LegalEntityId,
                                   LegalEntityName = qr.LegalEntityName,
                                   Status = q.Status,
                                   CreatedBy = q.CreatedBy,
                                   UserId = q.UserId,
                                   RequestName = s.NameEmployee
                               }).ToListAsync();

            return query;
        }

        public async Task<TimeSheetReportByBusinessUnit> GetListTimeSheetByBusinessUnitById(string Id)
        {
            var model = new TimeSheetReportByBusinessUnit();
            var detail = new List<TimeSheetReportByBusinessUnitDetail>();
            var header = await context.TimeSheetReportByBusinessUnit.Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (header != null)
            {
                model.Id = header.Id;
                model.Status = header.Status;
                model.CreatedBy = header.CreatedBy;
                model.CreatedOn = header.CreatedOn;
                model.IsDeleted = header.IsDeleted;
                model.StartDate = header.StartDate;
                model.EndDate = header.EndDate;
                model.LegalEntityId = header.LegalEntityId;
                model.CompanyId = header.CompanyId;
                model.JobId = header.JobId;

                var listDetail = await context.TimeSheetReportByBusinessUnitDetail.Where(x => x.TimesheetReportByBusinessUnitId == Id).ToListAsync();
                foreach (var item in listDetail)
                {
                    TimeSheetReportByBusinessUnitDetail m = new TimeSheetReportByBusinessUnitDetail();
                    m.Id = item.Id;
                    m.UserId = item.UserId;
                    m.EmployeeName = item.EmployeeName;
                    m.LegalEntityName = item.LegalEntityName;
                    m.DivisionName = item.DivisionName;
                    m.StartDate = item.StartDate;
                    m.EndDate = item.EndDate;
                    m.TotalHours = item.TotalHours;
                    m.Bu1 = item.Bu1;
                    m.Bu2 = item.Bu2;
                    m.Bu3 = item.Bu3;
                    m.Bu4 = item.Bu4;
                    m.Bu5 = item.Bu5;
                    m.Bu6 = item.Bu6;
                    m.Bu7 = item.Bu7;
                    m.Bu8 = item.Bu8;
                    m.Bu9 = item.Bu9;
                    m.Bu10 = item.Bu10;
                    m.Bu11 = item.Bu11;
                    m.Bu12 = item.Bu12;
                    m.BuNonJob = item.BuNonJob;
                    m.TotalHours = item.TotalHours;
                    m.JobPercentage = item.JobPercentage;
                    m.NonJobPercentage = item.NonJobPercentage;
                    m.TimesheetReportByBusinessUnitId = item.TimesheetReportByBusinessUnitId;
                    detail.Add(m);
                }
                model.Detail = listDetail;
            }

            return model;
        }

        public async Task<int> DeleteReportTimeSheetByBusinessUnit(TimeSheetReportByBusinessUnit entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }
    }
}
