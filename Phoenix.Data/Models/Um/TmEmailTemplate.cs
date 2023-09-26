using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmEmailTemplate", Schema = "Um")]
    public class TmEmailTemplate : EntityBaseUm
    {
        public string Id { get; set; }
        public string EmailTemplateDescription { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? IsHtml { get; set; }
        public string SubGroupId { get; set; }
        public string DivisionId { get; set; }
        public string MenuId { get; set; }
    }
}
