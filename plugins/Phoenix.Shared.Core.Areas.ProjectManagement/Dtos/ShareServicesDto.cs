using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class ShareServicesDto
    {
        public string id { get; set; }
        public string shareservice_name { get; set; }
        public string detail_shareservices { get; set; }
    }
}
