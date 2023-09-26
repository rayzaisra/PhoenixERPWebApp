using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models
{
    [Table("chartofaccount_report", Schema = "fn")]
    public class ChartofaccountReport : FinanceEntityBase
    {
        [Key]
        [MaxLength(50)]
        [Column(nameof(Id)), StringLength(50)]
        public string Id { get; set; }

        [MaxLength(20)]
        [Column("report_name"), StringLength(20)]
        public string ReportName { get; set; }
    }
}