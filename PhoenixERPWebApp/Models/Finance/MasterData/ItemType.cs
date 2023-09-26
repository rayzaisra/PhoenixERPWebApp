using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ItemType : FinanceEntityBase
    {
        [DisplayName("ID Item Type")]
        public string Id { get; set; }

        [DisplayName("Item Type Name")]
        public string TypeName { get; set; }

         public string IsFixedAsset { get; set; } 

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
