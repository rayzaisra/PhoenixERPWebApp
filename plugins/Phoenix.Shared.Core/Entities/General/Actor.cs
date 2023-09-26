using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("actor",Schema ="pm")]
    public partial class Actor
    {
        public string id { get; set; }
        public string name { get; set; }
        public string extendata { get; set; }
        public int  level  { get; set; }
    }
}