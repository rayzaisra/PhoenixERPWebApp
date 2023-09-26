using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using DataAccess;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class internal_rfq_detail : IEntity, IBaseRecord
    {
        #region Default IBaseRecord

        public static readonly bool IsConstant;

        public static readonly string EntityId;

        public static readonly string EntityName;

        public static readonly string EntityDisplayName;

        public static readonly string DefaultView;

        public static readonly Dictionary<string, string> DefaultViewColumns;

        public static readonly Dictionary<string, string> DefaultViewColumnTitles;

        public static readonly Dictionary<string, string> DefaultViewOrders;

        public static readonly Dictionary<string, dynamic> FilterDynamicBuilder;

        #endregion

        static internal_rfq_detail()
        {
            IsConstant = false;
            EntityName = GetTableName();
            EntityDisplayName = GetTableName(true);
            EntityId = GUIDHash.ConvertToMd5HashGUID(EntityName).ToString();

            DefaultView = " SELECT "
                        + " r.id, "
                        + " r.created_by, "
                        + " r.created_on, "
                        + " r.modified_by, "
                        + " r.modified_on, "
                        + " r.approved_by, "
                        + " r.approved_on, "
                        + " r.is_active, "
                        + " r.is_locked, "
                        + " r.is_default, "
                        + " r.owner_id, "

                        + " r.item_id, " 
                        + " i1.item_code, " 
                        + " COALESCE(r.item_name, i1.item_name) AS item_name, " 
                        + " r.item_type, "
                        + " r.description, "
                        + " r.uom_id, " 
                        + " dbo.uom.unit_name, " 
                        + " r.qty, " 
                        + " r.unit_price, " 
                        + " r.amount, " 
                        + " rfq.internal_rfq_number, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM fn.internal_rfq_detail AS r " 
                        + " LEFT OUTER JOIN fn.internal_rfq AS rfq ON r.internal_rfq_id = rfq.id " 
                        + " LEFT OUTER JOIN dbo.item AS i1 ON r.item_id = i1.id " 
                        + " LEFT OUTER JOIN dbo.uom ON r.uom_id = dbo.uom.id " 
                        + " LEFT OUTER JOIN dbo.application_user AS u0 ON r.created_by = u0.id " 
                        + " LEFT OUTER JOIN dbo.application_user AS u1 ON r.modified_by = u1.id " 
                        + " LEFT OUTER JOIN dbo.application_user AS u2 ON r.approved_by = u2.id " 
                        + " LEFT OUTER JOIN dbo.application_user AS u3 ON r.owner_id = u3.id " 
                        + " LEFT OUTER JOIN dbo.team AS t ON r.owner_id = t.id ";

            DefaultViewColumns = new Dictionary<string, string>();
            DefaultViewColumns.Add("id", "r.id");
            DefaultViewColumns.Add("created_by", "r.created_by");
            DefaultViewColumns.Add("created_on", "r.created_on");
            DefaultViewColumns.Add("modified_by", "r.modified_by");
            DefaultViewColumns.Add("modified_on", "r.modified_on");
            DefaultViewColumns.Add("approved_by", "r.approved_by");
            DefaultViewColumns.Add("approved_on", "r.approved_on");
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("owner_id", "r.owner_id");

            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_code", "i1.item_code");
            DefaultViewColumns.Add("item_name", "r.item_name");
            DefaultViewColumns.Add("item_type", "r.item_type");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("unit_name", "dbo.uom.unit_name");
            DefaultViewColumns.Add("qty", "r.qty");
            DefaultViewColumns.Add("unit_price", "r.unit_price");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("internal_rfq_number", "rfq.internal_rfq_number");

            DefaultViewColumns.Add("record_created_by", "u0.app_fullname");
            DefaultViewColumns.Add("record_modified_by", "u1.app_fullname");
            DefaultViewColumns.Add("record_approved_by", "u2.app_fullname");
            DefaultViewColumns.Add("record_owner", "u3.app_fullname");
            DefaultViewColumns.Add("record_owning_team", "t.team_name");

            DefaultViewColumnTitles = new Dictionary<string, string>();
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            foreach (var c in DefaultViewColumns)
            {
                DefaultViewColumnTitles.Add(c.Key, ti.ToTitleCase(c.Key.Replace("_", " ")));
            }

            DefaultViewOrders = new Dictionary<string, string>();
            DefaultViewOrders.Add("item_name", "ASC");
            DefaultViewOrders.Add("internal_rfq_number", "ASC");
        }

        public bool CheckIfConstant()
        {
            return IsConstant;
        }

        public string GetEntityId()
        {
            return EntityId;
        }

        public string GetEntityName()
        {
            return EntityName;
        }

        public string GetEntityDisplayName()
        {
            return EntityDisplayName;
        }

        public string GetDefaultView()
        {
            return DefaultView;
        }

        public Dictionary<string, string> GetDefaultViewColumns()
        {
            return DefaultViewColumns;
        }

        public Dictionary<string, string> GetDefaultViewColumnTitles()
        {

            return DefaultViewColumnTitles;
        }

        public Dictionary<string, string> GetDefaultViewOrders()
        {
            return DefaultViewOrders;
        }

        public Dictionary<string, dynamic> GetFilterDynamicBuilder()
        {
            return FilterDynamicBuilder;
        }
    } 
}
