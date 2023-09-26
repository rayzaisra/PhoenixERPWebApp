using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Demotion), Schema = "hr")]
    public class Demotion : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("effective_date")]
        public DateTime? EffectiveDate { get; set; }

        [MaxLength(50)]
        [Column("job_title_id"), StringLength(50)]
        public string JobTitleId { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [MaxLength(50)]
        [Column("job_level_id"), StringLength(50)]
        public string JobLevelId { get; set; }

        [Column(nameof(Notes)), StringLength(2147483647)]
        public string Notes { get; set; }

        [MaxLength(255)]
        [Column("warning_letter"), StringLength(255)]
        public string WarningLetter { get; set; }

        [MaxLength(50)]
        [Column(nameof(Requester)), StringLength(50)]
        public string Requester { get; set; }

        [MaxLength(50)]
        [Column("request_code"), StringLength(50)]
        public string RequestCode { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [NotMapped]
        public IFormFile fileWarningLetter { get; set; }

        [NotMapped]
        public string WarningLetterName { get; set; }

        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }

        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }
    }
}