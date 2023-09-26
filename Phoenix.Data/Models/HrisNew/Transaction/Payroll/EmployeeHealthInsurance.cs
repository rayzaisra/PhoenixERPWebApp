using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("employee_health_insurance", Schema = "hr")]
    public class EmployeeHealthInsurance : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(10)]
        [Column(nameof(Endorsement)), StringLength(10)]
        public string Endorsement { get; set; }

        [Column("effective_date")]
        public DateTime? EffectiveDate { get; set; }

        [MaxLength(30)]
        [Column("plan_number"), StringLength(30)]
        public string PlanNumber { get; set; }

        [MaxLength(50)]
        [Column("insurance_company_id"), StringLength(50)]
        public string InsuranceCompanyId { get; set; }

        [MaxLength(100)]
        [Column(nameof(Remarks)), StringLength(100)]
        public string Remarks { get; set; }

        [MaxLength(5)]
        [Column(nameof(Inpatient)), StringLength(5)]
        public string Inpatient { get; set; }

        [Column("inpatient_premi")]
        public decimal? InpatientPremi { get; set; }

        [MaxLength(5)]
        [Column(nameof(Outpatient)), StringLength(5)]
        public string Outpatient { get; set; }

        [Column("outpatient_premi")]
        public decimal? OutpatientPremi { get; set; }

        [MaxLength(5)]
        [Column(nameof(Dental)), StringLength(5)]
        public string Dental { get; set; }

        [Column("dental_premi")]
        public decimal? DentalPremi { get; set; }

        [MaxLength(5)]
        [Column(nameof(Maternity)), StringLength(5)]
        public string Maternity { get; set; }

        [Column("maternity_premi")]
        public decimal? MaternityPremi { get; set; }

        [MaxLength(5)]
        [Column(nameof(Optical)), StringLength(5)]
        public string Optical { get; set; }

        [Column("optical_premi")]
        public decimal? OpticalPremi { get; set; }

        [Column("total_mutation")]
        public decimal? TotalMutation { get; set; }

        [Column("mutation_date")]
        public DateTime? MutationDate { get; set; }

        [NotMapped]
        public InfoEmployee infoEmployee { get; set; }

        [NotMapped]
        public List<EmployeeHealthInsuranceFamily> EmployeeHealthInsuranceFamilys { get; set; }

    }
}