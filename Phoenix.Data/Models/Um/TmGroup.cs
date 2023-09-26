using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TmGroup", Schema = "Um")]
    public class TmGroup : EntityBaseUm
    {
        public string GroupName { get; set; }
    }
}
