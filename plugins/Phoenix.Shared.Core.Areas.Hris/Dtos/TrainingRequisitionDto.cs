using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Phoenix.Shared.Core.Areas.General.Dtos;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TrainingRequisitionDto
    {
        [Required]
        public string Id { get; set; }
        public string training_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? purpose_start_date { get; set; }
        public DateTime? purpose_end_date { get; set; }
        public string venue { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string training_type {get; set;}
        public string training_category { get; set; }
        public string traning_name { get; set; }
        public DateTime? created_on { get; set; }
        public Int32 training_cost { get; set; }
        public int? statusReal { get; set; }
        public detialTrainingRequester detialTrainingRequester { get; set; }
        public List<participantTraining> ListParticipantTraining { get; set; }
        public List<TrUserApprovalDto> ApprovalReal { get; set; }
        public string add_recomendation { get; set; }


    }

    public class StatusTrainingReal {
           public string StatusApproved { get; set; }
    }

    public class participantTraining {
        public string Id { get; set; }
        public string employee_basic_info_id { get; set; }
        public string name_employee { get; set; }
        public string job_title_name { get; set; }
        public string job_grade_name { get; set; }
        public participantBusinessUnit participantBusinessUnit { get; set; }

    }

    public class participantBusinessUnit {
        public string Id { get; set; }
        public string name { get; set; }
    }

    public class detialTrainingRequester {
        public string request_by { get; set; }
        public string job_title_name { get; set; }
        public string job_grade_name { get; set; }

    }

    public class TrainingRequisitionNew
    {
        public string Id { get; set; }
        public string training_id { get; set; }
        public string employee_basic_info_id { get; set; }
        public DateTime? purpose_start_date { get; set; }
        public DateTime? purpose_end_date { get; set; }
        public string venue { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string training_type { get; set; }
        public string traning_name { get; set; }
        public detialTrainingRequester detialTrainingRequester { get; set; }
        public List<participantTraining> listParticipantTraining { get; set; }

    }


    public class TrainingRequisitionSearch : SearchParameter
    {
        public DateTime? Search_purpose_end_date { get; set; }
        public string Search_status { get; set; }
        public string Search_training_cost { get; set; }
        public string Search_venue { get; set; }
        public DateTime? Search_purpose_start_date { get; set; }
        public string Search_trainingname { get; set; }
        public string Search_training_type { get; set; }
        public string Search_training_category { get; set; }


    }

    public class TrainingSchedule
    {
        public string title { get; set; }
        public string start { get; set; }
        public string description { get; set; }
        public string className { get; set; }
    
    }
}