using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Promotion), Schema = "hr")]
    public class Promotion : HrisEntityBase
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

        [Column(nameof(Skills)), StringLength(2147483647)]
        public string Skills { get; set; }

        [Column(nameof(Attitude)), StringLength(2147483647)]
        public string Attitude { get; set; }

        [Column(nameof(Performence)), StringLength(2147483647)]
        public string Performence { get; set; }

        [MaxLength(50)]
        [Column(nameof(Requester)), StringLength(50)]
        public string Requester { get; set; }

        [MaxLength(50)]
        [Column("request_code"), StringLength(50)]
        public string RequestCode { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [Column("date_of_latest")]
        public DateTime? DateOfLatest { get; set; }

        [Column("receive_warning_letter")]
        public bool? ReceiveWarningLetter { get; set; }

        [Column("position_on_org")]
        public bool? PositionOnOrg { get; set; }

        [Column(nameof(Notes)), StringLength(2147483647)]
        public string Notes { get; set; }

        [Column("increment_percentage")]
        public int? IncrementPercentage { get; set; }

        [Column("current_salary_zone")]
        public int? CurrentSalaryZone { get; set; }

        [Column("next_salary_zone")]
        public int? NextSalaryZone { get; set; }

        [Column("current_salary")]
        public decimal? CurrentSalary { get; set; }

        [Column("next_salary")]
        public decimal? NextSalary { get; set; }

        [Column("submit_user")]
        public bool? SubmitUser { get; set; }

        [Column("is_synchronize")]
        public bool? IsSynchronize { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }


        [NotMapped]
        public InfoEmployee requestEmployee { get; set; }

        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }

        [NotMapped]
        public List<PromotionSubcomponentAdditional> ComponentAdditionalList { get; set; }

        [NotMapped]
        public List<PromotionSubcomponentDeduction> ComponentDeductionList { get; set; }
    }
}