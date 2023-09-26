using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pca_refrence", Schema = "pm")]
    public partial class PcaRefrence: GenericEntity<string>
    {
        public string pca_id { get; set; }
        public string pca_refrence { get; set; }
    }
}
