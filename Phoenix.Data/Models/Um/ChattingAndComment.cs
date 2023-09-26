using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("ChattAndComment", Schema = "dbo")]
    public class ChattingAndComment
    {
        [Key, Required]
        [MaxLength(128)]
        [Column(nameof(Id)), StringLength(128)]
        public string Id { get; set; }

        [Required]
        [Column(nameof(ChatText))]
        public string ChatText { get; set; }


        [MaxLength(25)]
        [Column(nameof(FileExt)), StringLength(25)]
        public string FileExt { get; set; }

        /// <summary>
        /// JIka User To User = Combine UserAppId From & TO,
        /// JIka User in Project = Kode Project nya....
        /// </summary>
        [Required]
        [MaxLength(256)]
        [Column(nameof(CollaborateCode)), StringLength(256)]
        public string CollaborateCode { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(nameof(MemberCode)), StringLength(150)]
        public string MemberCode { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(nameof(MemberName)), StringLength(150)]
        public string MemberName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }

        public string CreatedDateString { get { return Convert.ToDateTime(CreatedOn).ToString("dd-MM-yyyy hh:mm:ss tt"); } }
    }
}
