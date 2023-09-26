using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models
{
    [Serializable]
    public class Menu
    {
        public Menu()
        {
            Children = new List<Menu>();
        }

        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string MenuName { get; set; }
        public string MenuLink { get; set; }
        public string MenuUnique { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public int? MenuIndex { get; set; }
        public List<Menu> Children { get; set; }
    }

}
