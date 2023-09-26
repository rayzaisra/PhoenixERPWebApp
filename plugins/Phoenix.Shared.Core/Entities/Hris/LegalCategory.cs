using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("legal_category",Schema ="hr")]
    public partial class LegalCategory : GenericEntity<string>
    {
        public string category { get; set; }
        public string description { get; set; }
    }
}