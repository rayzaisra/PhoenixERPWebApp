using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Shared.Core.Areas.General.Dtos.Config
{
    public class MsGraphSetting
    {
        public bool useMsLogin { get; set; }
        public string clientId { get; set; }
        public string tenantId { get; set; }
        public string clientSecret { get; set; }
        public string objectId { get; set; }
        public string office365Username { get; set; }
        public string office365Password { get; set; }
    }
}
