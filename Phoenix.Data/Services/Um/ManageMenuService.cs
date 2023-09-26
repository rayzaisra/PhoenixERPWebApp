using Microsoft.EntityFrameworkCore;
using Phoenix.Data.Models.Um;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phoenix.Data.Services.Um
{
    public class ManageMenuService : IManageMenuService
    {
        readonly DataContext context;

        public string UserId => context.GetUserId();

        /// <summary>
        /// And endpoint to manage SalaryComponent
        /// </summary>
        /// <param name="context">Database context</param>
        public ManageMenuService(DataContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(TmMenu entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await context.PhoenixAddAsync(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> AddRangeAsync(params TmMenu[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(TmMenu entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRageAsync(params TmMenu[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteSoftAsync(TmMenu entity)
        {
            context.PhoenixDelete(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteSoftRangeAsync(params TmMenu[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> EditAsync(TmMenu entity)
        {
            context.PhoenixEdit(entity);
            return await context.SaveChangesAsync();
        }

        public Task<int> EditRangeAsync(TmMenu entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmMenu>> GetBySearch(string key)
        {
            var parentList = (from x in context.TmMenus select new { x.ParentId, x.MenuName, x.Id }).ToList();
            var data = await (from m in context.TmMenus
                              select new TmMenu
                              {
                                  Id = m.Id,
                                  MenuName = m.MenuName,
                                  MenuLink = m.MenuLink,
                                  MenuUnique = m.MenuUnique,
                                  ParentId = m.ParentId,
                                  IsDeleted = m.IsDeleted
                              }).Where(x => Convert.ToBoolean(x.IsDeleted) == false && x.MenuName.Contains(key)).ToListAsync();
            if (parentList.Count == 0) return data;

            foreach (var item in data.ToList())
            {
                if (item.ParentId == "0") item.ParentName = "ROOT";
                else
                {
                    if (!string.IsNullOrEmpty(item.ParentId))
                    {
                        var parentName = parentList.Where(x => x.Id.Equals(item.ParentId)).FirstOrDefault();
                        if (parentName != null)
                            item.ParentName = parentName.MenuName;
                        else item.ParentName = "NOT FOUND PARENT";
                    }
                }
            }
            return data;
        }

        public async Task<List<TmMenu>> GetRaw()
        {
            return await context.TmMenus.AsNoTracking().Where(x => !(x.IsDeleted ?? false)).ToListAsync();
        }
        public async Task<TmMenu> GetLastIndex(string parentId)
        {
            var data = await context.TmMenus.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.ParentId.Equals(parentId)).ToListAsync();
            return data.OrderByDescending(x=>x.MenuIndex).First();
        }

        public async Task<TmMenu> GetByUniqueName(string uniqueName)
        {
            return await context.TmMenus.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.MenuUnique.Equals(uniqueName)).FirstOrDefaultAsync();
        }

        public async Task<List<TmMenu>> Get()
        {
			var list = await context.TmUserAccesses.AsNoTracking().Include(x => x.TmMenu)
                .Where(x => !(x.IsDeleted ?? false) && !(x.TmMenu.IsDeleted ?? false) && x.UserId.Equals(UserId))//UserId
				.Select(x => x.TmMenu)
                .OrderBy(x=>x.MenuIndex)
                .ToListAsync();

			list = list.Where(x => x != null).ToList();
            list.ForEach(i => i.Children = list.Where(ch => ch.ParentId == i.Id).Distinct().OrderBy(x=>x.MenuIndex).ToList());
            list = list.Where(x => x.ParentId == "0").Distinct().OrderBy(x => x.MenuIndex).ToList();
            return list;
            //return new HashSet<TmMenu>(list);
        }

        public Task<List<TmMenu>> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<TmMenu> Get(string id)
        {
            return await context.TmMenus.AsNoTracking().Where(x => !(x.IsDeleted ?? false) && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<TmMenu>> GetHierachyMenu()
        {
            var newListMenu = new List<TmMenu>();
            var listMenu = await context.TmMenus.ToListAsync();
            var listParentMenu = listMenu.Where(x => x.ParentId == "0" && x.IsDeleted == false).OrderBy(x => x.MenuIndex).ToList();
            var listSubMenu = listMenu.Where(x => x.ParentId != "0" && x.IsDeleted == false).OrderBy(x => x.MenuIndex).ToList();

            foreach (var item in listParentMenu)
            {
                var menuItem = new TmMenu
                {
                    Id = item.Id,
                    MenuName = item.MenuName,
                    ParentId = item.ParentId,
                    MenuLink = item.MenuLink,
                    MenuIndex = item.MenuIndex
                };
                menuItem.Children = GenerateTree(listSubMenu, item);
                newListMenu.Add(menuItem);
            }
            return newListMenu;
        }

        public List<TmMenu> GenerateTree(IEnumerable<TmMenu> collection, TmMenu rootItem)
        {
            var lst = new List<TmMenu>();
            foreach (TmMenu c in collection.Where(c => c.ParentId == rootItem.Id).OrderBy(x => x.MenuIndex))
            {
                lst.Add(new TmMenu
                {
                    Id = c.Id,
                    MenuName = c.MenuName,
                    ParentId = c.ParentId,
                    MenuLink = c.MenuLink,
                    MenuIndex = c.MenuIndex,
                    Children = GenerateTree(collection, c)
                });
            }
            return lst;
        }

        public async Task<TmMenu> GetMenuByParentIdAndIndex(string parentId, int idx)
        {
            return await context.TmMenus.Where(x => x.ParentId.Equals(parentId) && x.MenuIndex.Equals(idx)).FirstOrDefaultAsync();
        }

        public async Task<List<TmMenu>> GetListMenuByParentId(string parentId)
        {
            var newListMenu = new List<TmMenu>();
            var listParentMenu = await context.TmMenus.Where(x => x.ParentId == parentId && x.IsDeleted == false).OrderBy(x => x.MenuIndex).ToListAsync();
            var listSubMenu = await context.TmMenus.Where(x => x.ParentId != "0" && x.IsDeleted == false).OrderBy(x => x.MenuIndex).ToListAsync();

            foreach (var item in listParentMenu)
            {
                var menuItem = new TmMenu
                {
                    Id = item.Id,
                    MenuName = item.MenuName,
                    ParentId = item.ParentId,
                    MenuLink = item.MenuLink,
                    MenuIndex = item.MenuIndex
                };
                menuItem.Children = GenerateTree(listSubMenu, item);
                newListMenu.Add(menuItem);
            }
            return newListMenu;
        }

    }
}
