using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("education_level",Schema ="hr")]
    public partial class EducationLevel : GenericEntity<string>
    {
        [Column("education_name"), StringLength(100)]
        public string education_name { get; set; }

        [Column("education_seq")]
        public int education_seq { get; set; }
    }
}