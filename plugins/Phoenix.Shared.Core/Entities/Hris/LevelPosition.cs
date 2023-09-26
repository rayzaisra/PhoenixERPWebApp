using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("level_position",Schema ="hr")]
    public partial class LevelPosition : GenericEntity<string>
    {
        public string position_level { get; set; }
    }
}