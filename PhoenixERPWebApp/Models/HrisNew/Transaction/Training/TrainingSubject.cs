using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class TrainingSubject : HrisEntityBase
    {
 
        public string Id { get; set; }
        public string Subject { get; set; }
       
    }
}