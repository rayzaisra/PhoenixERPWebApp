using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities.Finance
{
    [Table("master_division", Schema = "fn")]
    public partial class MasterDivision :BaseEntity<string>
    {
        public string name { get; set; }
    }
}
