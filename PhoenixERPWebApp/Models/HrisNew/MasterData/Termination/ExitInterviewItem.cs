using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ExitInterviewItem : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("No")]
        public int? No { get; set; }
        
        [DisplayName("Text Item")]
        public string TextItem { get; set; }

        [DisplayName("Category")]
        public string CategoryExitInterviewId { get; set; }

        [DisplayName("Type")]
        public int Type { get; set; }

        public CategoryExitInterview CategoryExitInterview { get; set; }

        [DisplayName("Type")]
        public string TypeName { get { return InputType.InputTypeName(Type); } }
    }
}