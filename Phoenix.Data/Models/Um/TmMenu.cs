using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmMenu", Schema = "Um")]
    public class TmMenu 
    {
        public TmMenu()
        {
            Children = new List<TmMenu>();
            TmUserAccesses = new List<TmUserAccess>();
            TmGroupAccesses = new List<TmGroupAccess>();
        }

        [Key, Required]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string MenuName { get; set; }
        public string MenuLink{ get; set; }
        public string MenuUnique { get; set; }
        public string ParentId { get; set; }
        public int? MenuIndex { get; set; }

        [NotMapped]
        public string GroupMenuName { get; set; }
        [NotMapped]
        public string ParentName { get; set; }

        [NotMapped]
        public List<TmMenu> Children { get; set; }

        public virtual List<TmUserAccess> TmUserAccesses { get; set; }

        public virtual List<TmGroupAccess> TmGroupAccesses { get; set; }
    }


}
