using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_apprailsal_m1_header", Schema = "hr")]
    public class PdrApprailsalM1Header : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_id"), StringLength(50)]
        public string EmployeeId { get; set; }

        [Column("review_start")]
        public DateTime? ReviewStart { get; set; }

        [Column("review_end")]
        public DateTime? ReviewEnd { get; set; }

        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }

        [NotMapped]
        public List<PdrApprailsalM1> PdrApprailsalM1s { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }
    }
}