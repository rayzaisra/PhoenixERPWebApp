using System;
using System.Collections.Generic;
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
    public interface IStatusLogService
    {
        Task<List<StatusLog>> Get(string id);
    }

    public class StatusLogService : IStatusLogService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage StatusLog
        /// </summary>
        /// <param name="context">Database context</param>
        public StatusLogService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(StatusLog entity)
        {
            var employeId = context.GetEmployeeId();
            var employeeName = string.Empty;
            var seq = context.StatusLogs.AsNoTracking().Where(x => x.TransactionId == entity.TransactionId).Max(x => x.Seq);
            var employee = await context.EmployeeBasicInfos.AsNoTracking().Where(x => x.Id == employeId).FirstOrDefaultAsync();
            if (employee == null)
            {
                employeeName = context.TmUserApps.Where(x => x.EmployeeBasicInfoId == employeId).Select(x => x.AliasName).FirstOrDefault();
            }
            else
            {
                employeeName = employee.NameEmployee;
            }

            entity.Seq = (seq ?? 0) + 1;
            entity.EmployeeId = employeId;
            entity.EmployeeName = employeeName;
            entity.StatusDate = DateTime.Now;

            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<List<StatusLog>> Get(string id) => await context.StatusLogs.AsNoTracking().Where(x => x.TransactionId == id).ToListAsync();
    }
}