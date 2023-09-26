using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.ProjectManagement.Dtos
{
     public class PceDto :PceNew
    {
        public string Id { get; set; }
        public string job_name { get; set; }
        public string currency_name { get; set; }
        public string mother_pca_name { get; set; }
        public string businesss_unit_name { get; set; }
        public string rate_card_name { get; set; }
        public string brand_name { get; set; }
        public string job_status { get; set; }
        public string company_name { get; set; }
        public DateTime? created_date { get; set; }
        public string pca_code { get; set; }
    }
    public class PceNew
    {
        public string description { get; set; }
        public string code { get; set; }
        public string job_id { get; set; }
        public List<PceTaskDto> detail { get; set; }
        public string name { get; set; }
        public string revision { get; set; }
        public decimal? total { get; set; }
        public string currency { get; set; }
        public string pca_id { get; set; }
        public string mother_pca_id { get; set; }
        public string rate_card_id { get; set; }
        public decimal? subtotal { get; set; }
        public decimal? other_fee { get; set; }
        public string other_fee_name { get; set; }
        public decimal? other_fee_percentage { get; set; }
        public decimal? vat { get; set; }
        public string status { get; set; }

    }
    public class PceTaskDto
    {
        public string id { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }

        public decimal? price { get; set; }
        public decimal? total { get; set; }
        public string task_name { get; set; }
        public List<PceSubTask> sub_task { get; set; }
    }
    public class PceSubTask
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
    public class PceSearch : SearchParameter
    {
 
        
    }
}