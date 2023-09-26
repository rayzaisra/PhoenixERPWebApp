using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageUserApprovalService : IManageUserApprovalService
    {
        readonly DataContext context;

        public string UserId => context.GetEmployeeId();

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageUserApprovalService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmTemplateUserApproval entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmTemplateUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmTemplateUserApproval entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmTemplateUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmTemplateUserApproval entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmTemplateUserApproval[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmTemplateUserApproval entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmTemplateUserApproval entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<TmTemplateUserApproval>> Get()
        {
            return await context.TmTemplateUserApprovals.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
        }
        public async Task<List<TmTemplateUserApproval>> GetByTemplateId(string id)
        {
            return await context.TmTemplateUserApprovals.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.TmTempApprovalId.Equals(id)).ToListAsync();
        }

        public Task<List<TmTemplateUserApproval>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmTemplateUserApproval> Get(string id)
        {
            return await context.TmTemplateUserApprovals.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Delete By Tempate Approval Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteByTemplateIdAsync(string idTemp,string empId)
        {
            await context.Database.ExecuteSqlCommandAsync("delete from Um.TmConditionUserApproval WHERE TemplateUserApprovalId IN (SELECT ID FROM  UM.TmTemplateUserApproval WHERE TmTempApprovalId = @ID AND EmployeeBasicInfoId = @EMPID)", new SqlParameter("@ID",idTemp), new SqlParameter("@EMPID", empId));
            return await context.Database.ExecuteSqlCommandAsync("UPDATE UM.TmTemplateUserApproval SET ISDELETED = 1 WHERE TmTempApprovalId = @ID AND EmployeeBasicInfoId = @EMPID", new SqlParameter("@ID", idTemp), new SqlParameter("@EMPID", empId));
        }
    }
}
