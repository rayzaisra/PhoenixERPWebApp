using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class SalaryProcessHeader : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Code")]
        public string Code { get; set; }

        [DisplayName("Month")]
        public int? Month { get; set; }

        [DisplayName("Year")]
        public int? Year { get; set; }

        [DisplayName("Is Tmp")]
        public bool? IsTmp { get; set; }

        //Not Mapped 
        public int? Status { get; set; }
        public string StatusDesc { get; set; }
        public string RemarkRejected { get; set; }
        public string RequestBy { get; set; }
        public bool IsApprove { get; set; }
        public List<SalaryProcess> SalaryProcesss { get; set; }
        public List<SalaryProcessComponent> SalaryProcessComponents { get; set; }
    }
}