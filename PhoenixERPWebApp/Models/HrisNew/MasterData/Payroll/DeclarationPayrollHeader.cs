using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class DeclarationPayrollHeader : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Declaration Number")]
        public string Code { get; set; }

        [DisplayName("Month")]
        public int? Month { get; set; }
        [DisplayName("Month")]
        public string MonthName { get; set; }

        [DisplayName("Year")]
        public int? Year { get; set; }

        [DisplayName("IsTmp")]
        public bool? IsTmp { get; set; }

        [DisplayName("Declaration Type")]
        public string DeclarationType { get; set; }

        [DisplayName("SubComponentId")]
        public string SubComponentId { get; set; }

        [DisplayName("Component Name")]
        public string SubComponentName { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Status")]
        public int? Status { get; set; }

        [DisplayName("Status")]
        public string StatusName { get; set; }

        public bool IsApprove { get; set; }

        public string GroupId { get; set; }
        public string SubgroupId { get; set; }
        public string DivisionId { get; set; }
        public string DepartementId { get; set; }


    }

    public class DeclarationPayrollDto
    {
        public DeclarationPayrollHeader Header { get; set; }
        public List<DeclarationPayrollDetail> Details { get; set; }
    }
}