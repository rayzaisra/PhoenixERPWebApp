using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table(nameof(Rotation), Schema = "hr")]
    public class Rotation : HrisEntityBase
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
        [Column(nameof(Requester)), StringLength(50)]
        public string Requester { get; set; }

        [MaxLength(50)]
        [Column("request_code"), StringLength(50)]
        public string RequestCode { get; set; }

        [Column("request_date")]
        public DateTime? RequestDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        [Column("subgroup_id"), StringLength(50)]
        public string RotationSubgroupId { get; set; }

        [MaxLength(50)]
        [Column("division_id"), StringLength(50)]
        public string RotationDivisionId { get; set; }

        [MaxLength(50)]
        [Column("department_id"), StringLength(50)]
        public string RotationDepartmentId { get; set; }

        [MaxLength(50)]
        [Column("job_level_id"), StringLength(50)]
        public string JobLevelId { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [MaxLength(50)]
        [Column("job_title_id"), StringLength(50)]
        public string JobTitleId { get; set; }

        [MaxLength(50)]
        [Column("location_id"), StringLength(50)]
        public string LocationId { get; set; }

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
        public List<RotationSubcomponentAdditional> ComponentAdditionalList { get; set; }

        [NotMapped]
        public List<RotationSubcomponentDeduction> ComponentDeductionList { get; set; }
    }
}