using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TrainingBondingDto
    {
        [Required]
            public string Id { get; set; }
        public string training_req_id { get; set; }
        public string training_name { get; set; }
        public string training_category { get; set; }
        public int? training_cost { get; set; }
        public string employee_basic_info_id { get; set; }
        public string employee_full_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? created_on { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string training_type { get; set; }
        public string traning_name { get; set; }
        public string venue { get; set; }
        public int? statusReal { get; set; }
        
        public DateTime? purpose_start_date { get; set; }
        public detialTrainingRequester detialTrainingRequester { get; set; }
        public List<participantTraining> ListParticipantTraining { get; set; }
        public List<TrUserApprovalDto> ApprovalReal { get; set; }
    }
    public class TrainingBondingNew
    {
		 public string Id { get; set; }
        public string training_req_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? created_on { get; set; }
    }            
    public class TrainingBondingSearch : SearchParameter
    {
        public string Search_name { get; set; }
        public string Search_category { get; set; }
        public string Search_employee_full_name { get; set; }
        
    }
}