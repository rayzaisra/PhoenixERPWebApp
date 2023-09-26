//using CommonTool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using DataAccess;
using DataAccess.PhoenixERP.General;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class vw_approval_financial_transaction : IEntityNoBaseEntity
    {
        #region Default IBaseRecord

        public static readonly string EntityId;

        public static readonly string EntityName;

        public static readonly string EntityDisplayName;

        public static readonly string DefaultView;

        public static readonly Dictionary<string, string> DefaultViewColumns;

        public static readonly Dictionary<string, string> DefaultViewColumnTitles;

        public static readonly Dictionary<string, string> DefaultViewOrders;

        public static readonly Dictionary<string, dynamic> FilterDynamicBuilder;

        #endregion

        static vw_approval_financial_transaction()
        {
            EntityName = approval.GetTableName();
            EntityDisplayName = approval.GetTableName(true);
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

                        + " r.approval_number, "
                        + " r.title, "
                        + " r.description, "
                        + " r.record_id, "
                        + " r.application_entity_id, "
                        + " e1.entity_name, "
                        + " e1.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team, "

                        + " ft.transaction_number, "
                        + " ft.transaction_datetime "

                        + " FROM dbo.approval AS r "
                        + " LEFT OUTER JOIN fn.financial_transaction ft ON ft.id=r.record_id "

                        + " LEFT OUTER JOIN dbo.application_entity AS e1 ON e1.id = r.application_entity_id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON o1.id = e1.organization_id "
                        + " LEFT OUTER JOIN dbo.application_user AS u0 ON u0.id = r.created_by "
                        + " LEFT OUTER JOIN dbo.application_user AS u1 ON u1.id = r.modified_by "
                        + " LEFT OUTER JOIN dbo.application_user AS u2 ON u2.id = r.approved_by "
                        + " LEFT OUTER JOIN dbo.application_user AS u3 ON u3.id = r.owner_id "
                        + " LEFT OUTER JOIN dbo.team AS t ON t.id = r.owner_id ";


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

            DefaultViewColumns.Add("approval_number", "r.approval_number");
            DefaultViewColumns.Add("title", "r.title");
            DefaultViewColumns.Add("description", "r.description");
            DefaultViewColumns.Add("record_id", "r.record_id");
            DefaultViewColumns.Add("application_entity_id", "r.application_entity_id");
            DefaultViewColumns.Add("entity_name", "e1.entity_name");
            DefaultViewColumns.Add("organization_id", "a1.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");

            DefaultViewColumns.Add("record_created_by", "u0.app_fullname");
            DefaultViewColumns.Add("record_modified_by", "u1.app_fullname");
            DefaultViewColumns.Add("record_approved_by", "u2.app_fullname");
            DefaultViewColumns.Add("record_owner", "u3.app_fullname");
            DefaultViewColumns.Add("record_owning_team", "t.team_name");

            DefaultViewColumns.Add("transaction_number", "ft.transaction_number");
            DefaultViewColumns.Add("transaction_datetime", "ft.transaction_datetime");

            DefaultViewColumnTitles = new Dictionary<string, string>();
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            foreach (var c in DefaultViewColumns)
            {
                DefaultViewColumnTitles.Add(c.Key, ti.ToTitleCase(c.Key.Replace("_", " ")));
            }

            DefaultViewOrders = new Dictionary<string, string>();
            DefaultViewOrders.Add("created_on", "DESC");
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

        public string GetCustomView()
        {
            return DefaultView;
        }

        public Dictionary<string, string> GetCustomViewColumns()
        {
            return DefaultViewColumns;
        }

        public Dictionary<string, string> GetCustomViewColumnTitles()
        {

            return DefaultViewColumnTitles;
        }

        public Dictionary<string, string> GetCustomViewOrders()
        {
            return DefaultViewOrders;
        }

        public Dictionary<string, dynamic> GetFilterDynamicBuilder()
        {
            return FilterDynamicBuilder;
        }
    } 
}