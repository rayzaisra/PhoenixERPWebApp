using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
     public class RateCardDto :RateCardNew
    {
        public string Id { get; set; }
        public string business_unit_name { get; set; }
        public string currency_name { get; set; }

    }
    public class RateCardNew
    {
        public string description { get; set; }
        public string code { get; set; }
        public string business_unit_id { get; set; }
        public List<RateCardTask> detail { get; set; }
        public string name { get; set; }
        public string revision { get; set; }
        public double? total { get; set; }
        public string currency { get; set; }
    }
    public class RateCardTask
    {
        public string id { get; set; }
        public long price { get; set; }
        public string task_name { get; set; }
        public List<RateCardSubTask> sub_task { get; set; }
    }
    public class RateCardSubTask
    {
        public string id { get; set; }
        public string brand { get; set; }
        public string category_id { get; set; }
        public string category_name { get; set; }
        public long price { get; set; }
        public int quantity { get; set; }
        public string remarks { get; set; }
        public string subbrand { get; set; }
        public string subtask { get; set; }

        public long total { get; set; }

    }
    public class RateCardSearch : SearchParameter
    {
 
        
    }
}