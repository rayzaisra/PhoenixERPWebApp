using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("departement",Schema ="hr")]
    public partial class Departement : GenericEntity<string>
    {
        public string departement_code { get; set; }
        public string departement_name { get; set; }
        public string divisionid { get; set; }
    }
}