using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("pdr_employee", Schema = "hr")]
    public class PdrEmployee : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [Column("review_start")]
        public DateTime? ReviewStart { get; set; }

        [Column("review_end")]
        public DateTime? ReviewEnd { get; set; }

        [Column("date_appraisal")]
        public DateTime? DateAppraisal { get; set; }

        [Column("final_score")]
        public decimal? FinalScore { get; set; }

        [MaxLength(100)]
        [Column(nameof(Grade)), StringLength(100)]
        public string Grade { get; set; }

        [Column("date_submit")]
        public DateTime? DateSubmit { get; set; }

        [MaxLength(255)]
        [Column("file_result"), StringLength(255)]
        public string FileResult { get; set; }

        public int Year { get; set; }

        [Column("salary_increase")]
        public int? SalaryIncrease { get; set; }

        [MaxLength(50)]
        [Column("job_grade_id"), StringLength(50)]
        public string JobGradeId { get; set; }

        [Column(nameof(Notes)), StringLength(2147483647)]
        public string Notes { get; set; }

        [NotMapped]
        public PdrRecomendation PdrRecomendation { get; set; }
        [NotMapped]
        public List<PdrDetailM2below> PdrDetailM2belows { get; set; }
        [NotMapped]
        public List<PdrOverallScore> PdrOverallScores { get; set; }
        [NotMapped]
        public List<PdrDevelopmentPlan> PdrDevelopmentPlans { get; set; } 

        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }

        [NotMapped]
        public List<PdrCategory> PdrCategorys { get; set; }

        [NotMapped]
        public List<PdrGrade> PdrGrades { get; set; }

        [NotMapped]
        public int? Status { get; set; }

        [NotMapped]
        public string StatusDesc { get; set; }

        [NotMapped]
        public string RemarkRejected { get; set; }

    }
}