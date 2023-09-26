using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmUserAccess", Schema = "Um")]
    public class TmUserAccess : EntityBaseUm
    {
        public string MenuId { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
        [NotMapped]
        public string MenuName { get; set; }
        [NotMapped]
        public string GroupName { get; set; }
        [NotMapped]
        public string ParentId { get; set; }
        [NotMapped]
        public string GroupMenuName { get; set; }
        public virtual TmMenu TmMenu { get; set; }
    }

    public class TmUserAccessViewModel
    {
        public string MenuId { get; set; }
        public bool IsRead { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string MenuName { get; set; }
        public string MenuUnique { get; set; }
        public string MenuLink { get; set; }
    }
}
