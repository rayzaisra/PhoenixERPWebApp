using Phoenix.Data.Models.Um;
using System.Collections.Generic;

namespace Phoenix.Data.Models.ViewModel
{
    public class GroupingMenuViewModel
    {
        public string MenuId { get; set; }
        public string GroupMenuName { get; set; }
        public List<TmGroupAccess> ListMenuGroup { get; set; }
    }

    public class GroupingMenuUserAccessViewModel
    {
        public string MenuId { get; set; }
        public string GroupMenuName { get; set; }
        public List<TmUserAccess> ListMenuGroup { get; set; }
    }
}
