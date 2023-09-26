using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure.DefaultConfiguration
{
    public static class ApplicationRole
    {
        public const String SYS_ADMIN = "00000000-0000-0000-0000-000000000001";
        public const String ADMIN_PROCUREMENT = "00000000-0000-0000-0000-100000000001";
        public const String ADMIN_FA = "00000000-0000-0000-0000-100000000002";
        public const String FRONT_DESK= "00000000-0000-0000-0000-100000000003";
        public const String USER = "00000000-0000-0000-0000-200000000001";

        public static Dictionary<string, string> GetDefaultRole() {
            var result = new Dictionary<string, string>();
            result.Add(SYS_ADMIN, "sys_admin");
            result.Add(ADMIN_PROCUREMENT, "Admin procurement");
            result.Add(ADMIN_FA,"Admin FA");
            result.Add(FRONT_DESK, "Frontdesk");
            result.Add(USER, "User");

            return result;
        }

        public static String GetRoleName(String id)
        {
            return GetDefaultRole()[id];
        }
    }
}
