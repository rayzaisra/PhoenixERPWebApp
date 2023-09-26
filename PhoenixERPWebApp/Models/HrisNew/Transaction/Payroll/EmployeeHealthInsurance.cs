using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeeHealthInsurance : HrisEntityBase
    {
        [Required]
        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }
        
        [DisplayName("Endorsement")]
        public string Endorsement { get; set; }

        [DisplayName("Effective Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectiveDate { get; set; }

        [DisplayName("Plan Number")]
        public string PlanNumber { get; set; }

        [DisplayName("Insurance Company")]
        public string InsuranceCompanyId { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Inpatient")]
        public string Inpatient { get; set; }

        [DisplayName("Inpatient Premi")]
        public decimal? InpatientPremi { get; set; }

        [DisplayName("Outpatient")]
        public string Outpatient { get; set; }

        [DisplayName("Outpatient Premi")]
        public decimal? OutpatientPremi { get; set; }

        [DisplayName("Dental")]
        public string Dental { get; set; }

        [DisplayName("Dental Premi")]
        public decimal? DentalPremi { get; set; }

        [DisplayName("Maternity")]
        public string Maternity { get; set; }

        [DisplayName("Maternity Premi")]
        public decimal? MaternityPremi { get; set; }

        [DisplayName("Optical")]
        public string Optical { get; set; }

        [DisplayName("Optical Premi")]
        public decimal? OpticalPremi { get; set; }

        [DisplayName("Total Mutation")]
        public decimal? TotalMutation { get; set; }

        [DisplayName("Mutation Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? MutationDate { get; set; }
        public InfoEmployee infoEmployee { get; set; }      
        [NotMapped]
        public List<EmployeeHealthInsuranceFamily> EmployeeHealthInsuranceFamilys { get; set; }
    }
}