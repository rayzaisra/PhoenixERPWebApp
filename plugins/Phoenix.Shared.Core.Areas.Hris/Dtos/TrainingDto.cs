using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TrainingDto
    {
        [Required]
            public string Id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public int training_cost { get; set; }
        public string bonding_detail { get; set; }
		public string recruitment { get; set; }
		public string description { get; set; }
		public string target_audiance { get; set; }
        public DateTime? created_on { get; set; }
        public string training_type { get; set; }
    }
    public class TrainingNew
    {
		 public string Id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public int training_cost { get; set; }
        public string bonding_detail { get; set; }
		public string recruitment { get; set; }
		public string description { get; set; }
		public string target_audiance { get; set; }
        public DateTime? created_on { get; set; }
        public string training_type { get; set; }
    }            
    public class TrainingSearch : SearchParameter
    {
        public string Search_name { get; set; }
        public string Search_category { get; set; }
        public string Search_target_audience { get; set; }
        
    }
}