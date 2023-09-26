using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrApprailsalM1Header : HrisEntityBase
    {

        public PdrApprailsalM1Header()
        {
            
        }

        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Employee Id")]
        public string EmployeeId { get; set; }

        [DisplayName("Review Start")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReviewStart { get; set; }

        [DisplayName("Review End")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReviewEnd { get; set; }

        //[NotMapped]
        public InfoEmployee infoEmployee { get; set; }
        public List<PdrApprailsalM1> PdrApprailsalM1s { get; set; }

        public int? Status { get; set; }
        public string StatusDesc { get; set; }
        public string RemarkRejected { get; set; }
        public bool IsApprove { get; set; }
    }
}