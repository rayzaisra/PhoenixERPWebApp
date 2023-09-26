using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("business_unit_type",Schema ="dbo")]
    public partial class BusinessUnitType : GenericEntity<string>
    {
        public string business_unit_type_name { get; set; }
        public string organization_id { get; set; }
        public Int32 business_unit_level { get; set; }
    }
}