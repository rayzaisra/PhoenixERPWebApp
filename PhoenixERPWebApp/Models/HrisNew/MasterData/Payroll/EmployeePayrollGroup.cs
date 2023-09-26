using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class EmployeePayrollGroup : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Group Name")]
        public string GroupName { get; set; }

        public string GroupId { get; set; }

        public List<GroupSubcomponentAdditional> GroupSubcomponentAdditionals { get; set; }
        public List<GroupSubcomponentDeduction> GroupSubcomponentDeductions { get; set; }
    }
}