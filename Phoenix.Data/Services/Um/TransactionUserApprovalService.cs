using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class TransactionUserApprovalService : ITransactionUserApprovalService
    {
        readonly DataContext context;

        public string UserId => context.GetEmployeeId();


        public TransactionUserApprovalService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TrUserApproval entity)
        {
            await context.PhoenixAddApprovalAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TrUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TrUserApproval entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TrUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TrUserApproval entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TrUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TrUserApproval entity)
        {
            context.PhoenixEditApproval(entity);
            return await context.SaveChangesAsync();
        }
        public async Task<int> UpdateNextApproval(string templateAppId,int indexUser)
        {
            var commandText = "UPDATE Um.TrUserApproval SET StatusApproved = 1, CreatedOn = GETDATE() WHERE TrTempApprovalId = @TrTempApprovalId AND IndexUser = @IndexUser";
            var paramTempId = new SqlParameter("@TrTempApprovalId", templateAppId);
            var paramIndexUser = new SqlParameter("@IndexUser", indexUser);
            return await context.Database.ExecuteSqlCommandAsync(commandText, paramTempId,paramIndexUser);
        }
        public async Task<TrUserApproval> GetCurrentApproval(string approvalId)
        {
            return await context.TrUserApprovals.AsNoTracking().Where(x=>x.TrTempApprovalId.Equals(approvalId) && x.StatusApproved == StatusApproved.CurrentApproval).FirstOrDefaultAsync();
        }

        public Task<int> EditRangeAsync(TrUserApproval entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrUserApproval>> Get()
        {
             return await context.TrUserApprovals.AsNoTracking().ToListAsync();
        }
        public async Task<List<TrUserApproval>> GetByTemplateId(string id)
        {
            return await context.TrUserApprovals.AsNoTracking().Where(x => x.TrTempApprovalId.Equals(id)).ToListAsync();
        }
        public async Task<TrUserApproval> GetByTemplateSingleId(string id)
        {
            return await context.TrUserApprovals.AsNoTracking().Where(x => x.TrTempApprovalId.Equals(id)).FirstOrDefaultAsync();
        }

        public Task<List<TrUserApproval>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TrUserApproval> Get(string id)
        {
            return await context.TrUserApprovals.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<TrUserApproval> GetUserApprovalByTemplateIdAndEmpId(string trApprovalId,string empId)
        {
            return await context.TrUserApprovals.AsNoTracking().Where(x => x.TrTempApprovalId.Equals(trApprovalId) && x.EmployeeBasicInfoId.Equals(empId)).FirstOrDefaultAsync();
        }

        public async Task<List<AppIdConvert>> GetAppIdFromReferenceId(string referenceId) {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@RefId",
                    SqlDbType.NVarChar) {Value = referenceId}
            };
            SqlParameter[] param = sqlParameters.ToArray();
            var resultSet = await context.ExecuteStoredProcedure<AppIdConvert>("dbo.uspGetListUserApprovalByRefId", param);
            return resultSet;
        }
    }
}
