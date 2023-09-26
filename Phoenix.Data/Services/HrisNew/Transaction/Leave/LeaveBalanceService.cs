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
using Phoenix.Data.Models;
using Phoenix.Data.RestApi;
using Phoenix.Shared.Core.Contexts;

namespace Phoenix.Data
{
    public interface ILeaveBalanceService
    {
        Task<List<LeaveBalanceView>> Get();
        Task<int> Generate(string Type);
        //Task<dynamic> GetById(string employeeId);
        Task<TableLeaveBalance> GetEmployeeLeaveBalaceInfoList(string employeeId, string leaveRequestTypeId);
        Task<List<GenerateLeaveBalanceByLeaveRequestType>> GenerateByLeaveRequestTypeId(string leaveRequestTypeId);
        //Task<dynamic> GetLeaveHistoryList(string employeeId);
    }

    public class LeaveBalanceService : ILeaveBalanceService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage LeaveBalance
        /// </summary>
        /// <param name="context">Database context</param>
        public LeaveBalanceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<LeaveBalanceView>> Get() => await context.ExecuteStoredProcedure<LeaveBalanceView>("uspGetLeaveBalanceList");

        public async Task<int> Generate(string Type)
        {
            var paramType = new SqlParameter("@Type", Type);
            var paramEmployeeId = new SqlParameter("@RequestBy", context.GetEmployeeId());

            var result = await context.Database.ExecuteSqlCommandAsync("EXEC uspGenerateLeaveBalanceByLeaveRequestId @Type,  @RequestBy", paramType, paramEmployeeId);
            return result;
        }

        public async Task<List<GenerateLeaveBalanceByLeaveRequestType>> GenerateByLeaveRequestTypeId(string leaveRequestTypeId)
        {
            List<GenerateLeaveBalanceByLeaveRequestType> model = new List<GenerateLeaveBalanceByLeaveRequestType>();
            if (!string.IsNullOrEmpty(leaveRequestTypeId))
            {
                var paramType = new SqlParameter[] {
                                new SqlParameter("@Id", leaveRequestTypeId)
                            };

                var dt = await context.GetDataTable(string.Format("EXEC uspGetLeaveBalanceListByLeaveRequestId '{0}'", leaveRequestTypeId), null);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GenerateLeaveBalanceByLeaveRequestType m = new GenerateLeaveBalanceByLeaveRequestType();
                        if (!string.IsNullOrEmpty(dt.Rows[i]["EmployeeId"].ToString()))
                        {
                            m.EmployeeId = dt.Rows[i]["EmployeeId"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["EmployeeName"].ToString()))
                        {
                            m.EmployeeName = dt.Rows[i]["EmployeeName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["LegalEntityName"].ToString()))
                        {
                            m.LegalEntityName = dt.Rows[i]["LegalEntityName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["JobTitleName"].ToString()))
                        {
                            m.JobTitleName = dt.Rows[i]["JobTitleName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["JobGradeName"].ToString()))
                        {
                            m.JobGradeName = dt.Rows[i]["JobGradeName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["JoinDate"].ToString()))
                        {
                            m.JoinDate = DateTime.Parse(dt.Rows[i]["JoinDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["EmploymentStatusName"].ToString()))
                        {
                            m.EmploymentStatusName = dt.Rows[i]["EmploymentStatusName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt.Rows[i]["LeaveType"].ToString()))
                        {
                            m.LeaveType = dt.Rows[i]["LeaveType"].ToString();
                        }
                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["BalanceCurrentYear"].ToString()))
                            {
                                var x = dt.Rows[i]["BalanceCurrentYear"].ToString();
                                m.BalanceCurrentYear = Convert.ToDecimal(dt.Rows[i]["BalanceCurrentYear"].ToString());
                            }
                        }
                        catch 
                        {
                            m.BalanceCurrentYear = null;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["BalanceCarryOver"].ToString()))
                            {
                                m.BalanceCarryOver = Convert.ToDecimal(dt.Rows[i]["BalanceCarryOver"].ToString());
                            }
                        }
                        catch
                        {
                            m.BalanceCarryOver = null;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["TotalCompensatoryDayOff"].ToString()))
                            {
                                m.TotalCompensatoryDayOff = Convert.ToDecimal(dt.Rows[i]["TotalCompensatoryDayOff"].ToString());
                            }
                        }
                        catch
                        {
                            m.TotalCompensatoryDayOff = null;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["ExpiredDateCurrentYear"].ToString()))
                            {
                                m.ExpiredDateCurrentYear = DateTime.Parse(dt.Rows[i]["ExpiredDateCurrentYear"].ToString());
                            }
                        }
                        catch
                        {
                            m.ExpiredDateCurrentYear = null;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["ExpiredDateCarryOver"].ToString()))
                            {
                                m.ExpiredDateCarryOver = DateTime.Parse(dt.Rows[i]["ExpiredDateCarryOver"].ToString());
                            }
                        }
                        catch
                        {
                            m.ExpiredDateCarryOver = null;
                        }

                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["ExpiredYear"].ToString()))
                            {
                                m.ExpiredYear = DateTime.Parse(dt.Rows[i]["ExpiredYear"].ToString());
                            }
                        }
                        catch
                        {
                            m.ExpiredYear = null;
                        }
                        model.Add(m);
                    }
                }

            }

            return model;
        }

        public async Task<TableLeaveBalance> GetEmployeeLeaveBalaceInfoList(string employeeId, string leaveRequestTypeId)
        {
            //return await context.LeaveBalances
            //  .Where(x => x.EmployeeBasicInfoId == employeeId && !(x.IsDeleted ?? false) && x.ExpiredDate >= (endDate ?? DateTime.Now))
            //  .Select(x => new
            //  {
            //      x.Id,
            //      x.EmployeeBasicInfoId,
            //      LeaveType = LeaveType.LeaveTypeName(x.Type),
            //      x.Balance,
            //      x.ExpiredDate
            //  }).AsNoTracking().ToListAsync();
            TableLeaveBalance result = new TableLeaveBalance();

            UseCarryLeaveBalance useCarry = new UseCarryLeaveBalance();
            List<ListTableLeaveBalance> model = new List<ListTableLeaveBalance>();

            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(leaveRequestTypeId))
            {
                var paramType = new SqlParameter[] {
                                new SqlParameter("@RequestBy", employeeId),
                                new SqlParameter("@LeaveRequestTypeId", leaveRequestTypeId)
                            };

                var ds = await context.ExecuteStoredProcedureDataSet("uspGetLeaveBalanceByEmployeeId", paramType);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ListTableLeaveBalance m = new ListTableLeaveBalance();
                        var dt = ds.Tables[0];
                        if (!string.IsNullOrEmpty(dt.Rows[i]["BalanceCarryOver"].ToString()))
                        {
                            m.BalanceCarryOver = Convert.ToDecimal(dt.Rows[i]["BalanceCarryOver"].ToString());
                        }
                        else
                        {
                            m.BalanceCarryOver = 0;
                        }

                        if (!string.IsNullOrEmpty(dt.Rows[i]["BalanceCurrentYear"].ToString()))
                        {
                            m.BalanceCurrentYear = Convert.ToDecimal(dt.Rows[i]["BalanceCurrentYear"].ToString());
                        }
                        else
                        {
                            m.BalanceCurrentYear = 0;
                        }

                        m.EmployeBasicInfoId = dt.Rows[i]["EmployeBasicInfoId"].ToString();

                        if (!string.IsNullOrEmpty(dt.Rows[i]["ExpiredDateCarryOver"].ToString()))
                        {
                            m.ExpiredDateCarryOver = DateTime.Parse(dt.Rows[i]["ExpiredDateCarryOver"].ToString());
                        }

                        if (!string.IsNullOrEmpty(dt.Rows[i]["ExpiredDateCurrentYear"].ToString()))
                        {
                            m.ExpiredDateCurrentYear = DateTime.Parse(dt.Rows[i]["ExpiredDateCurrentYear"].ToString());
                        }
                        m.LeaveType = dt.Rows[i]["LeaveType"].ToString();
                        model.Add(m);
                    }

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    UseCarryLeaveBalance x = new UseCarryLeaveBalance();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        var dt = ds.Tables[1];
                        if (!string.IsNullOrEmpty(dt.Rows[i]["UseCurrentYear"].ToString()))
                        {
                            if (dt.Rows[i]["UseCurrentYear"].ToString() == "1")
                            {
                                x.UseCurrentYear = true;
                            }
                            else
                            {
                                x.UseCurrentYear = false;
                            }
                        }

                        if (!string.IsNullOrEmpty(dt.Rows[i]["UseCarryOver"].ToString()))
                        {
                            if (dt.Rows[i]["UseCarryOver"].ToString() == "1")
                            {
                                x.UseCarryOver = true;
                            }
                            else
                            {
                                x.UseCarryOver = false;
                            }
                        }
                    }
                }
            }

            result.ListTableLeaveBalance = model;
            result.UseCarryLeaveBalance = useCarry;
            return result;
        }

        //public async Task<dynamic> GetById(string employeeId)
        //{
        //    return await context.LeaveBalances
        //      .Where(x => x.EmployeeBasicInfoId == employeeId && !(x.IsDeleted ?? false))
        //      .Select(x => new
        //      {
        //          x.Id,
        //          LeaveType = LeaveType.LeaveTypeName(x.Type),
        //          x.Balance,
        //          Default = x.Total
        //      }).FirstOrDefaultAsync();
        //}


        //public async Task<dynamic> GetLeaveHistoryList(string employeeId)
        //{
        //    return await (from a in context.LeaveTransactions
        //                  join b in context.Leaves on a.LeaveId equals b.Id
        //                  where b.EmployeeBasicInfoId == employeeId
        //                  select new
        //                  {
        //                      a.Id,
        //                      Date = a.TakenDate,
        //                      Reason = "",
        //                      Days = a.Total
        //                  }).AsNoTracking().ToListAsync();
        //}
    }
    public class TableLeaveBalance
    { 
        public List<ListTableLeaveBalance> ListTableLeaveBalance { get; set; }
        public UseCarryLeaveBalance UseCarryLeaveBalance { get; set; }
    }
    public class ListTableLeaveBalance
    {
        public string LeaveType { get; set; }
        public string EmployeBasicInfoId { get; set; }
        public DateTime? ExpiredDateCurrentYear { get; set; }
        public Decimal BalanceCurrentYear { get; set; }
        public Decimal BalanceCarryOver { get; set; }
        public DateTime? ExpiredDateCarryOver { get; set; }

    }

    public class UseCarryLeaveBalance
    {
        public bool UseCurrentYear { get; set; }
        public bool UseCarryOver { get; set; }
    }

    public class GenerateLeaveBalanceByLeaveRequestType
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string LegalEntityName { get; set; }
        public string JobTitleName { get; set; }
        public string JobGradeName { get; set; }
        public DateTime? JoinDate { get; set; }
        public string EmploymentStatusName { get; set; }
        public string LeaveType { get; set; }
        public decimal? BalanceCurrentYear { get; set; }
        public DateTime? ExpiredDateCurrentYear { get; set; }
        public decimal? BalanceCarryOver { get; set; }
        public DateTime? ExpiredDateCarryOver { get; set; }
        public decimal? TotalCompensatoryDayOff { get; set; }
        public DateTime? ExpiredYear { get; set; }
    }
}



