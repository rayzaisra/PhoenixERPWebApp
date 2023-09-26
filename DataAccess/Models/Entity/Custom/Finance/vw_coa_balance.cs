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
    public partial class vw_coa_balance : IEntityNoBaseEntity
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

        static vw_coa_balance()
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

                        + " r.organization_id, "
                        + " r.account_group_id, "
                        + " r.account_parent_id, "
                        + " r.account_number, "
                        + " r.account_name, "
                        + " r.account_level, "
                        + " fd.currency_id, "
                        + " r.cost_sharing_allocation_id, "
                        + " r.legal_entity_id, "
                        + " r.is_normal_debit, "
                        + " r.report_balance, "
                        + " r.report_profit_loss, "
                        + " c.currency_code, "
                        + " c.currency_name, "
                        + " c.currency_symbol, "
                        + " fd.id as financial_transaction_id, "
                        + " fd.exchange_rate, "
                        + "	COALESCE(fd.debit,0) AS debit, "
                        + "	COALESCE(fd.credit,0) AS credit,"
                        + " COALESCE(fd.exchange_rate * fd.debit, 0) AS normalized_debit, "
                        + " COALESCE(fd.exchange_rate * fd.credit, 0) AS normalized_credit, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM fn.account AS r "

                        + " LEFT OUTER JOIN fn.financial_transaction_detail AS fd "
                        + " ON r.id = fd.account_id "
                        + " AND fd.is_active = 1 "
                        + " AND fd.is_default = 1 "
                        + " AND fd.is_locked = 1 "

                        + " LEFT OUTER JOIN fn.financial_transaction AS ft "
                        + " ON fd.financial_transaction_id = ft.id "
                        + " AND ft.is_active = 1 "
                        + " AND ft.is_default = 1 "
                        + " AND ft.is_locked = 1 "

                        + " LEFT OUTER JOIN fn.financial_transaction_type AS ftt "
                        + " ON ft.transaction_type_id = ftt.id "

                        + " LEFT OUTER JOIN dbo.currency AS c "
                        + " ON fd.currency_id = c.id "

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

            DefaultViewColumns.Add("organization_id", "r.organization_id");
            DefaultViewColumns.Add("account_group_id", "r.account_group_id");
            DefaultViewColumns.Add("account_parent_id", "r.account_parent_id");
            DefaultViewColumns.Add("account_number", "r.account_number");
            DefaultViewColumns.Add("account_name", "r.account_name");
            DefaultViewColumns.Add("account_level", "r.account_level");
            DefaultViewColumns.Add("currency_id", "fd.currency_id");
            DefaultViewColumns.Add("cost_sharing_allocation_id", "r.cost_sharing_allocation_id");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("is_normal_debit", "r.is_normal_debit");
            DefaultViewColumns.Add("report_balance", "r.report_balance");
            DefaultViewColumns.Add("report_profit_loss", "r.report_profit_loss");
            DefaultViewColumns.Add("currency_code", "c.currency_code");
            DefaultViewColumns.Add("currency_name", "c.currency_name");
            DefaultViewColumns.Add("currency_symbol", "c.currency_symbol");
            DefaultViewColumns.Add("financial_transaction_id", "c.fd.id");
            DefaultViewColumns.Add("exchange_rate", "fd.exchange_rate");
            DefaultViewColumns.Add("debit", "COALESCE(fd.debit,0)");
            DefaultViewColumns.Add("credit", "COALESCE(fd.credit,0)");
            DefaultViewColumns.Add("normalized_debit", "COALESCE(fd.exchange_rate * fd.debit, 0)");
            DefaultViewColumns.Add("normalized_credit", "COALESCE(fd.exchange_rate * fd.credit, 0");

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
            DefaultViewOrders.Add("account_level", "DESC");
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