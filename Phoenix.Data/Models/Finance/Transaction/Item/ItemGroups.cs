using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Finance.Transaction.Item
{
   public  class ItemGroups : FinanceEntityBase
    {
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }
        [Column("organization_id")]  //,<, nvarchar(50),>
        public string OrganizationId { get; set; }
        [Column("group_name")]  //    ,<, nvarchar(400),>)
        public string GroupName { get; set; }
    }
}
