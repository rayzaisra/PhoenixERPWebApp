using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class StatusLog
    {
        [DisplayName("Transaction Id")]
        public string TransactionId { get; set; }

        [DisplayName("No")]
        public int? Seq { get; set; }

        [DisplayName("Employee Id")]
        public string EmployeeId { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Status Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? StatusDate { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Remarks")]
        public string Description { get; set; }
    }
}