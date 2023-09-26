using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoenixERPWebApp.Models
{
    public class TaskRateCard
    {
        public decimal? price { get; set; }
        public string task_name { get; set; }
        public List<SubTaskRateCard> sub_Task { get; set; }
    }
}
