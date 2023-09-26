using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LeaveTransaction : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Leave Id")]
        public string LeaveId { get; set; }

        [DisplayName("Total")]
        public int Total { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }

        [DisplayName("Year")]
        public string Year { get; set; }

        [DisplayName("Taken Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TakenDate { get; set; }

        public Leave Leave { get; set; }
    }
}