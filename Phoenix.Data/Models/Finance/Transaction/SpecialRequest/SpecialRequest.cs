using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.SpecialRequest
{
    [Table("special_request", Schema = "fn")]
    public class SpecialRequest : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
    
        [Column("special_name")]
        public string special_name { get; set; }

        [MaxLength(50)]
        [Column("organization_id"), StringLength(50)]
        public string organization_id { get; set; }

    }
}
