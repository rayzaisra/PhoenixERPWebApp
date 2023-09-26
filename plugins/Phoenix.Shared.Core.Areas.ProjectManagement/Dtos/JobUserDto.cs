using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class JobUserDto
    {

        public string Id { get; set; }
        public string job_name { get; set; }
        public string user_id { get; set; }

    }
}
