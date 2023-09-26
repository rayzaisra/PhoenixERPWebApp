using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class PdrParameter : HrisEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Category")]
        public string PdrCategoryId { get; set; }

        [DisplayName("Alphabet")]
        public string Alphabet { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Key Behavior 1")]
        public string KeyBehavior1 { get; set; }

        [DisplayName("Key Behavior 2")]
        public string KeyBehavior2 { get; set; }

        [DisplayName("Key Behavior 3")]
        public string KeyBehavior3 { get; set; }

        [DisplayName("Key Behavior 4")]
        public string KeyBehavior4 { get; set; }

        [DisplayName("Key Behavior 5")]
        public string KeyBehavior5 { get; set; }

        [DisplayName("Job Grade")]
        public string JobGradeId { get; set; }

        [DisplayName("Grade")]
        public string Grade { get; set; }

        [DisplayName("Category Name")]
        public string PdrCategoryName { get; set; }
    }
}