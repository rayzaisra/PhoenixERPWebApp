using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Filters
{
    public class UserSession
    {
        public String AppUserId { get; set; }
        public String Username { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String BusinessUnitId { get; set; }
        public String OrganizationID { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public String AppUserIPAddress { get; set; }
        public String Token { get; set; }
        public String Language { get; set; }

        public String EmailUser { get; set; }
        public String GroupAccess { get; set; }
        public String EmployeeId { get; set; }
        public String BusinessGroup { get; set; }
        public String BusinessSubGroup { get; set; }
        public String BusinessUnitDivisi { get; set; }
        public String BusinessUnitDepartment { get; set; }
        public String JobTitleId { get; set; }
        public String WindowsUserClaim { get; set; }

    }
}
