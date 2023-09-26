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
    public partial class inventory_stock_taking_detail : IEntity, IBaseRecord
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

        static inventory_stock_taking_detail()
        {
            IsConstant = false;
            EntityName = GetTableName();
            EntityDisplayName = GetTableName(true);
            EntityId = GUIDHash.ConvertToMd5HashGUID(EntityName).ToString();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT");
            sb.AppendLine(@"	r.id,");
            sb.AppendLine(@"	r.created_by,");
            sb.AppendLine(@"	r.created_on,");
            sb.AppendLine(@"	r.modified_by,");
            sb.AppendLine(@"	r.modified_on,");
            sb.AppendLine(@"	r.approved_by,");
            sb.AppendLine(@"	r.approved_on,");
            sb.AppendLine(@"	r.is_active,");
            sb.AppendLine(@"	r.is_locked,");
            sb.AppendLine(@"	r.is_default,");
            sb.AppendLine(@"	r.owner_id,");

            sb.AppendLine(@"	r.inventory_stock_taking_id,");
            sb.AppendLine(@"	st.stock_taking_number,");
            sb.AppendLine(@"	st.stock_taking_datetime,");
            sb.AppendLine(@"	r.item_id,");
            sb.AppendLine(@"	item.item_code,");
            sb.AppendLine(@"	item.item_name,");
            sb.AppendLine(@"	st.item_type_id,");
            sb.AppendLine(@"	it.type_name AS item_type_name,");
            sb.AppendLine(@"	r.item_category_id,");
            sb.AppendLine(@"	COALESCE(r.item_category_name, ic.category_name) AS item_category_name,");
            sb.AppendLine(@"	r.uom_id,");
            sb.AppendLine(@"	uom.unit_name AS uom_name,");
            sb.AppendLine(@"	r.brand,");
            sb.AppendLine(@"	r.subbrand,");
            sb.AppendLine(@"	r.task_id,");
            sb.AppendLine(@"	r.task_detail,");
            sb.AppendLine(@"	r.description,");
            sb.AppendLine(@"	r.qty_on_hand,");
            sb.AppendLine(@"	r.stock_taking,");
            sb.AppendLine(@"	r.broken,");
            sb.AppendLine(@"	r.difference,");
            sb.AppendLine(@"	r.file_attachment,");
            sb.AppendLine(@"	fm.name AS filename,");

            sb.AppendLine(@"	u0.app_fullname AS record_created_by,");
            sb.AppendLine(@"	u1.app_fullname AS record_modified_by,");
            sb.AppendLine(@"	u2.app_fullname AS record_approved_by,");
            sb.AppendLine(@"	u3.app_fullname AS record_owner,");
            sb.AppendLine(@"	t.team_name AS record_owning_team ");

            sb.AppendLine(@"FROM");
            sb.AppendLine(@"	fn.inventory_stock_taking_detail AS r");
            sb.AppendLine(@"	LEFT OUTER JOIN  fn.inventory_stock_taking AS st ON r.inventory_stock_taking_id = st.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.item ON r.item_id = item.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.item_type AS it ON st.item_type_id = it.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.item_category AS ic ON r.item_category_id = ic.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.uom ON r.uom_id = uom.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.filemaster AS fm ON r.file_attachment = fm.id");

            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u0 ON r.created_by = u0.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u1 ON r.modified_by = u1.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u2 ON r.approved_by = u2.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.application_user AS u3 ON r.owner_id = u3.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.team AS t ON r.owner_id = t.id");

            DefaultView = sb.ToString();

            DefaultViewColumns = new Dictionary<string, string>();
            DefaultViewColumns.Add("id", "r.id");
            DefaultViewColumns.Add("created_by", "r.created_by");
            DefaultViewColumns.Add("created_on", "r.created_on");
            DefaultViewColumns.Add("modified_by", "r.modified_by");
            DefaultViewColumns.Add("modified_on", "r.modified_on");
            DefaultViewColumns.Add("approved_by", "r.approved_by");
            DefaultViewColumns.Add("approved_on", "r.approved_on");
            DefaultViewColumns.Add("deleted_by", "r.deleted_by");
            DefaultViewColumns.Add("deleted_on", "r.deleted_on");
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("is_deleted", "r.is_deleted");
            DefaultViewColumns.Add("owner_id", "r.owner_id");

            DefaultViewColumns.Add("inventory_stock_taking_id", "r.inventory_stock_taking_id");
            DefaultViewColumns.Add("stock_taking_number", "st.stock_taking_number");
            DefaultViewColumns.Add("stock_taking_datetime", "st.stock_taking_datetime");
            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_code", "item.item_code");
            DefaultViewColumns.Add("item_name", "item.item_name");
            DefaultViewColumns.Add("item_type_id", "st.item_type_id");
            DefaultViewColumns.Add("item_type_name", "it.type_name");
            DefaultViewColumns.Add("item_category_id", "r.item_category_id");
            DefaultViewColumns.Add("item_category_name", "COALESCE(r.item_category_name, ic.category_name)");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("uom_name", "uom.unit_name");
            DefaultViewColumns.Add("brand", "r.brand");
            DefaultViewColumns.Add("subbrand", "r.subbrand");
            DefaultViewColumns.Add("task_id", "r.task_id");
            DefaultViewColumns.Add("task_detail", "r.task_detail");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("qty_on_hand", "r.qty_on_hand");
            DefaultViewColumns.Add("stock_taking", "r.stock_taking");
            DefaultViewColumns.Add("broken", "r.broken");
            DefaultViewColumns.Add("difference", "r.difference");
            DefaultViewColumns.Add("file_attachment", "r.file_attachment");
            DefaultViewColumns.Add("filename", "fm.name");
            
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
            DefaultViewOrders.Add("stock_taking_number", "DESC");
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
