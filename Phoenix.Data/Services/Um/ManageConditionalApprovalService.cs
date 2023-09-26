using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageConditionalApprovalService : IManageConditionalApprovalService
    {
        readonly DataContext context;

        public string UserId => context.GetEmployeeId();

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageConditionalApprovalService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmConditionUserApproval entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmConditionUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmConditionUserApproval entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmConditionUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmConditionUserApproval entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> DeleteByUserApprovalTempId(string id)
        {
            var commandText = "DELETE FROM Um.TmConditionUserApproval WHERE TemplateUserApprovalId = @Id";
            var name = new SqlParameter("@Id", id);
            return await context.Database.ExecuteSqlCommandAsync(commandText, name);
        }

        public Task<int> DeleteSoftRangeAsync(params TmConditionUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmConditionUserApproval entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmConditionUserApproval entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<TmConditionUserApproval>> Get()
        {
            return await context.TmConditionUserApprovals.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
        }

        public Task<List<TmConditionUserApproval>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmConditionUserApproval> Get(string id)
        {
            return await context.TmConditionUserApprovals.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
        }
    }
}
