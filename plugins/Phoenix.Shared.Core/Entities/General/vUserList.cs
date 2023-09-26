using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("v_userlist",Schema ="pm")]
    public partial class vUserList :BaseEntity<string>
    {
        public string Email { get; set; }
        public string AliasName { get; set; }
        public string GroupId { get; set; }
        public string EmployeeBasicInfoId { get; set; }
        public bool? IsMsUser { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
       
        public string business_unit_group_id { get; set; }
        public string business_unit_subgroup_id { get; set; }
        public string business_unit_division_id { get; set; }
        public string business_unit_departement_id { get; set; }

    }
}