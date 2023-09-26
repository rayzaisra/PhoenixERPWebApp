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
    public partial class fixed_asset_received_detail : IEntity, IBaseRecord
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

        static fixed_asset_received_detail()
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

            sb.AppendLine(@"	r.fixed_asset_received_id,");
            sb.AppendLine(@"	far.far_number,");
            sb.AppendLine(@"	far.far_datetime,");
            sb.AppendLine(@"	r.fixed_asset_id,");
            sb.AppendLine(@"	fa.item_id,");
            sb.AppendLine(@"	fa.fixed_asset_number,");
            sb.AppendLine(@"	fa.fixed_asset_name,");
            sb.AppendLine(@"	r.account_id,");
            sb.AppendLine(@"	ac.account_number,");
            sb.AppendLine(@"	ac.account_name,");
            sb.AppendLine(@"	r.sku_code,");
            sb.AppendLine(@"	r.serial_number,");
            sb.AppendLine(@"	r.item_category_id,");
            sb.AppendLine(@"	COALESCE(cat.category_name, r.item_category_name) AS item_category_name,");
            sb.AppendLine(@"	r.uom_id,");
            sb.AppendLine(@"	COALESCE(uom.unit_name, r.unit_name) AS uom_name,");
            sb.AppendLine(@"	r.brand,");
            sb.AppendLine(@"	r.subbrand,");
            sb.AppendLine(@"	r.description,");
            sb.AppendLine(@"	r.qty,");
            sb.AppendLine(@"	r.remark,");
            sb.AppendLine(@"	r.file_attachment,");
            sb.AppendLine(@"	fm.name AS filename,");

            sb.AppendLine(@"	u0.app_fullname AS record_created_by,");
            sb.AppendLine(@"	u1.app_fullname AS record_modified_by,");
            sb.AppendLine(@"	u2.app_fullname AS record_approved_by,");
            sb.AppendLine(@"	u3.app_fullname AS record_owner,");
            sb.AppendLine(@"	t.team_name AS record_owning_team ");

            sb.AppendLine(@"FROM");
            sb.AppendLine(@"	fn.fixed_asset_received_detail AS r");
            sb.AppendLine(@"	LEFT OUTER JOIN  fn.fixed_asset_received AS far ON r.fixed_asset_received_id = far.id");
            sb.AppendLine(@"	LEFT OUTER JOIN  fn.fixed_asset AS fa ON r.fixed_asset_id = fa.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.item ON fa.item_id = item.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.item_category AS cat ON r.item_category_id = cat.id");
            sb.AppendLine(@"	LEFT OUTER JOIN dbo.uom ON r.uom_id = uom.id");
            sb.AppendLine(@"	LEFT OUTER JOIN  fn.account AS ac ON r.account_id = ac.id");
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
            
            DefaultViewColumns.Add("fixed_asset_received_id", "r.fixed_asset_received_id");
            DefaultViewColumns.Add("far_number", "far.far_number");
            DefaultViewColumns.Add("far_datetime", "far.far_datetime");
            DefaultViewColumns.Add("fixed_asset_id", "r.fixed_asset_id");
            DefaultViewColumns.Add("item_id", "fa.item_id");
            DefaultViewColumns.Add("fixed_asset_number", "fa.fixed_asset_number");
            DefaultViewColumns.Add("fixed_asset_name", "fa.fixed_asset_name");
            DefaultViewColumns.Add("account_id", "r.account_id");
            DefaultViewColumns.Add("account_number", "ac.account_number");
            DefaultViewColumns.Add("account_name", "ac.account_name");
            DefaultViewColumns.Add("sku_code", "r.sku_code");
            DefaultViewColumns.Add("serial_number", "r.serial_number");
            DefaultViewColumns.Add("item_category_id", "r.item_category_id");
            DefaultViewColumns.Add("item_category_name", "COALESCE(cat.category_name, r.item_category_name)");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("uom_name", "COALESCE(uom.unit_name, r.unit_name)");
            DefaultViewColumns.Add("brand", "r.brand");
            DefaultViewColumns.Add("subbrand", "r.subbrand");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("qty", "r.qty");
            DefaultViewColumns.Add("remark", "r.remark");
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
            DefaultViewOrders.Add("far_number", "DESC");
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
