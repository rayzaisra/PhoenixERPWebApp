using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("ChatInfo", Schema = "dbo")]
    public class ChatInfo
    {
        [Key, Required]
        [MaxLength(128)]
        [Column(nameof(Id)), StringLength(128)]
        public string Id { get; set; }


        [Required]
        [MaxLength(150)]
        [Column(nameof(MemberCode)), StringLength(150)]
        public string MemberCode { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(nameof(MemberName)), StringLength(150)]
        public string MemberName { get; set; }


        [Required]
        [MaxLength(150)]
        [Column(nameof(MemberToCode)), StringLength(150)]
        public string MemberToCode { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(nameof(MemberToName)), StringLength(150)]
        public string MemberToName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
