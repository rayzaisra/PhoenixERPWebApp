using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("type_of_expense", Schema = "pm")]
    public partial class TypeOfExpense : GenericEntity<string>
    {
        public string name { get; set; }
    }
}
