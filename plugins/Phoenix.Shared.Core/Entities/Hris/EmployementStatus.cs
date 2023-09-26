using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("employement_status",Schema ="hr")]
    public partial class EmployementStatus : GenericEntity<string>
    {
        public string employment_status_name { get; set; }
    }
}