//using CommonTool;
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
    public partial class purchase_request_detail : IEntity, IBaseRecord
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

        static purchase_request_detail()
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

                        + " r.purchase_request_id, "
                        + " p.request_number, "
                        + " r.item_id, "
                        + " it.type_name, "
                        + " r.item_type_id, "
                        + " r.rfq_id, "
                        + " r.item_name, " 
                        + " i1.item_code, "
                        + " r.uom_id, "
                        + " u.unit_name AS uom_name, "
                        + " u.unit_symbol AS uom_symbol,"
                        + " r.subbrand_name, "
                        + " r.description, "
                        + " r.quantity, "
                        + " r.unit_price, "
                        + " r.currency_id,	 "
                        + " c.currency_code, "
                        + " r.exchange_rate, "
                        + " r.amount, "
                        + " r.pph, "
                        + " r.ppn, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "
                        + " FROM fn.purchase_request_detail AS r "

                        + " LEFT OUTER JOIN fn.purchase_request AS p "
                        + " ON r.purchase_request_id = p.id "

                        + " LEFT OUTER JOIN dbo.item_type AS it "
                        + " ON r.item_type_id = it.id " 
                        + " LEFT OUTER JOIN dbo.item AS i1 ON r.item_id = i1.id"

                        + " LEFT OUTER JOIN dbo.currency AS c "
                        + " ON r.currency_id = c.id " 

                        + " LEFT OUTER JOIN dbo.uom AS u "
                        + " ON r.uom_id = u.id " 

                        + " LEFT OUTER JOIN dbo.application_user AS u0 "
                        + " ON r.created_by = u0.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u1 "
                        + " ON r.modified_by = u1.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u2 "
                        + " ON r.approved_by = u2.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u3 "
                        + " ON r.owner_id = u3.id "
                        + " LEFT OUTER JOIN dbo.team AS t " 
                        + " ON r.owner_id = t.id ";

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

            DefaultViewColumns.Add("purchase_request_id", "r.purchase_request_id");
            DefaultViewColumns.Add("request_number", "p.request_number");
            DefaultViewColumns.Add("item_id", "r.item_id");
            DefaultViewColumns.Add("item_name", "r.item_name");
            DefaultViewColumns.Add("item_type_id", "r.item_type_id");
            DefaultViewColumns.Add("rfq_id", "r.rfq_id");
            DefaultViewColumns.Add("item_code", "i1.item_code");
            DefaultViewColumns.Add("type_name", "r.type_name");
            DefaultViewColumns.Add("uom_id", "r.uom_id");
            DefaultViewColumns.Add("uom_name", "u.unit_name");
            DefaultViewColumns.Add("uom_symbol", "u.unit_symbol");
            DefaultViewColumns.Add("subbrand_name", "r.subbrand_name");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("quantity", "r.quantity");
            DefaultViewColumns.Add("unit_price", "r.unit_price");
            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_code", "c.currency_code");            
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("amount", "r.amount");
            DefaultViewColumns.Add("pph", "r.pph");
            DefaultViewColumns.Add("ppn", "r.ppn");

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
            DefaultViewOrders.Add("request_number", "ASC");
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
