using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("province",Schema ="hr")]
    public partial class Province : GenericEntity<string>
    {
        public int? code { get; set; }
        public string province { get; set; }
    }
}