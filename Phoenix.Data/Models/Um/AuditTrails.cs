using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Data.Models.Um
{
    [Table("TBL_AUDIT", Schema = "Um")]
    public class AuditTrails
    {
        [Key]
        public int ID { get; set; }
        public string ACT_TYPE { get; set; }
        public string TABLE_NAME{ get; set; }
        public string PRIMARY_KEY{ get; set; }
        public string FIELD_NAME{ get; set; }
        public string OLD_VALUE{ get; set; }
        public string NEW_VALUE{ get; set; }
        public DateTime? UPDATE_DATE{ get; set; }
        public string UPDATE_BY{ get; set; }
        public string DB_USER_NAME { get; set; }

        [NotMapped]
        public string ActionDescription
        {
            get
            {
                var actType = "DELETED";
                if (ACT_TYPE == "I") actType = "INSERT";
                else if (ACT_TYPE == "U") actType = "UPDATE";
                return actType;
            }
        }
        [NotMapped]
        public string DateToString
        {
            get
            {
                return Convert.ToDateTime(UPDATE_DATE).ToString("yyyy-MM-dd hh:mm:ss");
            }
        }
    }
}
