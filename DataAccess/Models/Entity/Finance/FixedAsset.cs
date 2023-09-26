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
    public partial class fixed_asset : IEntity, IBaseRecord
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

        static fixed_asset()
        {
            IsConstant = false;
            EntityName = GetTableName();
            EntityDisplayName = GetTableName(true);
            EntityId = GUIDHash.ConvertToMd5HashGUID(EntityName).ToString();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT");
            sb.AppendLine(@"    r.id,");
            sb.AppendLine(@"    r.created_by,");
            sb.AppendLine(@"    r.created_on,");
            sb.AppendLine(@"    r.modified_by,");
            sb.AppendLine(@"    r.modified_on,");
            sb.AppendLine(@"    r.approved_by,");
            sb.AppendLine(@"    r.approved_on,");
            sb.AppendLine(@"    r.is_active,");
            sb.AppendLine(@"    r.is_locked,");
            sb.AppendLine(@"    r.is_default,");
            sb.AppendLine(@"    r.owner_id,");

            sb.AppendLine(@"	r.item_id,");
            sb.AppendLine(@"	item.item_code,");
            sb.AppendLine(@"	r.item_group_id,");
            sb.AppendLine(@"	ig.group_name AS item_group_name,");
            sb.AppendLine(@"	r.item_type_id,");
            sb.AppendLine(@"	r.item_category_id,");
            sb.AppendLine(@"	r.item_condition_id,");
            sb.AppendLine(@"    COALESCe(r.fixed_asset_name, item.item_name) AS fixed_asset_name,");
            sb.AppendLine(@"    r.fixed_asset_number,");
            sb.AppendLine(@"    r.unit_count,");
            sb.AppendLine(@"    r.purchase_order_id,");
            sb.AppendLine(@"	po.purchase_order_number,");
            sb.AppendLine(@"	po.purchase_order_date,");
            sb.AppendLine(@"    r.current_location,");
            sb.AppendLine(@"    r.exchange_rate,");
            sb.AppendLine(@"    r.evident_number,");
            sb.AppendLine(@"    r.current_price,");
            sb.AppendLine(@"    r.registration_date,");
            sb.AppendLine(@"    r.economic_period,");
            sb.AppendLine(@"    r.description,");
            sb.AppendLine(@"    cd.commercial_depreciation_name,");
            sb.AppendLine(@"    cur.currency_name,");
            sb.AppendLine(@"	cur.currency_code,");

            sb.AppendLine(@"    u0.app_fullname AS record_created_by,");
            sb.AppendLine(@"    u1.app_fullname AS record_modified_by,");
            sb.AppendLine(@"    u2.app_fullname AS record_approved_by,");
            sb.AppendLine(@"    u3.app_fullname AS record_owner,");
            sb.AppendLine(@"    t.team_name AS record_owning_team");

            sb.AppendLine(@"FROM fn.fixed_asset AS r");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.item AS item");
            sb.AppendLine(@"    ON r.item_id = item.id");
            sb.AppendLine(@"LEFT OUTER JOIN fn.purchase_order AS po");
            sb.AppendLine(@"    ON r.purchase_order_id = po.id");
            sb.AppendLine(@"LEFT OUTER JOIN fn.commercial_depreciation AS cd");
            sb.AppendLine(@"    ON r.commercial_depreciation_id = cd.id");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.currency AS cur");
            sb.AppendLine(@"    ON r.currency_id = cur.id");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.item_group AS ig");
            sb.AppendLine(@"    ON r.item_group_id = ig.id");

            sb.AppendLine(@"LEFT OUTER JOIN dbo.application_user AS u0");
            sb.AppendLine(@"    ON r.created_by = u0.id");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.application_user AS u1");
            sb.AppendLine(@"    ON r.modified_by = u1.id");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.application_user AS u2");
            sb.AppendLine(@"    ON r.approved_by = u2.id");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.application_user AS u3");
            sb.AppendLine(@"    ON r.owner_id = u3.id");
            sb.AppendLine(@"LEFT OUTER JOIN dbo.team AS t");
            sb.AppendLine(@"    ON r.owner_id = t.id");

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
            
            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_code", "item.item_code");
            DefaultViewColumns.Add("item_group_id", "r.item_group_id");
            DefaultViewColumns.Add("item_group_name", "ig.group_name");
            DefaultViewColumns.Add("item_type_id", "r.item_type_id");
            DefaultViewColumns.Add("item_category_id", "r.item_category_id");
            DefaultViewColumns.Add("item_condition_id", "r.item_condition_id");
            DefaultViewColumns.Add("fixed_asset_name", "COALESCE(r.fixed_asset_name, item.item_name)");
            DefaultViewColumns.Add("fixed_asset_number", "r.fixed_asset_number");
            DefaultViewColumns.Add("unit_count", "r.unit_count");
            DefaultViewColumns.Add("purchase_order_id", "r.purchase_order_id");
            DefaultViewColumns.Add("purchase_order_number", "po.purchase_order_number");
            DefaultViewColumns.Add("purchase_order_date", "po.purchase_order_date");
            DefaultViewColumns.Add("current_location", "r.current_location");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("evident_number", "r.evident_number");
            DefaultViewColumns.Add("current_price", "r.current_price");
            DefaultViewColumns.Add("registration_date", "r.registration_date");
            DefaultViewColumns.Add("economic_period", "r.economic_period");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("commercial_depreciation_name", "cd.commercial_depreciation_name");
            DefaultViewColumns.Add("currency_name", "cur.currency_name");
            DefaultViewColumns.Add("currency_code", "cur.currency_code");

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
            DefaultViewOrders.Add("fixed_asset_number", "ASC");
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
