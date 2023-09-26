using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class GroupChattingService : IGroupChattingService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public GroupChattingService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(GroupChatting entity)
        {
            await context.PhoenixAddLiteAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params GroupChatting[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(GroupChatting entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params GroupChatting[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(GroupChatting entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params GroupChatting[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(GroupChatting entity)
        {
            context.PhoenixEditLite(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(GroupChatting entity)
        {
            throw new NotImplementedException();
        }


        public async Task<List<GroupChatting>> Get() => await context.GroupChattings.AsNoTracking().ToListAsync();
        public async Task<List<GroupChatting>> GetBySearch(string key) => await context.GroupChattings.Where(x=>x.GroupName.Contains(key)).AsNoTracking().ToListAsync();

        public Task<List<GroupChatting>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<GroupChatting> Get(string Id) => await context.GroupChattings.AsNoTracking().Where(x => x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<GroupChatVm> GetByGroupId(string id)
        {
            var data = await (from x in context.GroupChattings.Where(x=>x.Id.Equals(id))
                              select new GroupChatVm
                              {
                                  Id = x.Id,
                                  GroupName = x.GroupName,
                                  Members = null
                              }).FirstOrDefaultAsync();

            data.Members = (from d in context.GroupChattingMembers.Where(c => c.GroupChattingId.Equals(id))
                            join u in context.TmUserApps
                            on d.MemberCode equals u.Id
                            select new GroupChatMemberVm
                            {
                                GroupId = id,
                                MemberCode = d.MemberCode,
                                MemberName = u.AliasName
                            }).OrderBy(c=>c.MemberName).ToList();

            return data;
        }

        public async Task<List<GroupChatting>> GetGroupChatByMemberCode(string memberCode)
        {
            var data = await (from x in context.GroupChattings
                              join m in context.GroupChattingMembers.Where(c=>c.MemberCode.Equals(memberCode)) on x.Id equals m.GroupChattingId
                              select new GroupChatting
                              {
                                  Id = x.Id,
                                  GroupName = x.GroupName
                              }).ToListAsync();
            return data;
        }

    }
}
