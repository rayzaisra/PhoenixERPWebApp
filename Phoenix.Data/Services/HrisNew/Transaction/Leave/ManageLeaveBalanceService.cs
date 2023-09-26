using System;
using System.Collections.Generic;
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

    public interface IManageLeaveBalanceService
    {
        Task<List<ManageLeaveBalanceView>> Get();
        Task<dynamic> GetEmployeeLeaveBalaceInfoList(string employeeId, DateTime? endDate);
        Task<LeaveBalanceViewModel> getAnnualLeave(string employeeId, string leaveType);
        Task<int> saveManageLeaveBalance(LeaveBalanceViewModel model);
    }

    public class ManageLeaveBalanceService : IManageLeaveBalanceService
    {
        readonly DataContext context;
        public ManageLeaveBalanceService(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<ManageLeaveBalanceView>> Get() => await context.ExecuteStoredProcedure<ManageLeaveBalanceView>("uspGetLeaveBalanceList");

        public async Task<dynamic> GetEmployeeLeaveBalaceInfoList(string employeeId, DateTime? endDate)
        {
            return await context.LeaveBalances
             .Where(x => x.EmployeeBasicInfoId == employeeId && !(x.IsDeleted ?? false) && x.ExpiredDate >= (endDate ?? DateTime.Now))
             .Select(x => new
             {
                 x.Id,
                 x.EmployeeBasicInfoId,
                 LeaveType = x.Type,
                 x.Balance,
                 x.ExpiredDate
             }).AsNoTracking().ToListAsync();
        }

        public async Task<LeaveBalanceViewModel> getAnnualLeave(string employeeId, string leaveType)
        {
            return await context.LeaveBalances.Where(x => x.EmployeeBasicInfoId == employeeId && !(x.IsDeleted ?? false) && x.Type == leaveType)
                .Select(x => new LeaveBalanceViewModel()
                {
                    EmployeeId = x.EmployeeBasicInfoId,
                    ExpiredDate = (x.ExpiredDate != null ? x.ExpiredDate.Value.ToString("yyyy-MM-dd") : "N/A"),
                    LeaveBalance = x.Balance.ToString()
                }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> saveManageLeaveBalance(LeaveBalanceViewModel model)
        {
            var data = await context.LeaveBalances.Where(x => x.EmployeeBasicInfoId == model.EmployeeId && !(x.IsDeleted ?? false) && x.Type == "").AsNoTracking().FirstOrDefaultAsync();
            var save = 0;
            if (data != null)
            {
                data.Balance = Convert.ToInt32(model.LeaveBalance);
                context.LeaveBalances.Update(data);
                save = await context.SaveChangesAsync();
            }
            else
            {
                LeaveBalance ent = new LeaveBalance()
                {
                    Balance = Convert.ToInt32(model.LeaveBalance),
                    Total = Convert.ToInt32(model.LeaveBalance),
                    EmployeeBasicInfoId = model.EmployeeId,
                    Type = "",
                    Year = DateTime.Now.Year,
                    ExpiredDate = new DateTime(DateTime.Now.Year, 12, 31)
                };
                await context.PhoenixAddAsync(ent);
                save = await context.SaveChangesAsync();
            }

            return save;

        }
    }
}
