using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TmUserApp
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Alias Name")]
        public string AliasName { get; set; }

        [DisplayName("User Pwd")]
        public string UserPwd { get; set; }

        [DisplayName("Group Id")]
        public string GroupId { get; set; }

        [DisplayName("Employee Basic Info Id")]
        public string EmployeeBasicInfoId { get; set; }

        [DisplayName("Is Ms User")]
        public bool? IsMsUser { get; set; }

        [DisplayName("Is Deleted")]
        public bool? IsDeleted { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName("Is Active")]
        public bool? IsActive { get; set; }

        [DisplayName("Sub Group Id")]
        public string SubGroupId { get; set; }

        [DisplayName("Division Id")]
        public string DivisionId { get; set; }
    }
}