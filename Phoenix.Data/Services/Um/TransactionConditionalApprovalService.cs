using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class TransactionConditionalApprovalService : ITransactionConditionalApprovalService
    {
        readonly DataContext context;

        public string UserId => context.GetEmployeeId();

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public TransactionConditionalApprovalService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrConditionUserApproval entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddApprovalAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TrConditionUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TrConditionUserApproval entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TrConditionUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrConditionUserApproval entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> DeleteByUserApprovalTempId(string id)
        {
            var commandText = "DELETE FROM Um.TrConditionUserApproval WHERE TrUserApprovalId = @Id";
            var name = new SqlParameter("@Id", id);
            return await context.Database.ExecuteSqlCommandAsync(commandText, name);
        }

        public Task<int> DeleteSoftRangeAsync(params TrConditionUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TrConditionUserApproval entity)
        {
            context.PhoenixEditApproval(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TrConditionUserApproval entity)
        {
            throw new NotImplementedException();
        }
        public async Task<int> UpdateNextApproval(string userApprovalId)
        {
            var commandText = "UPDATE Um.TrConditionUserApproval SET StatusApproved = 1 WHERE TrUserApprovalId = @TrUserApprovalId ";
            var paramuserApprovalId = new SqlParameter("@TrUserApprovalId", userApprovalId);
            return await context.Database.ExecuteSqlCommandAsync(commandText, paramuserApprovalId);
        }

        public async Task<List<TrConditionUserApproval>> Get()
        {
            return await context.TrConditionUserApprovals.AsNoTracking().ToListAsync();
        }
        public async Task<List<TrConditionUserApproval>> GetByUserApprovalId(string id)
        {
            return await context.TrConditionUserApprovals.AsNoTracking().Where(x=>x.TrUserApprovalId.Equals(id)).ToListAsync();
        }

        public Task<List<TrConditionUserApproval>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TrConditionUserApproval> Get(string id)
        {
            return await context.TrConditionUserApprovals.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
