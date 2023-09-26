using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Phoenix.Shared.Core.Entities
{
    [Table("request_for_quotation_task", Schema = "fn")]
    public partial class RequestForQuotationTask : GenericEntity<string>
    {
        public string request_for_quotation_id { get; set; }
        public string task { get; set; }
        public string subtask { get; set; }
        public int quantity { get; set; }
       
    }
}
