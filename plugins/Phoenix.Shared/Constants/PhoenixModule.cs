using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Constants
{
    public class PhoenixModule
    {
        public const string API = "Api";
        public const string FINANCE = "Finance";
        public const string GENERAL = "General";
        public const string PM = "ProjectManagement";
        public const string HRIS = "Hris";
        public const string DEFAULT_ROUTE_API = API + "/[controller]/[action]";
        public const string FINANCE_ROUTE_API = API+"/" + PhoenixModule.FINANCE + "/[controller]/[action]";
        public const string GENERAL_ROUTE_API = API + "/" + PhoenixModule.GENERAL + "/[controller]/[action]";
        public const string PM_ROUTE_API = API + "/" + PhoenixModule.PM + "/[controller]/[action]";
        public const string HRIS_ROUTE_API = API + "/" + PhoenixModule.HRIS + "/[controller]/[action]";
    }
}
