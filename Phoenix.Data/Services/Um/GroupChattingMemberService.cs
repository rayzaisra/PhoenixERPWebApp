using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class GroupChattingMemberService : IGroupChattingMemberService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public GroupChattingMemberService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(GroupChattingMember entity)
        {
            await context.PhoenixAddLiteAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params GroupChattingMember[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(GroupChattingMember entity)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteByGroupIdAsync(string groupId)
        {
            var commandText = "DELETE FROM dbo.GroupChattingMember WHERE GroupChattingId = @GroupId";
            var name = new SqlParameter("@GroupId", groupId);
            return context.Database.ExecuteSqlCommandAsync(commandText, name);
        }
        public Task<int> DeleteSoftByGroupIdAsync(string groupId)
        {
            var commandText = "UPDATE dbo.GroupChattingMember SET IsDeleted = 1 WHERE GroupChattingId = @GroupId";
            var name = new SqlParameter("@GroupId", groupId);
            return context.Database.ExecuteSqlCommandAsync(commandText, name);
        }
        public Task<int> DeleteRageAsync(params GroupChattingMember[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(GroupChattingMember entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params GroupChattingMember[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(GroupChattingMember entity)
        {
            context.PhoenixEditLite(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(GroupChattingMember entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<GroupChattingMember>> Get() => await context.GroupChattingMembers.AsNoTracking().ToListAsync();
        public async Task<List<GroupChattingMember>> GetMemberByGroupId(string id) => await context.GroupChattingMembers.Where(x=>x.GroupChattingId.Equals(id)).AsNoTracking().ToListAsync();

        public Task<List<GroupChattingMember>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<GroupChattingMember> Get(string Id) => await context.GroupChattingMembers.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
    }
}
