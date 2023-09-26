using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class TeamDto
    {
        [Required]
        public string Id { get; set; }

        public string organization_id { get; set; }

        public string team_leader { get; set; }
        [StringLength(400)]
        [Required]
        public string team_name { get; set; }

        public bool? IsJobDelete { get; set; }
        public bool? is_template { get; set; }
        public string job_id { get; set; }
    }
    public class TeamNew
    {

        public string organization_id { get; set; }
        [StringLength(50)]
        public string team_leader { get; set; }
        [StringLength(400)]
        [Required]
        public string team_name { get; set; }
        public string istemplate { get; set; }
    }
    public class TeamWithUserList : TeamDto
    {

        public List<UserList> userlist { get; set; }
    }
    public class TeamUsers
    {
        public string team_id { get; set; }
        public List<Teamlist> team { get; set; }
    }
    public class TeamJobUsers
    {
        public string job_id { get; set; }
        public List<Teamlist> team { get; set; }
    }
    public class Teamlist
    {
        public string user_id { get; set; }
        public string act_as { get; set; }
        public string job_detail_id { get; set; }
    }
    public class UserList
    {
        public string username { get; set; }
        public string email { get; set; }
        public string user_id { get; set; }
        public string act_as { get; set; }
        public string act_as_name { get; set; }
    }
    public class TeamSearch : SearchParameter
    {
        public string Search_organization_id { get; set; }
        public string Search_team_leader { get; set; }
        public string Search_team_name { get; set; }
        public string Search_job_id_is_null { get; set; }

    }
    public class UserlistSearch : SearchParameter
    {


    }
}