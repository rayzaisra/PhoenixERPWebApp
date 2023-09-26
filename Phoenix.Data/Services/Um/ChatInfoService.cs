using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ChatInfoService : IChatInfoService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ChatInfoService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(ChatInfo entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddLiteAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params ChatInfo[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(ChatInfo entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params ChatInfo[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(ChatInfo entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params ChatInfo[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(ChatInfo entity)
        {
            context.PhoenixEditLite(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(ChatInfo entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ChatInfo>> Get() => await context.ChatInfos.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<ChatInfo>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<ChatInfo> Get(string Id) => await context.ChatInfos.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<List<ChatInfo>> GetListContact(string memberCode) => await context.ChatInfos.Where(x => !(x.IsDeleted ?? false) && x.MemberCode == memberCode).AsNoTracking().ToListAsync();
        public async Task<List<ChattingAndComment>> GetListChat(string code) => await context.ChattingAndComments.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.CollaborateCode.Equals(code)).ToListAsync();
        public async Task<List<ChattingAndComment>> GetListChatTeam(string collaborateCode)=> await context.ChattingAndComments.Where(x=>x.CollaborateCode.Equals(collaborateCode)).OrderBy(x=>x.CreatedOn).ToListAsync();
        public async Task<int> AddChatAndCommentAsync(ChattingAndComment entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddLiteAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<string> AddChatInfo(ContactMemberChatVm entity)
        {
            var isReady = await context.ChatInfos.AsNoTracking()
                                .Where(c => c.MemberCode.Equals(entity.MemberCode) && c.MemberToCode.Equals(entity.MemberToCode)).FirstOrDefaultAsync();
            if (isReady == null)
            {
                // add data for From Member...
                var model = new ChatInfo();
                var codex = Guid.NewGuid().ToString();
                model.Id = codex;
                model.MemberCode = entity.MemberCode;
                model.MemberName = entity.MemberName;
                model.MemberToCode = entity.MemberToCode;
                model.MemberToName = entity.MemberToName;
                await context.PhoenixAddLiteAsync(model);
                await context.SaveChangesAsync();

                //add data for To Member
                model = new ChatInfo();
                model.Id = Guid.NewGuid().ToString();
                model.MemberCode = entity.MemberToCode;
                model.MemberName = entity.MemberToName;
                model.MemberToCode = entity.MemberCode;
                model.MemberToName = entity.MemberName;
                await context.PhoenixAddLiteAsync(model);
                await context.SaveChangesAsync();

                return codex;
            }
            else
            {
                return isReady.Id;
            }
        }

        public List<TeamInfoVm> GetListTeamByMember(string memberCode)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@MemberCode",
                    SqlDbType.NVarChar) {Value = memberCode}
            };
            var resultSet = ExecuteStoredProcedure<TeamInfoVm>("dbo.uspGetListTeamByMember", sqlParameters);
            return resultSet;
        }
        public List<ListTeamMember> GetListTeamMemberByJobId(string jobId)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@JobId", SqlDbType.NVarChar) {Value = jobId}
            };
            var resultSet = ExecuteStoredProcedure<ListTeamMember>("dbo.uspGetTeamMemberByJobId", sqlParameters);
            return resultSet;
        }

        #region Function Execute Stored Procedure
        public List<T> ExecuteStoredProcedure<T>(string storedProcedure, List<SqlParameter> parameters) where T : new()
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;

                // set some parameters of the stored procedure
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {
                    var test = DataReaderMapToList<T>(dataReader);
                    return test;
                }
            }
        }
        public List<T> ExecuteStoredProcedure<T>(string storedProcedure) where T : new()
        {
            using (var cmd = context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = storedProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {
                    var test = DataReaderMapToList<T>(dataReader);
                    return test;
                }
            }
        }
        private static List<T> DataReaderMapToList<T>(DbDataReader dr)
        {
            List<T> list = new List<T>();
            while (dr.Read())
            {
                var obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        #endregion
    }
}
