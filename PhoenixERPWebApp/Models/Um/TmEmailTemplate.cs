using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PhoenixERPWebApp.Models.Um
{
    public class TmEmailTemplate : EntityBase
    {
        [DisplayName("ID")]
        public string Id { get; set; }
        [DisplayName("Email Template Description")]
        public string EmailTemplateDescription { get; set; }
        [DisplayName("Subject")]
        public string Subject { get; set; }
        [DisplayName("Body")]
        public string Body { get; set; }
        [DisplayName("Is Html")]
        public bool? IsHtml { get; set; }
        public bool IsHtmlVal { get; set; }
        [DisplayName("Sub Group")]
        public string SubGroupId { get; set; }
        [DisplayName("Division")]
        public string DivisionId { get; set; }
        [DisplayName("Menu")]
        public string MenuId { get; set; }
        public string Html
        {
            get
            {
                return (bool)IsHtml ? "Yes" : "No";
            }
        }

    }
}
