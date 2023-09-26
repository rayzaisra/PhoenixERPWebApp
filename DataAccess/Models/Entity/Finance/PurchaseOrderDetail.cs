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
    public partial class purchase_order_detail : IEntity, IBaseRecord
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

        static purchase_order_detail()
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
                + " r.purchase_order_id, " 
                + " po.purchase_order_number, " 
                + " po.purchase_order_date, " 
                + " r.item_id, " 
                + " i.item_code, " 
                + " r.item_type, " 
                + " it.type_name, "
                +@" CASE COALESCE(i.item_code, '0000000000000')
                    WHEN '0000000000000' THEN r.item_name
                    ELSE i.item_name
                END AS item_name, "
                //+ " r.item_name, " 
                +" r.item_brand, " 
                + " r.item_sub_brand, " 
                + " r.item_description, " 
                + " r.qty, "
                + " 0 AS 'total_received_qty', "
                + " r.unit_price, " 
                + " r.amount, " 
                + " r.uom_id, " 
                + " uo.unit_name AS uom_name, " 
                + " r.discount_amount, " 
                + " r.discount_percent, " 
                + " r.tax_percent, " 
                + " r.tax_amount, " 
                + " r.amount_net, " 
                + " po.organization_id, " 
                + " o1.organization_name, "
                + " g.id AS inventory_goods_receipt_detail_id, "

                + " u0.app_fullname AS record_created_by, " 
                + " u1.app_fullname AS record_modified_by, " 
                + " u2.app_fullname AS record_approved_by, " 
                + " u3.app_fullname AS record_owner, " 
                + " t.team_name AS record_owning_team " 
                + " FROM fn.purchase_order_detail AS r " 
                + " LEFT OUTER JOIN dbo.item AS i ON r.item_id = i.id " 
                + " LEFT OUTER JOIN dbo.item_type AS it ON r.item_type = it.id " 
                + " LEFT OUTER JOIN fn.purchase_order AS po ON r.purchase_order_id = po.id " 
                + " LEFT OUTER JOIN dbo.uom AS uo ON r.uom_id = uo.id " 
                + " LEFT OUTER JOIN dbo.organization AS o1 ON po.organization_id = o1.id " 
                + " LEFT OUTER JOIN dbo.application_user AS u0 ON r.created_by = u0.id " 
                + " LEFT OUTER JOIN dbo.application_user AS u1 ON r.modified_by = u1.id " 
                + " LEFT OUTER JOIN dbo.application_user AS u2 ON r.approved_by = u2.id " 
                + " LEFT OUTER JOIN dbo.application_user AS u3 ON r.owner_id = u3.id " 
                + " LEFT OUTER JOIN dbo.team AS t ON r.owner_id = t.id "
                + " LEFT OUTER JOIN fn.inventory_goods_receipt_detail AS g ON r.id = g.purchase_order_detail_id ";

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

            DefaultViewColumns.Add("purchase_order_id", "r.purchase_order_id");
            DefaultViewColumns.Add("purchase_order_number", "po.purchase_order_number");
            DefaultViewColumns.Add("purchase_order_date", "po.purchase_order_date");
            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_code", "it.item_code");
            //DefaultViewColumns.Add("item_name", "r.item_name");
            DefaultViewColumns.Add("item_name", 
                @"CASE COALESCE(i.item_code, '0000000000000')
                    WHEN '0000000000000' THEN r.item_name
                    ELSE i.item_name
                END");
            DefaultViewColumns.Add("type_name", "it.type_name");
            DefaultViewColumns.Add("item_description", "r.item_description");
            DefaultViewColumns.Add("qty", "r.qty");
            DefaultViewColumns.Add("unit_price", "r.unit_price");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("organization_id", "po.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("uom_name", "uo.unit_name");
            DefaultViewColumns.Add("item_type", "r.item_type");
            DefaultViewColumns.Add("item_brand", "r.item_brand");
            DefaultViewColumns.Add("item_sub_brand", "r.item_sub_brand");
            DefaultViewColumns.Add("tax_percent", "r.tax_percent");
            DefaultViewColumns.Add("tax_amount", "r.tax_amount");
            DefaultViewColumns.Add("amount_net", "r.amount_net");            

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
            DefaultViewOrders.Add("purchase_order_number", "DESC");
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
