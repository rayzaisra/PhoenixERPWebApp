using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("salary_process", Schema = "hr")]
    public class SalaryProcess : HrisEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(50)]
        [Column("employee_basic_info_id"), StringLength(50)]
        public string EmployeeBasicInfoId { get; set; }

        [MaxLength(50)]
        [Column("employement_status_id"), StringLength(50)]
        public string EmployementStatusId { get; set; }

        [Column("basic_salary")]
        public decimal? BasicSalary { get; set; }

        [Column(nameof(Transport))]
        public decimal? Transport { get; set; }

        [Column(nameof(Loan))]
        public decimal? Loan { get; set; }

        [Column(nameof(Bonus))]
        public decimal? Bonus { get; set; }

        [Column(nameof(Bruto))]
        public decimal? Bruto { get; set; }

        [Column("bruto_ofyears")]
        public decimal? BrutoOfyears { get; set; }

        [Column("position_cost")]
        public decimal? PositionCost { get; set; }

        [Column(nameof(Netto))]
        public decimal? Netto { get; set; }

        [Column("netto_ofyears")]
        public decimal? NettoOfyears { get; set; }

        [Column("ptkp_ofmonths")]
        public decimal? PtkpOfmonths { get; set; }

        [Column("ptkp_ofyears")]
        public decimal? PtkpOfyears { get; set; }

        [Column("pkp_ofyears")]
        public decimal? PkpOfyears { get; set; }

        [Column("tax_ofyears")]
        public decimal? TaxOfyears { get; set; }

        [Column("tax_ofmonths")]
        public decimal? TaxOfmonths { get; set; }

        [Column("tax_no_npwp")]
        public decimal? TaxNoNpwp { get; set; }

        [Column(nameof(Location))]
        public int? Location { get; set; }

        [Column("bpjs_tk_corp")]
        public decimal? BpjsTkCorp { get; set; }

        [Column("bpjs_tk_emp")]
        public decimal? BpjsTkEmp { get; set; }

        [Column("bpjs_ks_corp")]
        public decimal? BpjsKsCorp { get; set; }

        [Column("bpjs_ks_emp")]
        public decimal? BpjsKsEmp { get; set; }

        [MaxLength(100)]
        [Column("account_no"), StringLength(100)]
        public string AccountNo { get; set; }

        [MaxLength(50)]
        [Column("salary_process_header_id"), StringLength(50)]
        public string SalaryProcessHeaderId { get; set; }

        [Column("total_day_ratio")]
        public int? TotalDayRatio { get; set; }

        [Column("total_work")]
        public int? TotalWork { get; set; }
    }
}