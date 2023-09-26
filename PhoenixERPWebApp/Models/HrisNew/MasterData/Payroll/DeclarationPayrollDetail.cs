using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class DeclarationPayrollDetail : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("DeclarationPayrollHeaderId")]
        public string DeclarationPayrollHeaderId { get; set; }
        [DisplayName("EmployeeBasicInfoId")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Value (Rp)")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double? Amount { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityId { get; set; }
        [DisplayName("Legal Entity")]
        public string LegalEntityName { get; set; }



    }
}