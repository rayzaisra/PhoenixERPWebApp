using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Phoenix.Data.Models.Um;
using Phoenix.Data.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageUserAccessService : IManageUserAccessService
    {
        readonly DataContext context;

        /// <summary>
        /// And endpoint to manage user access
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageUserAccessService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmUserAccess entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public async Task<int> AddRangeAsync(params TmUserAccess[] entities)
        {
            await context.PhoenixAddRangeAsync(entities).ConfigureAwait(false);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(TmUserAccess entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmUserAccess[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmUserAccess entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmUserAccess[] entities)
        {
            throw new NotImplementedException();
        }
        public Task<int> DeleteByUserIdAsync(string userId)
        {
            var commandText = "DELETE FROM Um.TmUserAccess WHERE UserId = @UserId";
            var name = new SqlParameter("@UserId", userId);
            return context.Database.ExecuteSqlCommandAsync(commandText, name);
        }

        public async Task<int> EditAsync(TmUserAccess entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmUserAccess entity)
        {
            throw new NotImplementedException();
        }
        public async Task<List<TmUserAccess>> GetListByUserId(string userId, string groupId)
        {
            var paramGroupId = new SqlParameter("@GroupId", groupId);
            var paramUserId = new SqlParameter("@UserId", userId);
            var data = await context.TmUserAccesses.FromSql("EXEC uspGetUserAccess @GroupId, @UserId", paramGroupId, paramUserId).AsNoTracking().ToListAsync();
            var listMenu = new List<TmUserAccess>();
            foreach (var u in data.ToList())
            {
                listMenu.Add(new TmUserAccess
                {
                    Id = u.Id,
                    MenuId = u.MenuId,
                    UserId = u.UserId,
                    GroupId = u.GroupId,
                    IsRead = u.IsRead,
                    IsAdd = u.IsAdd,
                    IsEdit = u.IsEdit,
                    IsDelete = u.IsDelete,
                    MenuName = u.CreatedBy,
                    ParentId = u.ModifiedBy
                });
            }
            return listMenu;
        }

        public async Task<List<TmUserAccess>> Get() => await context.TmUserAccesses.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();

        public Task<List<TmUserAccess>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmUserAccess> Get(string Id) => await context.TmUserAccesses.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == Id).AsNoTracking().FirstOrDefaultAsync();
        public async Task<TmUserAccess> GetDataByUserId(string userId) => await context.TmUserAccesses.AsNoTracking().Where(x => x.UserId == userId).AsNoTracking().FirstOrDefaultAsync();

        public async Task<TmUserAccessViewModel> GetById(string menuId, string userId = "")
        {
            if (string.IsNullOrWhiteSpace(userId))
                userId = context.GetUserId();

            var a = await context.TmUserAccesses.Include(x => x.TmMenu).AsNoTracking()
                .Where(x => x.MenuId.Equals(menuId) && x.UserId == userId)
                .Select(x => new TmUserAccessViewModel
                {
                    MenuId = x.MenuId,
                    IsAdd = x.IsAdd ?? false,
                    IsDelete = x.IsDelete ?? false,
                    IsEdit = x.IsEdit ?? false,
                    IsRead = x.IsRead ?? false,
                    MenuName = x.TmMenu.MenuName,
                    MenuUnique = x.TmMenu.MenuUnique,
                    MenuLink = x.TmMenu.MenuLink
                }).FirstOrDefaultAsync();

            return a;
        }


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
        public async Task<List<GroupingMenuUserAccessViewModel>> GetUserAccessByGroupingMenu(string groupId, string userId)
        {
            List<GroupingMenuUserAccessViewModel> lgm = new List<GroupingMenuUserAccessViewModel>();
            var listMenu = (from m in context.TmMenus.Where(x => x.IsDeleted == false)
                            join g in context.TmGroupAccesses.Where(x => x.GroupId.Equals(groupId) && x.IsDeleted == false).ToList()
                            on m.Id equals g.MenuId
                            select new TmMenu
                            {
                                Id = m.Id,
                                MenuName = m.MenuName,
                                ParentId = m.ParentId,
                                GroupMenuName = ""
                            }).ToList();

            var listUserAccess = await (from x in context.TmUserAccesses.Where(x => x.GroupId.Equals(groupId) && x.UserId.Equals(userId) && x.IsDeleted.Equals(false))
                                        select new TmUserAccess
                                        {
                                            Id = x.Id,
                                            MenuId = x.MenuId,
                                            GroupId = x.GroupId,
                                            UserId = userId,
                                            IsRead = x.IsRead == null ? false : x.IsRead,
                                            IsAdd = x.IsAdd == null ? false : x.IsAdd,
                                            IsEdit = x.IsEdit == null ? false : x.IsEdit,
                                            IsDelete = x.IsDelete == null ? false : x.IsEdit,
                                            GroupMenuName = ""
                                        }).ToListAsync();

            var listGroupMenu = listMenu.Where(x => x.ParentId.Equals("0")).ToList();
            foreach (var item in listGroupMenu)
            {
                GenerateTree(listMenu, item, item.MenuName);
            }

            var newListUserAccess = new List<TmUserAccess>();
            foreach (var item in GroupingMenus.ToList())
            {
                var ga = new TmUserAccess();
                var data = listUserAccess.Where(x => x.MenuId.Equals(item.Id)).FirstOrDefault();
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
                ga.UserId = userId;
                ga.MenuId = item.Id;
                ga.ParentId = item.ParentId;
                ga.GroupId = groupId;
                ga.MenuName = item.MenuName;
                ga.GroupMenuName = item.GroupMenuName;
                newListUserAccess.Add(ga);
            }

            foreach (var item in listGroupMenu)
            {
                var gm = new GroupingMenuUserAccessViewModel
                {
                    GroupMenuName = item.MenuName,
                    MenuId = item.Id,
                    ListMenuGroup = newListUserAccess.Where(x => x.GroupMenuName.Equals(item.MenuName)).ToList()
                };
                lgm.Add(gm);
            }

            return lgm.Where(x => x.ListMenuGroup.Count > 0).OrderBy(x => x.GroupMenuName).ToList();
        }

        public async Task<TmUserAccessViewModel> GetByContoller(string areaName, string controllerName)
        {
            var result = await (from a in context.TmUserAccesses
                                join b in context.TmAuthorizations on a.MenuId equals b.MenuId
                                join c in context.TmMenus on a.MenuId equals c.Id
                                where a.UserId == context.GetUserId() && b.AreaName == areaName && b.ControllerName == controllerName
                                select new TmUserAccessViewModel
                                {
                                    MenuId = a.MenuId,
                                    IsAdd = a.IsAdd ?? false,
                                    IsDelete = a.IsDelete ?? false,
                                    IsEdit = a.IsEdit ?? false,
                                    IsRead = a.IsRead ?? false,
                                    MenuName = c.MenuName,
                                    MenuUnique = c.MenuUnique,
                                    MenuLink = c.MenuLink
                                }).FirstOrDefaultAsync();

            return result;
        }
    }
}
