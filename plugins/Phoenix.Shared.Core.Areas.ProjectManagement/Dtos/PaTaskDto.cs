using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
    public class PaTaskDto
    {
        public string Id { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string central_resources_name { get; set; }
        public decimal? margin { get; set; }
        public string job_detail_id { get; set; }
        public decimal? total { get; set; }
        public bool status { get; set; }
    }
    public class PaTaskMargin
    {
 
        public string task_id { get; set; }
 
        public decimal? margin { get; set; }
  
    }
    public class PcaForm
    {

        public List<PaTaskMargin> Pcatask { get; set; }
        public string department { get; set; }
        public string currency { get; set; }
        public string feetype { get; set; }
        public decimal? fee_percentage { get; set; }
        public decimal? feetotal { get; set; }
        public int? hour_per_day { get; set; }
        public string job_detail_id { get; set; }
        public string mother_pca { get; set; }

        public DateTime? pa_date { get; set; }
        public string pa_number { get; set; }
        public string rate_card_id { get; set; }
    }
    public class Pataskexist:TaskDto
    {
        public string patask_taskname { get; set; }
    }
        public class PaTaskList:PaTaskDto
    {
        public List<PaDetailDto> PcaDetil { get; set; }
    }
    public class PaTaskNew
    {
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string central_resources_name { get; set; }
        public float margin { get; set; }
        public string job_detail_id { get; set; }
        public int status { get; set; }
    }            
    public class PaTaskSearch : SearchParameter
    {
        
    }
}