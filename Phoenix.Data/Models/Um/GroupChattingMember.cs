using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("GroupChattingMember", Schema = "dbo")]
    public class GroupChattingMember
    {
        [Key, Required]
        [MaxLength(128)]
        [Column(nameof(Id)), StringLength(128)]
        public string Id { get; set; }


        [Required]
        [MaxLength(256)]
        [Column(nameof(MemberCode)), StringLength(256)]
        public string MemberCode { get; set; }

        [Required]
        [MaxLength(128)]
        [Column(nameof(GroupChattingId)), StringLength(128)]
        public string GroupChattingId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
