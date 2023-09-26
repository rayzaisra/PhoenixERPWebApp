using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ExitInterview : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }
        public string name { get; set; }
        [DisplayName("Termination Id")]
        public string TerminationId { get; set; }

        [DisplayName("Category Exit Interview Id")]
        public string CategoryExitInterviewId { get; set; }

        [DisplayName(" Exit Interview Item Id")]
        public string ExitInterviewItemId { get; set; }

        [DisplayName("Result")]
        public string Result { get; set; }

        [DisplayName("Text Item")]
        public string TextItem { get; set; }

        [DisplayName("Type")]
        public int Type { get; set; }

        [DisplayName("No")]
        public int? No { get; set; }
        public Termination Termination { get; set; }
    }
}