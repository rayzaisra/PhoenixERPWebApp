using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Phoenix.Data.Models.ViewModel
{
    [Table("vw_coa_invoice_client", Schema = "fn")]
    public class CoaInvoiceClient
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("code_rec")]
        public string CodeRec { get; set; }

        [Column("name1")]
        public string Name1 { get; set; }

        [Column("name2")]
        public string Name2 { get; set; }

        [Column("name3")]
        public string Name3 { get; set; }

        [Column("name4")]
        public string Name4 { get; set; }

        [Column("name5")]
        public string Name5 { get; set; }
    }
    
}
