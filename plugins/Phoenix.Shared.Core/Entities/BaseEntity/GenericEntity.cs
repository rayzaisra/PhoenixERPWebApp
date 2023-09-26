using Phoenix.Shared.Core.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Phoenix.Shared.Core.Entities
{
    public class GenericEntity<PK> :BaseEntity<PK>,ISoftDeletable
    {


        public string created_by { get; set; }
        public DateTime? created_on { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_on { get; set; }
        public string approved_by { get; set; }
        public DateTime? approved_on { get; set; }

        public string deleted_by { get; set; }
        public DateTime? deleted_on { get; set; }
        public bool? is_active { get; set; }
        public bool? is_locked { get; set; }
        public bool? is_default { get; set; }
        public bool? is_deleted { get; set; }
        public string owner_id { get; set; }
    }

 
}
