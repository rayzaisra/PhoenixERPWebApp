using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class MotherPcaDto : MotherPcaNew
    {
        public string Id { get; set; }
        public string business_unit_name { get; set; }
        public string currency_name { get; set; }

    }
    public class MotherPcaNew
    {
        public string description { get; set; }
        public string code { get; set; }
        public string business_unit_id { get; set; }
        public List<MotherPcaTask> detail { get; set; }
        public string name { get; set; }
        public string revision { get; set; }
        public double? total { get; set; }
        public string currency { get; set; }
    }
    public class MotherPcaTask
    {
        public string id { get; set; }
        public long price { get; set; }
        public string task_name { get; set; }
        public List<MotherPcaSubTask> sub_task { get; set; }
    }
    public class MotherPcaSubTask
    {
        public string id { get; set; }
     
        public long price { get; set; }
        public int quantity { get; set; }

        public string subtask { get; set; }

        public long total { get; set; }

    }
    public class MotherPcaSearch : SearchParameter
    {


    }

}