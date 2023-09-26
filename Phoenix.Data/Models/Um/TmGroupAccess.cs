using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmGroupAccess", Schema = "Um")]
    public class TmGroupAccess : EntityBaseUm
    {
        public string MenuId { get; set; }
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
}
