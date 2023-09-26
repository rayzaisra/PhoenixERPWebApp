using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMarvel.Infrastructure
{
    public static class AppConstants
    {
        public const string SessionKey = "PhoenixWebUserSession";
        public const string TokenSessionKey = "PhoenixAPITokenSession";
        public const string ServerTypeWeb = "Marvel-Web";
        public const string ServerTypeAPI = "Marvel-API";

        //session spesifik

        public const string EmployeeBasicInfoId = "PhoenixEmployeeBasicInfoIdSession";
        public const string BusinessUnitGroupId = "PhoenixBusinessUnitGroupIdSession";
        public const string BusinessUnitSubgroupId = "PhoenixBusinessUnitSubgroupIdSession";
        public const string BusinessUnitDivisiId = "PhoenixBusinessUnitDivisiIdSession";
        public const string BusinessUnitDepartementId = "PhoenixBusinessUnitDepartementIdSession";
        public const string JobTitleId = "PhoenixJobTitleIdSession";
    }
}
