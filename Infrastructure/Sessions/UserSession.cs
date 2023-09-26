using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.Sessions
{
    [Serializable]
    public class UserSession
    {
        public String AppUserId { get; set; }
        public String Username { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String BusinessUnitId { get; set; }
        public String LegalEntityId { get; set; }
        public String OrganizationID { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public String AppUserIPAddress { get; set; }
        public String Token { get; set; }
        public List<App_Role> AppRole { get; set; }
        public String Language { get; set; }
        public String EmpoyeeId { get; set; }
        public string EmployeeBasicInfoId { get; set; }

        public string BusinessUnitGroupId { get; set; }
        public string BusinessUnitSubgroupId { get; set; }
        public string BusinessUnitDivisiId { get; set; }
        public string BusinessUnitDepartementId { get; set; }
        public string JobTitleId { get; set; }

        public bool IsAdminProcurement
        {
            get
            {
                if (IsSystemAdministrator) return true;
                if (AppRole == null) return false;
                    return AppRole.Exists(o => o.Id ==
                    DefaultConfiguration.ApplicationRole.ADMIN_PROCUREMENT);
            }
        }

    }

    public class App_Role
    {
        public String Id { get; set; }
        public String RoleName { get; set; }
    }
}
