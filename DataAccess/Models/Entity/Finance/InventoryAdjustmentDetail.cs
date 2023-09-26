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
    public partial class inventory_adjustment_detail : IEntity, IBaseRecord
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

        static inventory_adjustment_detail()
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
                        + " r.deleted_by, "
                        + " r.deleted_on, "
                        + " r.is_active, "
                        + " r.is_locked, "
                        + " r.is_default, "
                        + " r.is_deleted, "
                        + " r.owner_id, "

                        + " r.inventory_adjustment_id, "
                        + " ia.adjustment_number, "
                        + " ia.adjustment_datetime, "
                        + " r.item_id, "
                        + " item.item_code, "
                        + " item.item_name, "
                        + " ia.item_type_id, "
                        + " it.type_name AS item_type_name, "
                        + " r.item_category_id, "
                        + " COALESCE(r.item_category_name, ic.category_name) AS item_category_name, "
                        + " r.uom_id, "
                        + " uom.unit_name AS uom_name, "
                        + " r.brand, "
                        + " r.subbrand, "
                        + " r.task_id, "
                        + " r.task_detail, "
                        + " r.description, "
                        + " r.qty_on_hand, "
                        + " r.adjusted_in, "
                        + " r.adjusted_out, "
                        + " r.file_attachment, "
                        + " fm.name AS filename, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM fn.inventory_adjustment_detail AS r "
                        + " LEFT OUTER JOIN  fn.inventory_adjustment AS ia ON r.inventory_adjustment_id = ia.id "
                        + " LEFT OUTER JOIN dbo.item ON r.item_id = item.id "
                        + " LEFT OUTER JOIN dbo.item_type AS it ON ia.item_type_id = it.id "
                        + " LEFT OUTER JOIN dbo.item_category AS ic ON r.item_category_id = ic.id "
                        + " LEFT OUTER JOIN dbo.uom ON r.uom_id = uom.id "
                        + " LEFT OUTER JOIN dbo.filemaster AS fm ON r.file_attachment = fm.id "

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
            DefaultViewColumns.Add("deleted_by", "r.deleted_by");
            DefaultViewColumns.Add("deleted_on", "r.deleted_on");
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("is_deleted", "r.is_deleted");
            DefaultViewColumns.Add("owner_id", "r.owner_id");
            
            DefaultViewColumns.Add("inventory_adjustment_id", "r.inventory_adjustment_id");
            DefaultViewColumns.Add("adjustment_number", "ia.adjustment_number");
            DefaultViewColumns.Add("adjustment_datetime", "ia.adjustment_datetime");
            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_code", "item.item_code");
            DefaultViewColumns.Add("item_name", "item.item_name");
            DefaultViewColumns.Add("item_type_id", "ia.item_type_id");
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
            DefaultViewColumns.Add("adjusted_in", "r.adjusted_in");
            DefaultViewColumns.Add("adjusted_out", "r.adjusted_out");
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
            DefaultViewOrders.Add("adjustment_number", "DESC");
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
