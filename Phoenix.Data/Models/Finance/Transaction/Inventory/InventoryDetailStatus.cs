using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.Finance.Transaction.Inventory
{

    [Table("inventory_detail_status", Schema = "fn")]
    public class InventoryDetailStatus : FinanceEntityBase
    {

        [Column("id")]
        public string Id { get; set; }

        [Column("get_form")]
        public int getForm { get; set; }

        [Column("status_name")]
        public string statusName { get; set; }
    }
}
