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
    public partial class account : IEntity, IBaseRecord
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

        static account()
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

                        + " r.account_number, "
                        + " r.account_name, "
                        + " r.account_level, "
                        + " r.account_group_id, "
                        + " a1.account_group_name, "
                        + " r.account_parent_id, "
                        + " p1.account_number AS account_parent_number, "
                        + " p1.account_name AS account_parent_name, "
                        + " COALESCE(r.is_normal_debit, a1.is_normal_debit) AS is_normal_debit, "
                        + " COALESCE(r.report_balance, a1.report_balance) AS report_balance, "
                        + " COALESCE(r.report_profit_loss, a1.report_profit_loss) AS report_profit_loss, "
                        + " r.currency_id, "
                        + " c1.currency_name, "
                        + " c1.currency_code, "
                        + " r.legal_entity_id, "
                        + " l1.legal_entity_name, "
                        + " r.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM            fn.account AS r "
                        + " LEFT OUTER JOIN fn.account AS p1 ON r.account_parent_id = p1.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS l1 ON r.legal_entity_id = l1.id "
                        + " LEFT OUTER JOIN dbo.currency AS c1 ON r.currency_id = c1.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON r.organization_id = o1.id "
                        + " LEFT OUTER JOIN fn.account_group AS a1 ON r.account_group_id = a1.id "

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

            DefaultViewColumns.Add("account_number", "r.account_number");
            DefaultViewColumns.Add("account_name", "r.account_name");
            DefaultViewColumns.Add("account_level", "r.account_level");
            DefaultViewColumns.Add("account_group_id", "r.account_group_id");
            DefaultViewColumns.Add("account_group_name", "a1.account_group_name");
            DefaultViewColumns.Add("account_parent_id", "r.account_parent_id");
            DefaultViewColumns.Add("account_parent_number", "p1.account_parent_number");
            DefaultViewColumns.Add("account_parent_name", "p1.account_parent_name");
            DefaultViewColumns.Add("is_normal_debit", "COALESCE(r.is_normal_debit, a1.is_normal_debit)");
            DefaultViewColumns.Add("report_balance", "COALESCE(r.report_balance, a1.report_balance)");
            DefaultViewColumns.Add("report_profit_loss", "COALESCE(r.report_profit_loss, a1.report_profit_loss)");

            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_name", "c1.currency_name");
            DefaultViewColumns.Add("currency_code", "c1.currency_code");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "l1.legal_entity_name");
            DefaultViewColumns.Add("organization_id", "r.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");

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
            DefaultViewOrders.Add("legal_entity_name", "ASC");
            DefaultViewOrders.Add("account_number", "ASC");
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
