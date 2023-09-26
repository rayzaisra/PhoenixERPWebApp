using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("Doa", Schema = "Um")]
    public class Doa
    {
        [Key]
        [MaxLength(128)]
        [Column(nameof(Id)), StringLength(128)]
        public string Id { get; set; }

        [MaxLength(150)]
        [Column(nameof(AssignFrom)), StringLength(150)]
        public string AssignFrom { get; set; }

        [MaxLength(150)]
        [Column(nameof(AssignTo)), StringLength(150)]
        public string AssignTo { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsAssignToApproved { get; set; }

        [MaxLength(500)]
        [Column(nameof(ReasonDoa)), StringLength(500)]
        public string ReasonDoa { get; set; }

        [MaxLength(500)]
        [Column(nameof(ReasonRejectAssign)), StringLength(500)]
        public string ReasonRejectAssign { get; set; }

        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }

        [NotMapped]
        public string EmployeeName { get; set; }
        [NotMapped]
        public string FromDateToString
        {
            get {
                return Convert.ToDateTime(FromDate).ToString("dd-MM-yyyy");
            }
        }

        [NotMapped]
        public string ToDateToString
        {
            get
            {
                return Convert.ToDateTime(ToDate).ToString("dd-MM-yyyy");
            }
        }

    }
}