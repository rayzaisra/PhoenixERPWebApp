using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Entities
{
    [Table("pettys_cashs",Schema ="pm")]
    public partial class PettyCashs : GenericEntity<string>
    {
        //public int? pettyyy_amount { get; set; }
        //public DateTime? petty_date { get; set; }
        //public string task_id { get; set; }
        //public string transfer_bank_accname { get; set; }
        //public string transfer_bank_accno { get; set; }
        //public string transfer_status_id { get; set; }
    }
}