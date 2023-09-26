using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class AccountManagementDto
    {
        [Required]
            public string Id { get; set; }
        [StringLength(50)]
        public string user_id { get; set; }
    }
    public class AccountManagementNew
    {
        [StringLength(50)]
        public string user_id { get; set; }
    }
    public class AccountManagementList
    {

        public string Id { get; set; }
 
        public string app_username { get; set; }
        public string app_fullname { get; set; }
        public string user_id { get; set; }
    }
    public class AccountManagementWithTeamList: AccountManagementList
    {

        public string act_as { get; set; }
 
    }
    public class AccountManagementSearch : SearchParameter
    {
        public string Search_user_id { get; set; }
        public string Search_app_username { get; set; }


    }
}