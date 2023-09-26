using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageGroupAccessService : IManageGroupAccessService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageGroupAccessService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmGroupAccess entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(params TmGroupAccess[] entities)
        {
            await context.PhoenixAddRangeAsync(entities).ConfigureAwait(false);
            return await context.SaveChangesAsync();
        }
        public Task<int> DeleteByGroupIdAsync(string groupId)
        {
            var commandText = "DELETE FROM Um.TmGroupAccess WHERE GroupId = @GroupId";
            var name = new SqlParameter("@GroupId", groupId);
            return context.Database.ExecuteSqlCommandAsync(commandText, name);
        }
        public Task<int> DeleteByUserIdAsync(string groupId,string userId)
        {
            var commandText = "DELETE FROM Um.TmGroupAccess WHERE GroupId = @GroupId AND UserId = @UserId";
            var name = new SqlParameter("@GroupId", groupId);
            var uid = new SqlParameter("@UserId", userId);
            return context.Database.ExecuteSqlCommandAsync(commandText, name, uid);
        }
        public Task<int> DeleteAsync(TmGroupAccess entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteRageAsync(params TmGroupAccess[] entities)
        {
            context.PhoenixDeleteRange(entities);
            return await context.SaveChangesAsync();
        }

        public async Task<int> DeleteSoftAsync(TmGroupAccess entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmGroupAccess[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmGroupAccess entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmGroupAccess entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmGroupAccess>> GetByGroupId(string groupId)
        {

            var data = await (from m in context.TmMenus.Where(x=>x.IsDeleted == false)
                        join ga in context.TmGroupAccesses.Where(x=>x.GroupId.Equals(groupId) && x.IsDeleted == false) on m.Id equals ga.MenuId into gm
                        from lga in gm.DefaultIfEmpty()
                        select new TmGroupAccess{
                            MenuId = m.Id,
                            ParentId = m.ParentId,
                            GroupId = groupId,
                            MenuName = m.MenuName,
                            IsRead = lga.IsRead == null ? false : true,
                            IsAdd = lga.IsAdd == null ? false : true,
                            IsEdit = lga.IsEdit == null ? false : true,
                            IsDelete = lga.IsDelete == null ? false : true,
                        }).ToListAsync();

            return data.ToList();
        }

        public async Task<List<GroupingMenuViewModel>> GetByGroupingMenu(string groupId)
        {
            List<GroupingMenuViewModel> lgm = new List<GroupingMenuViewModel>();
            var listMenu = context.TmMenus.Where(x=>x.IsDeleted == false).ToList();
            var listGroupAccess = await (from x in context.TmGroupAccesses.Where(x => x.GroupId.Equals(groupId) && x.IsDeleted == false)  
                                   select new TmGroupAccess
                                   {
                                       Id = x.Id,
                                       MenuId = x.MenuId,
                                       GroupId = x.GroupId,
                                       IsRead = x.IsRead == null ? false : true,
                                       IsAdd = x.IsAdd == null ? false : true,
                                       IsEdit = x.IsEdit == null ? false : true,
                                       IsDelete = x.IsDelete == null ? false : true,
                                       GroupMenuName = ""
                                   }).ToListAsync();

            var listGroupMenu = listMenu.Where(x => x.ParentId.Equals("0")).ToList();
            foreach (var item in listGroupMenu)
            {
                GenerateTree(listMenu, item, item.MenuName);
            }

            var newListGroupAccess = new List<TmGroupAccess>();
            foreach (var item in GroupingMenus.ToList())
            {
                var ga = new TmGroupAccess();
                var data = listGroupAccess.Where(x => x.MenuId.Equals(item.Id)).FirstOrDefault();
                if (data == null)
                {
                    ga.IsRead = false;
                    ga.IsAdd = false;
                    ga.IsEdit = false;
                    ga.IsDelete = false;
                }
                else
                {
                    ga.IsRead = data.IsRead;
                    ga.IsAdd = data.IsAdd;
                    ga.IsEdit = data.IsEdit;
                    ga.IsDelete = data.IsDelete;
                }
                ga.MenuId = item.Id;
                ga.ParentId = item.ParentId;
                ga.GroupId = groupId;
                ga.MenuName = item.MenuName;
                ga.GroupMenuName = item.GroupMenuName;
                newListGroupAccess.Add(ga);
            }

            foreach (var item in listGroupMenu)
            {
                var gm = new GroupingMenuViewModel
                {
                    GroupMenuName = item.MenuName,
                    MenuId = item.Id,
                    ListMenuGroup = newListGroupAccess.Where(x => x.GroupMenuName.Equals(item.MenuName)).ToList()
                };
                lgm.Add(gm);
            }

            return lgm.Where(x => x.ListMenuGroup.Count > 0).OrderBy(x=>x.GroupMenuName).ToList();
        }


        public async Task<List<TmGroupAccess>> Get() => await context.TmGroupAccesses.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
        public async Task<List<TmGroupAccess>> GetById(string groupId) => await context.TmGroupAccesses.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.GroupId.Equals(groupId)).ToListAsync();

        public Task<List<TmGroupAccess>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }



        public async Task<TmGroupAccess> Get(string Id) => await context.TmGroupAccesses.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();

        private List<TmMenu> GenerateTree(List<TmMenu> collection, TmMenu rootItem, string groupingMenuName)
        {
            List<TmMenu> lst = new List<TmMenu>();
            foreach (TmMenu c in collection.Where(c => c.ParentId == rootItem.Id))
            {
                lst.Add(new TmMenu
                {
                    Children = GenerateTree(collection, c, groupingMenuName)
                });
                c.GroupMenuName = groupingMenuName;
                GroupingMenus.Add(c);
            }
            return lst;
        }
        private List<TmMenu> GroupingMenus = new List<TmMenu>();
        #region Grouping LIst User By Group
        /// <summary>
        /// Kebutuhan yang baru .....Permintaan ADa Grouping LIst User By Group..
        /// </summary>
        /// <returns></returns>
        public async Task<List<MappingGroupUserViewModel>> GetListUserHavingGroup()
        {
            var listGroup = await (from x in context.TmGroups
                                   select new MappingGroupUserViewModel
                                   {
                                       Id = x.Id,
                                       GroupName = x.GroupName,
                                       ListUsers = null
                                   }).ToListAsync();

            var resultSet = ExecuteStoredProcedure<UserAppMappingGroupViewModel>("dbo.uspGetUserAppHavingGroup");

            foreach (var group in listGroup)
                group.ListUsers = resultSet.Where(x => x.GroupId.Equals(group.Id)).ToList();

            return listGroup.OrderBy(x=>x.GroupName).ToList();
        }

        /// <summary>
        /// Get User Apps not Have Mapping Group.....
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<EmployeeMappingDivisionViewModel> GetListUserNotHavingGroup(string key)
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Key",
                    SqlDbType.NVarChar) {Value = key}
            };
            var resultSet = ExecuteStoredProcedure<EmployeeMappingDivisionViewModel>("dbo.uspGetEmployeeAndUserAppNotMapGroup", sqlParameters);
            return resultSet;
        }
        #endregion

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
    }
}
