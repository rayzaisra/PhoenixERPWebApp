using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("applicant_progress",Schema ="hr")]
    public partial class ApplicantProgress : GenericEntity<string>
    {
        public string applicant_progress { get; set; }
    }
}