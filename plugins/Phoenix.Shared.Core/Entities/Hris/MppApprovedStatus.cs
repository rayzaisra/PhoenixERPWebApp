using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("mpp_approved_status",Schema ="hr")]
    public partial class MppApprovedStatus : GenericEntity<string>
    {
        public DateTime? created_date { get; set; }
        public string id_mpp { get; set; }
        public string reason { get; set; }
        public DateTime? updated_date { get; set; }
    }
}