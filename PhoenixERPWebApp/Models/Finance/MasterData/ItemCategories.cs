using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class ItemCategories : FinanceEntityBase
    {
        [DisplayName("ID")]
        public string Id { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        [DisplayName("Life Time (Month)")]
        public int? LifeTimeMonth { get; set; }

        [DisplayName("Debit COA")]
        public string COADebit { get; set; }

        [DisplayName("Credit COA")]
        public string COACredit { get; set; }
    }
}
