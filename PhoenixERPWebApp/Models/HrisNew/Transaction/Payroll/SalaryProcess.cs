using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryProcess : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Employement Status Id")]
        public string EmployementStatusId { get; set; }

        [DisplayName("Basic Salary")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? BasicSalary { get; set; }

        [DisplayName("Transport")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Transport { get; set; }

        [DisplayName("Loan")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Loan { get; set; }

        [DisplayName("Bonus")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Bonus { get; set; }

        [DisplayName("Bruto")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Bruto { get; set; }

        [DisplayName("Bruto Ofyears")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? BrutoOfyears { get; set; }

        [DisplayName("Position Cost")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PositionCost { get; set; }

        [DisplayName("Netto")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? Netto { get; set; }

        [DisplayName("Netto Ofyears")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? NettoOfyears { get; set; }

        [DisplayName("Ptkp Ofmonths")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PtkpOfmonths { get; set; }

        [DisplayName("Ptkp Ofyears")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PtkpOfyears { get; set; }

        [DisplayName("Pkp Ofyears")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PkpOfyears { get; set; }

        [DisplayName("Tax Ofyears")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TaxOfyears { get; set; }

        [DisplayName("Tax Ofmonths")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TaxOfmonths { get; set; }

        [DisplayName("Tax No Npwp")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TaxNoNpwp { get; set; }

        [DisplayName("Location")]
        public int? Location { get; set; }

        [DisplayName("Bpjs Tk Corp")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? BpjsTkCorp { get; set; }

        [DisplayName("Bpjs Tk Emp")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? BpjsTkEmp { get; set; }
        
        [DisplayName("Bpjs Ks Corp")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? BpjsKsCorp { get; set; }

        [DisplayName("Bpjs Ks Emp")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? BpjsKsEmp { get; set; }

        [DisplayName("Account No")]
        public string AccountNo { get; set; }

        [DisplayName("Salary Process Header Id")]
        public string SalaryProcessHeaderId { get; set; }

        [DisplayName("Total Day Ratio")]
        public int? TotalDayRatio { get; set; }

        [DisplayName("Total Work")]
        public int? TotalWork { get; set; }
    }
}