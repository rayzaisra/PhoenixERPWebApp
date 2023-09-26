using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class HiringRequestApprovalDto
    {
        public string hiring_request_id { get; set; }
        public string reason { get; set; }
        public string Id { get; set; }
        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
        public string StatusApproved { get; set; }
    }
}
