using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Phoenix.Data.Attributes;
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Data.Services.Um;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface IOvertimeService : IDataServiceHris<Overtime>
    {
        Task<List<Overtime>> GetApprovalList();
        Task<List<Overtime>> GetAllOvertimeTransaction();
        Task<List<OvertimeReport>> GetOvertimeReport(string year);
        Task<OvertimeReportDetail> GetDetailOvertimeReport(string Id, string year);

        Task<List<OvertimeReportBody>> GetDetailOvertimeReportBody(string Id, string year);
        //bool IsRequest();
    }

    public class OvertimeService : IOvertimeService
    {
        readonly DataContext context;
        readonly GlobalFunctionApproval globalFcApproval;
        private readonly IManageUserAppService userApp;
        readonly Services.Um.IManageMenuService menuService;
        string idTemplate = "";

        /// <summary>
        /// And endpoint to manage Overtime
        /// </summary>
        /// <param name="context">Database context</param>
        public OvertimeService(DataContext context, GlobalFunctionApproval globalFcApproval, IManageUserAppService userApp, Services.Um.IManageMenuService menuService)
        {
            this.context = context;
            this.globalFcApproval = globalFcApproval;
            this.userApp = userApp;
            this.menuService = menuService;
        }

        public async Task<int> AddAsync(Overtime entity)
        {
            //var id = await TransID.GetTransId(context, Code.Overtime, DateTime.Today);
            //entity.Id = id;
            //entity.RequestCode = id;
            //var dataMenu = await menuService.GetByUniqueName(MenuUnique.Overtime).ConfigureAwait(false);
            //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            //idTemplate = dataTempate.Id;
            //return await globalFcApproval.SubmitApproval<Overtime>(false, entity.Id, entity.Status, $"{ApprovalLink.Overtime}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Overtime, entity);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var id = await TransID.GetTransId(context, Code.Overtime, DateTime.Today);
                    entity.Id = id;
                    entity.RequestCode = id;

                    await context.PhoenixAddAsync(entity);
                    var isApprove = true;
                    if (entity.Status != 0)//0: Draft
                    {

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Overtime).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        idTemplate = dataTempate.Id;
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Overtime}?Id={entity.Id}&isApprove=true",
                            IdTemplate = idTemplate,
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.OvertimeNotification
                        };



                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);

                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }

            }


        }

        public async Task<int> EditAsync(Overtime entity)
        {
            //if (string.IsNullOrEmpty(entity.RequestCode))
            //{
            //    entity.RequestCode = (context.Overtimes.Where(x => x.Id == entity.Id).FirstOrDefault() ?? new Overtime()).RequestCode;
            //}
            //var dataMenu = await menuService.GetByUniqueName(MenuUnique.Overtime).ConfigureAwait(false);
            //if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
            //var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
            //idTemplate = dataTempate.Id;
            //return await globalFcApproval.SubmitApproval<Overtime>(true, entity.Id, entity.Status, $"{ApprovalLink.Overtime}?Id={entity.Id}&isApprove=true", idTemplate, MenuUnique.Overtime, entity);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (string.IsNullOrEmpty(entity.RequestCode))
                    {
                        entity.RequestCode = (context.Overtimes.Where(x => x.Id == entity.Id).FirstOrDefault() ?? new Overtime()).RequestCode;
                    }

                    context.PhoenixEdit(entity);

                    var isApprove = true;
                    if (entity.Status != 0)//0: Draft
                    {

                        var dataMenu = await menuService.GetByUniqueName(MenuUnique.Overtime).ConfigureAwait(false);
                        if (dataMenu == null) { throw new Exception("You do not have access to do this process."); }
                        var dataTempate = context.TmTemplateApprovals.Where(x => x.MenuId == dataMenu.Id && x.IsDeleted == false).FirstOrDefault();
                        idTemplate = dataTempate.Id;
                        var vm = new Models.ViewModel.TransApprovalHrisVm()
                        {
                            MenuId = dataMenu.Id,
                            RefId = entity.Id,
                            DetailLink = $"{ApprovalLink.Overtime}?Id={entity.Id}&isApprove=true",
                            IdTemplate = idTemplate,
                            Module = NotificationModuleType.ModuleHRIS,
                            NotificationType = NotificationTypeCode.OvertimeNotification
                        };



                        var subGroupId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessSubGroup(context.HttpContext);
                        var divisionId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetBusinessUnitDivisi(context.HttpContext);
                        var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
                        isApprove = await globalFcApproval.Save(vm, subGroupId, divisionId, employeeId);

                        if (isApprove)
                        {
                            var save = await context.SaveChangesAsync();
                            transaction.Commit();
                            return save;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("please check the approval template that will be processed");
                        }
                    }
                    else
                    {
                        var save = await context.SaveChangesAsync();
                        transaction.Commit();
                        return save;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }

            }

        }

        public async Task<int> ApproveAsync(Overtime entity)
        {
            var employeeId = Phoenix.Shared.Core.PrincipalHelpers.ClainJwtPrincipalHelper.GetEmployeeId(context.HttpContext);
            var param = new SqlParameter[]
                   {
                        new SqlParameter("@EmployeeId",entity.EmployeeBasicInfoId),
                        new SqlParameter("@RequestBy",employeeId)
                   };
            var res = await context.ExecuteStoredProcedure("uspGenerateCDO", param);
            return await globalFcApproval.ProcessApproval<Overtime>(entity.Id, entity.Status, entity.RemarkRejected, entity);
        }

        public async Task<int> DeleteSoftAsync(Overtime entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }


        public async Task<List<Overtime>> GetAllOvertimeTransaction()
        {
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            var employeeId = context.GetEmployeeId();

            var result = await (from a in context.Overtimes
                                join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                where !(a.IsDeleted ?? false)
                                select new Overtime
                                {
                                    Id = a.Id,
                                    OvertimeDate = a.OvertimeDate,
                                    HourOvertime = a.HourOvertime,
                                    StatusDesc = c.StatusApprovedDescription,
                                    RequestDate = a.RequestDate,
                                    CreatedOn = a.CreatedOn,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = w.Id,
                                        EmployeeName = w.NameEmployee,
                                        //DateEmployed = w.JoinDate,
                                        //Phone = w.PhoneNumber,
                                        //Email = w.Email,
                                        //DateBirth = w.DateBirth,
                                        JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.TitleName).FirstOrDefault(),
                                        //Grade = w.BusinessUnitJobLevelId == null ? "" :
                                        //                       (from r in context.BusinessUnitJobLevels
                                        //                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                        //                        where r.Id == w.BusinessUnitJobLevelId
                                        //                        select s.GradeName).FirstOrDefault()
                                        LegalEntityId = w.LegalEntityId,
                                        LegalEntityName = (from r in context.LegalEntity
                                                           where r.Id == w.LegalEntityId
                                                           select r.LegalEntityName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var result2 = await (from a in context.Overtimes
                                 join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 && (HCAdmin ? true : a.CreatedBy == employeeId)
                                 select new Overtime
                                 {
                                     Id = a.Id,
                                     OvertimeDate = a.OvertimeDate,
                                     HourOvertime = a.HourOvertime,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     RequestDate = a.RequestDate,
                                     CreatedOn = a.CreatedOn,
                                     requestEmployee = new InfoEmployee
                                     {
                                         EmployeeId = w.Id,
                                         EmployeeName = w.NameEmployee,
                                         //DateEmployed = w.JoinDate,
                                         //Phone = w.PhoneNumber,
                                         //Email = w.Email,
                                         //DateBirth = w.DateBirth,
                                         JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                                (from r in context.BusinessUnitJobLevels
                                                                 join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                 where r.Id == w.BusinessUnitJobLevelId
                                                                 select s.TitleName).FirstOrDefault(),
                                         //Grade = w.BusinessUnitJobLevelId == null ? "" :
                                         //                       (from r in context.BusinessUnitJobLevels
                                         //                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                         //                        where r.Id == w.BusinessUnitJobLevelId
                                         //                        select s.GradeName).FirstOrDefault()
                                         LegalEntityId = w.LegalEntityId,
                                         LegalEntityName = (from r in context.LegalEntity
                                                            where r.Id == w.LegalEntityId
                                                            select r.LegalEntityName).FirstOrDefault()
                                     }
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (result == null) result = new List<Overtime>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }
        public async Task<List<Overtime>> Get()
        {
            var group = await userApp.GetByUserLogin();
            var HCAdmin = group.Where(x => x.GroupId == GroupAcess.HCAdmin).Count() > 0;
            var employeeId = context.GetEmployeeId();

            var result = await (from a in context.Overtimes
                                join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join e in context.EmployeeBasicInfos on a.CreatedBy equals e.Id
                                where !(a.IsDeleted ?? false) && (HCAdmin ? true : a.CreatedBy == employeeId)
                                select new Overtime
                                {
                                    Id = a.Id,
                                    OvertimeDate = a.OvertimeDate,
                                    HourOvertime = a.HourOvertime,
                                    StatusDesc = c.StatusApprovedDescription,
                                    RequestDate = a.RequestDate,
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    RequestorName = e.NameEmployee,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = w.Id,
                                        EmployeeName = w.NameEmployee,
                                        //DateEmployed = w.JoinDate,
                                        //Phone = w.PhoneNumber,
                                        //Email = w.Email,
                                        //DateBirth = w.DateBirth,
                                        JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.TitleName).FirstOrDefault(),
                                        //Grade = w.BusinessUnitJobLevelId == null ? "" :
                                        //                       (from r in context.BusinessUnitJobLevels
                                        //                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                        //                        where r.Id == w.BusinessUnitJobLevelId
                                        //                        select s.GradeName).FirstOrDefault()
                                        LegalEntityId = w.LegalEntityId,
                                        LegalEntityName = (from r in context.LegalEntity
                                                           where r.Id == w.LegalEntityId
                                                           select r.LegalEntityName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            var result2 = await (from a in context.Overtimes
                                 join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                 join e in context.EmployeeBasicInfos on a.CreatedBy equals e.Id
                                 where !(a.IsDeleted ?? false) && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                 && (HCAdmin ? true : a.CreatedBy == employeeId)
                                 select new Overtime
                                 {
                                     Id = a.Id,
                                     OvertimeDate = a.OvertimeDate,
                                     HourOvertime = a.HourOvertime,
                                     StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                     RequestDate = a.RequestDate,
                                     CreatedOn = a.CreatedOn,
                                     CreatedBy = a.CreatedBy,
                                     RequestorName = e.NameEmployee,
                                     requestEmployee = new InfoEmployee
                                     {
                                         EmployeeId = w.Id,
                                         EmployeeName = w.NameEmployee,
                                         //DateEmployed = w.JoinDate,
                                         //Phone = w.PhoneNumber,
                                         //Email = w.Email,
                                         //DateBirth = w.DateBirth,
                                         JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                                (from r in context.BusinessUnitJobLevels
                                                                 join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                 where r.Id == w.BusinessUnitJobLevelId
                                                                 select s.TitleName).FirstOrDefault(),
                                         //Grade = w.BusinessUnitJobLevelId == null ? "" :
                                         //                       (from r in context.BusinessUnitJobLevels
                                         //                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                         //                        where r.Id == w.BusinessUnitJobLevelId
                                         //                        select s.GradeName).FirstOrDefault()
                                         LegalEntityId = w.LegalEntityId,
                                         LegalEntityName = (from r in context.LegalEntity
                                                            where r.Id == w.LegalEntityId
                                                            select r.LegalEntityName).FirstOrDefault()
                                     }
                                 }).OrderByDescending(x => x.CreatedOn).ToListAsync();

            if (result == null) result = new List<Overtime>();
            result.AddRange(result2);
            return result.OrderByDescending(x => x.CreatedOn).ToList();
        }

        public async Task<List<OvertimeReport>> GetOvertimeReport(string year)
        {
            var intYear = Convert.ToInt32(year);
            //var result = await (from q in context.Overtimes.Where(x => x.IsDeleted != true)
            //                    join r in context.EmployeeBasicInfos on q.EmployeeBasicInfoId equals r.Id
            //                    join s in context.LegalEntity on r.LegalEntityId equals s.Id
            //                    join t in context.JobTitles on r.JobTitleId equals t.Id
            //                    join u in context.JobGrades on r.JobGradeId equals u.Id
            //                    join v in context.LeaveBalances on q.EmployeeBasicInfoId equals v.EmployeeBasicInfoId
            //                    where q.OvertimeDate.Year == intYear && v.Type == "COMPENSATORY"
            //                    select new OvertimeReport()
            //                    {
            //                        Id = q.Id,
            //                        EmployeeBasicInfoId = r.Id,
            //                        EmployeeName = r.NameEmployee,
            //                        JobGradeId = r.JobGradeId,
            //                        JobGradeName = u.GradeName,
            //                        JobTitleId = r.JobTitleId,
            //                        JobTitleName = t.TitleName,
            //                        LegalEntityId = r.LegalEntityId,
            //                        LegalEntityName = s.LegalEntityName,
            //                        Year = intYear,
            //                        CDO = v.Total,
            //                        BalanceCDO = v.Balance
            //                    }
            //                    ).ToListAsync();
            List<OvertimeReport> result = new List<OvertimeReport>();
            var param = new SqlParameter[]
                    {
                        new SqlParameter("@year",intYear)
                    };
            result = await context.ExecuteStoredProcedure<OvertimeReport>("uspGetListOvertimeReport", param);



            return result;
        }

        public async Task<List<OvertimeReportBody>> GetDetailOvertimeReportBody(string Id, string year)
        {
            var intYear = Convert.ToInt32(year);
            List<OvertimeReportBody> detail = new List<OvertimeReportBody>();
            var param = new SqlParameter[]
                    {
                         new SqlParameter("@employeeId",Id),
                        new SqlParameter("@year",intYear)
                    };
            DataSet ds = await context.ExecuteStoredProcedureDataSet("uspGetOvertimeReportDetailByEmployeeId", param);

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    OvertimeReportBody m = new OvertimeReportBody();
                    var dt = ds.Tables[1];
                    m.OvertimeId = dt.Rows[i]["OvertimeId"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["OvertimeDate"].ToString()))
                    {
                        m.OvertimeDate = DateTime.Parse(dt.Rows[i]["OvertimeDate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["HoursDuration"].ToString()))
                    {
                        m.HoursDuration = DateTime.Parse(dt.Rows[i]["HoursDuration"].ToString());
                    }

                    m.HoursDurationString = dt.Rows[i]["HoursDurationString"].ToString();
                    m.UserApprovalId = dt.Rows[i]["UserApprovalId"].ToString();
                    m.UserApprovalName = dt.Rows[i]["UserApprovalName"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ApprovedDate"].ToString()))
                    {
                        m.ApprovedDate = DateTime.Parse(dt.Rows[i]["ApprovedDate"].ToString());
                    }
                    detail.Add(m);

                }
             
            }
            return detail;
        }

        public async Task<OvertimeReportDetail> GetDetailOvertimeReport(string Id, string year)
        {
            var intYear = Convert.ToInt32(year);
            OvertimeReportDetail result = new OvertimeReportDetail();
            List<OvertimeReportBody> detail = new List<OvertimeReportBody>();
            var param = new SqlParameter[]
                    {
                         new SqlParameter("@employeeId",Id),
                        new SqlParameter("@year",intYear)
                    };
            DataSet ds = await context.ExecuteStoredProcedureDataSet("uspGetOvertimeReportDetailByEmployeeId", param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];
                OvertimeReport he = new OvertimeReport();
                he.Id = dt.Rows[0]["Id"].ToString();
                he.EmployeeBasicInfoId = dt.Rows[0]["EmployeeBasicInfoId"].ToString();
                he.EmployeeName = dt.Rows[0]["EmployeeName"].ToString();
                he.JobGradeId = dt.Rows[0]["JobGradeId"].ToString();
                he.JobGradeName = dt.Rows[0]["JobGradeName"].ToString();
                he.JobTitleId = dt.Rows[0]["JobTitleId"].ToString();
                he.JobTitleName = dt.Rows[0]["JobTitleName"].ToString();
                he.LegalEntityId = dt.Rows[0]["LegalEntityId"].ToString();
                he.LegalEntityName = dt.Rows[0]["LegalEntityName"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["CDO"].ToString()))
                {
                    he.CDO = Convert.ToInt32(dt.Rows[0]["CDO"].ToString()) ;
                }
                
                if (!string.IsNullOrEmpty(dt.Rows[0]["BalanceCDO"].ToString()))
                {
                    he.BalanceCDO = decimal.Parse(dt.Rows[0]["BalanceCDO"].ToString());
                }
               
                he.TotalOvertime = dt.Rows[0]["TotalOvertime"].ToString();
                if (!string.IsNullOrEmpty(dt.Rows[0]["Year"].ToString()))
                {
                    he.Year = Convert.ToInt32(dt.Rows[0]["Year"].ToString()) ;
                }
                result.Header = he;
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    OvertimeReportBody m = new OvertimeReportBody();
                    var dt = ds.Tables[1];
                    m.OvertimeId = dt.Rows[i]["OvertimeId"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["OvertimeDate"].ToString()))
                    {
                        m.OvertimeDate = DateTime.Parse(dt.Rows[i]["OvertimeDate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["HoursDuration"].ToString()))
                    {
                        m.HoursDuration = DateTime.Parse(dt.Rows[i]["HoursDuration"].ToString());
                    }

                    m.HoursDurationString = dt.Rows[i]["HoursDurationString"].ToString();
                    m.UserApprovalId = dt.Rows[i]["UserApprovalId"].ToString();
                    m.UserApprovalName = dt.Rows[i]["UserApprovalName"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["ApprovedDate"].ToString()))
                    {
                        m.ApprovedDate = DateTime.Parse(dt.Rows[i]["ApprovedDate"].ToString());
                    }
                    detail.Add(m);

                }
                result.Detail = detail;
            }
            return result;
        }

        public async Task<List<Overtime>> GetApprovalList()
        {
            var result = await (from a in context.Overtimes
                                join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                join c in context.TrTemplateApprovals on a.Id equals c.RefId
                                join d in context.TrUserApprovals on c.Id equals d.TrTempApprovalId
                                join e in context.EmployeeBasicInfos on a.CreatedBy equals e.Id
                                where !(a.IsDeleted ?? false) && d.StatusApproved == Models.Um.StatusApproved.CurrentApproval && d.EmployeeBasicInfoId == context.GetEmployeeId()
                                select new Overtime
                                {
                                    Id = a.Id,
                                    OvertimeDate = a.OvertimeDate,
                                    HourOvertime = a.HourOvertime,
                                    StatusDesc = c.StatusApprovedDescription,
                                    RequestDate = a.RequestDate,
                                    CreatedOn = a.CreatedOn,
                                    CreatedBy = a.CreatedBy,
                                    RequestorName = e.NameEmployee,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = w.Id,
                                        EmployeeName = w.NameEmployee,
                                        //DateEmployed = w.JoinDate,
                                        //Phone = w.PhoneNumber,
                                        //Email = w.Email,
                                        //DateBirth = w.DateBirth,
                                        JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobTitles on r.JobTitleId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.TitleName).FirstOrDefault(),
                                        //Grade = w.BusinessUnitJobLevelId == null ? "" :
                                        //                       (from r in context.BusinessUnitJobLevels
                                        //                        join s in context.JobGrades on r.JobGradeId equals s.Id
                                        //                        where r.Id == w.BusinessUnitJobLevelId
                                        //                        select s.GradeName).FirstOrDefault()
                                        LegalEntityId = w.LegalEntityId,
                                        LegalEntityName = (from r in context.LegalEntity
                                                           where r.Id == w.LegalEntityId
                                                           select r.LegalEntityName).FirstOrDefault()
                                    }
                                }).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return result;
        }

        public async Task<Overtime> Get(string Id)
        {
            var result = await (from a in context.Overtimes
                                join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                join d in context.TrTemplateApprovals on a.Id equals d.RefId
                                join e in context.EmployeeBasicInfos on a.CreatedBy equals e.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id
                                select new Overtime
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    OvertimeDate = a.OvertimeDate,
                                    HourOvertime = a.HourOvertime,
                                    ReasonOvertime = a.ReasonOvertime,
                                    Status = d.StatusApproved.GetHashCode(),
                                    StatusDesc = d.StatusApprovedDescription,
                                    CreatedBy = a.CreatedBy,
                                    RequestorName = e.NameEmployee,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = w.Id,
                                        EmployeeName = w.NameEmployee,
                                        //DateEmployed = w.JoinDate,
                                        //Phone = w.PhoneNumber,
                                        //Email = w.Email,
                                        //DateBirth = w.DateBirth,
                                        JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                          (from r in context.BusinessUnitJobLevels
                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                           where r.Id == w.BusinessUnitJobLevelId
                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.GradeName).FirstOrDefault(),
                                        LegalEntityId = w.LegalEntityId,
                                        LegalEntityName = (from r in context.LegalEntity
                                                           where r.Id == w.LegalEntityId
                                                           select r.LegalEntityName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();


            if (result == null)
            {
                result = await (from a in context.Overtimes
                                join w in context.EmployeeBasicInfos on a.EmployeeBasicInfoId equals w.Id
                                where !(a.IsDeleted ?? false) && a.Id == Id && !context.TrTemplateApprovals.Any(x => x.RefId == a.Id)
                                select new Overtime
                                {
                                    Id = a.Id,
                                    EmployeeBasicInfoId = a.EmployeeBasicInfoId,
                                    RequestCode = a.RequestCode,
                                    RequestDate = a.RequestDate,
                                    OvertimeDate = a.OvertimeDate,
                                    HourOvertime = a.HourOvertime,
                                    ReasonOvertime = a.ReasonOvertime,
                                    Status = StatusTransaction.Draft,//DRAFT
                                    StatusDesc = StatusTransaction.StatusName(StatusTransaction.Draft),
                                    CreatedBy = a.CreatedBy,
                                    CreatedOn = a.CreatedOn,
                                    ModifiedBy = a.ModifiedBy,
                                    ModifiedOn = a.ModifiedOn,
                                    ApprovedBy = a.ApprovedBy,
                                    ApprovedOn = a.ApprovedOn,
                                    IsActive = a.IsActive,
                                    IsLocked = a.IsLocked,
                                    IsDefault = a.IsDefault,
                                    IsDeleted = a.IsDeleted,
                                    OwnerId = a.OwnerId,
                                    DeletedBy = a.DeletedBy,
                                    DeletedOn = a.DeletedOn,
                                    requestEmployee = new InfoEmployee
                                    {
                                        EmployeeId = w.Id,
                                        EmployeeName = w.NameEmployee,
                                        //DateEmployed = w.JoinDate,
                                        //Phone = w.PhoneNumber,
                                        //Email = w.Email,
                                        //DateBirth = w.DateBirth,
                                        JobTitle = w.BusinessUnitJobLevelId == null ? "" :
                                                          (from r in context.BusinessUnitJobLevels
                                                           join s in context.JobTitles on r.JobTitleId equals s.Id
                                                           where r.Id == w.BusinessUnitJobLevelId
                                                           select s.TitleName).FirstOrDefault(),
                                        Grade = w.BusinessUnitJobLevelId == null ? "" :
                                                               (from r in context.BusinessUnitJobLevels
                                                                join s in context.JobGrades on r.JobGradeId equals s.Id
                                                                where r.Id == w.BusinessUnitJobLevelId
                                                                select s.GradeName).FirstOrDefault(),
                                        LegalEntityId = w.LegalEntityId,
                                        LegalEntityName = (from r in context.LegalEntity
                                                           where r.Id == w.LegalEntityId
                                                           select r.LegalEntityName).FirstOrDefault()
                                    }
                                }).FirstOrDefaultAsync();
            }

            return result;
        }
    }
}