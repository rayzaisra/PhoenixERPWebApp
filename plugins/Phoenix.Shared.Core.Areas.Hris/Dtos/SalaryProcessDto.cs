using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class SalaryProcessDto
    {
        [Required]
            public string Id { get; set; }
        public int? basic_salary { get; set; }
        public int? bonus { get; set; }
        public int? bruto { get; set; }
        public int? bruto_ofyears { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        public string employement_status_id { get; set; }
        public int? loan { get; set; }
        public int? location { get; set; }
        public int? month { get; set; }
        public int? netto { get; set; }
        public int? netto_ofyears { get; set; }
        public int? pkp_ofyears { get; set; }
        public int? position_cost { get; set; }
        public int? ptkp_ofmonths { get; set; }
        public int? ptkp_ofyears { get; set; }
        [StringLength(50)]
        public string status_employee_id { get; set; }
        public int? tax_no_npwp { get; set; }
        public int? tax_ofmonths { get; set; }
        public int? tax_ofyears { get; set; }
        public int? transport { get; set; }
        public int? year { get; set; }
    }
    public class SalaryProcessNew
    {
        public int? basic_salary { get; set; }
        public int? bonus { get; set; }
        public int? bruto { get; set; }
        public int? bruto_ofyears { get; set; }
        [StringLength(50)]
        public string employee_basic_info_id { get; set; }
        [StringLength(50)]
        public string employement_status_id { get; set; }
        public int? loan { get; set; }
        public int? location { get; set; }
        public int? month { get; set; }
        public int? netto { get; set; }
        public int? netto_ofyears { get; set; }
        public int? pkp_ofyears { get; set; }
        public int? position_cost { get; set; }
        public int? ptkp_ofmonths { get; set; }
        public int? ptkp_ofyears { get; set; }
        [StringLength(50)]
        public string status_employee_id { get; set; }
        public int? tax_no_npwp { get; set; }
        public int? tax_ofmonths { get; set; }
        public int? tax_ofyears { get; set; }
        public int? transport { get; set; }
        public int? year { get; set; }
    }            
    public class SalaryProcessSearch : SearchParameter
    {
        public int? Search_basic_salary { get; set; }
        public int? Search_bonus { get; set; }
        public int? Search_bruto { get; set; }
        public int? Search_bruto_ofyears { get; set; }
        public string Search_employee_basic_info_id { get; set; }
        public string Search_employement_status_id { get; set; }
        public int? Search_loan { get; set; }
        public int? Search_location { get; set; }
        public int? Search_month { get; set; }
        public int? Search_netto { get; set; }
        public int? Search_netto_ofyears { get; set; }
        public int? Search_pkp_ofyears { get; set; }
        public int? Search_position_cost { get; set; }
        public int? Search_ptkp_ofmonths { get; set; }
        public int? Search_ptkp_ofyears { get; set; }
        public string Search_status_employee_id { get; set; }
        public int? Search_tax_no_npwp { get; set; }
        public int? Search_tax_ofmonths { get; set; }
        public int? Search_tax_ofyears { get; set; }
        public int? Search_transport { get; set; }
        public int? Search_year { get; set; }
        
    }
}