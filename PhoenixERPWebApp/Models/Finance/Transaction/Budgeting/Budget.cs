using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Finance.Transaction.Budget
{
    public class Budget : FinanceEntityBase
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Type Budget Header Id")]
        public string type_budget_hdr_id { get; set; }

        [DisplayName("Budget Number")]
        public string BudgetNumber { get; set; }

        [DisplayName("Financial Year Id")]
        public string FinancialYearId { get; set; }

        [DisplayName("Financial Year Name")]
        public int? FinancialYearName { get; set; }

        [DisplayName("Financial Month")]
        public string FinancialMonth { get; set; }

        [DisplayName("Legal Entity Id")]
        public string LegalEntityId { get; set; }

        [DisplayName("Business Unit Id")]
        public string BusinessUnitId { get; set; }

        [DisplayName("Budget Number")]
        public string account_id { get; set; }

        [DisplayName("currency id")]
        public string currency_id { get; set; }

        [DisplayName("Exchange Rate")]
        public int? exchange_rate { get; set; }

        [DisplayName("Budget Value")]
        public double? budget_value { get; set; }

        [DisplayName("Budget Usage")]
        public double? budget_usage { get; set; }

        [DisplayName("Revision")]
        public int? revision { get; set; }

        public string employee_basic_info_id { get; set; }

     
        public decimal? budget_01 { get; set; }

       
        public decimal? budget_02 { get; set; }

      
        public decimal? budget_03 { get; set; }

       
        public decimal? budget_04 { get; set; }

       
        public decimal? budget_05 { get; set; }

      
        public decimal? budget_06 { get; set; }

     
        public decimal? budget_07 { get; set; }

    
        public decimal? budget_08 { get; set; }

    
        public decimal? budget_09 { get; set; }

      
        public decimal? budget_10 { get; set; }

     
        public decimal? budget_11 { get; set; }

        public decimal? budget_12 { get; set; }
        public decimal? TotalBudget { get; set; }

        public decimal? Actual { get; set; }
        public decimal? Balance { get; set; }

        public string CodeRec { get; set; }

        public string AccountName { get; set; }
    }
}
