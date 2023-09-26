using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ChartofAccount : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("ID COA")]
        public string CodeRec { get; set; }

        [DisplayName("COA Name Level 1")]
        public string Name1 { get; set; }

        [DisplayName("COA Name Level 2")]
        public string Name2 { get; set; }

        [DisplayName("COA Name Level 3")]
        public string Name3 { get; set; }

        [DisplayName("COA Name Level 4")]
        public string Name4 { get; set; }

        [DisplayName("COA Name Level 5")]
        public string Name5 { get; set; }

        [DisplayName("Level")]
        public string Level { get; set; }

        [DisplayName("Group")]
        public string Group { get; set; }

        [DisplayName("Condition")]
        public string Condition { get; set; }

        [DisplayName("Report")]
        public string Report { get; set; }

        [DisplayName("Reff Trx Code")]
        public string ReffTrxCode { get; set; }

        [DisplayName("Reff Trx Id")]
        public string ReffTrxId { get; set; }

        [DisplayName("Report")]
        public string ChartofaccountReportId { get; set; }

        //Not Mapped
        public string CodeRec1 { get; set; }
        public string CodeRec2 { get; set; }
        public string CodeRec3 { get; set; }
        public string CodeRec4 { get; set; }
    }
}