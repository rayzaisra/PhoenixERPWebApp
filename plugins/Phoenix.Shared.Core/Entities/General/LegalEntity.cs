using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("legal_entity",Schema ="dbo")]
    public partial class LegalEntity : GenericEntity<string>
    {
        public string legal_entity_name { get; set; }
       
    }
}