using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Shared.Core.Areas.General.Dtos
{
    public class UserLoginDto
    {
    }
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }


    }
    public class UserLoginData
    {
        public string Token { get; set; }
        public string MsGraphToken { get; set; }
        public string Name { get; set; }
        public List<RoleModule> Role { get; set; }
    }
    public class UserSort
    {
        public string Id { get; set; }
        public string Username { get; set; }

    }
    public class RoleModule
    {
        public string id { get; set; }
        public string Module { get; set; }
        public string ModuleIcon { get; set; }
        public List<RolePage> Page { get; set; }
    }
    public class RolePage
    {
        public string id { get; set; }
        public string Page { get; set; }
        public List<RoleButton> Button { get; set; }
    }
    public class RoleButton
    {
        public string Id { get; set; }
        public string Button { get; set; }
    }
}
