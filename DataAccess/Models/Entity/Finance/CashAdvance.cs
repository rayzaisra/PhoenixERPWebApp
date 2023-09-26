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
    public partial class cash_advance : IEntity, IBaseRecord
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

        static cash_advance()
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

                        + " r.ca_number, "
                        + " r.ca_date, "
                        + " r.approval_id, "
                        + " r.revision, "
                        + " r.ca_period_from, "
                        + " r.ca_period_to, "
                        + " r.requested_by, "
                        + " u4.app_fullname AS requester_name, "
                        + " r.requested_on, "
                        + " r.purchase_order_id, "
                        + " COALESCE(po.purchase_order_number, r.purchase_order_number) AS purchase_order_number, "
                        + " COALESCE(v1.company_name, r.vendor_name) AS vendor_name, "
                        + " r.job_id, "
                        + " COALESCE(j1.pa_number, r.job_number) AS job_number, "
                        + " COALESCE(cmp.company_name, r.client_name) AS client_name, "
                        + " r.employee_id, "
                        + " emp.app_fullname AS employee_name, "
                        + " r.payee_bank_id, "
                        + " b2.bank_name AS payee_bank_name, "
                        + " r.payee_account_number, "
                        + " r.payee_account_name, "
                        + " r.payee_bank_branch, "
                        + " r.legal_entity_id, "
                        + " l1.legal_entity_name, "
                        + " r.business_unit_id, "
                        + " b1.organization_id, "
                        + " b1.unit_name AS business_unit_name, "
                        + " o1.organization_name, "
                        + " r.ca_status_id, "
                        + " s1.status_name AS cash_advance_status, "
                        + " d1.total_amount, " 
                        + " r.notes, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.cash_advance AS r "
                        + " LEFT OUTER JOIN dbo.legal_entity AS l1 ON r.legal_entity_id = l1.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS b1 ON r.business_unit_id = b1.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON b1.organization_id = o1.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u4 ON r.requested_by = u4.id "
                        + " LEFT OUTER JOIN  fn.purchase_order AS po ON r.purchase_order_id = po.id "
                        + " LEFT OUTER JOIN  fn.company AS v1 ON r.vendor_id = v1.id "
                        + " LEFT OUTER JOIN  pm.job_pa AS j1 ON r.job_id = j1.id "
                        + " LEFT OUTER JOIN  pm.job_detail AS j2 ON j1.job_detail_id = j2.id "
                        + " LEFT OUTER JOIN  fn.company AS cmp ON j2.company_id = cmp.id "
                        + " LEFT OUTER JOIN  fn.bank AS b2 ON r.payee_bank_id = b2.id "
                        + " LEFT OUTER JOIN  fn.cash_advance_status AS s1 ON r.ca_status_id = s1.id "
                        + @" LEFT OUTER JOIN (
                                SELECT 
                                    cash_advance_id, 
                                    SUM(COALESCE(amount, 0)) AS total_amount
                                FROM fn.cash_advance_detail
                                WHERE COALESCE(is_deleted, 0) = 0
                                GROUP BY cash_advance_id
                            ) AS d1 ON d1.cash_advance_id = r.id "
                        + " LEFT OUTER JOIN dbo.application_user AS emp ON r.employee_id = emp.id "

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

            DefaultViewColumns.Add("ca_number", "r.ca_number");
            DefaultViewColumns.Add("ca_date", "r.ca_date");
            DefaultViewColumns.Add("approval_id", "r.approval_id");
            DefaultViewColumns.Add("revision", "r.revision");
            DefaultViewColumns.Add("ca_period_from", "r.ca_period_from");
            DefaultViewColumns.Add("ca_period_to", "r.ca_period_to");            
            DefaultViewColumns.Add("requested_by", "r.requested_by");
            DefaultViewColumns.Add("requester_name", "u4.app_fullname");
            DefaultViewColumns.Add("requested_on", "r.requested_on");
            DefaultViewColumns.Add("purchase_order_id", "r.purchase_order_id");
            DefaultViewColumns.Add("purchase_order_number", "COALESCE(po.purchase_order_number, r.purchase_order_number)");
            DefaultViewColumns.Add("vendor_name", "COALESCE(v1.company_name, r.vendor_name)");
            DefaultViewColumns.Add("job_id", "r.job_id");
            DefaultViewColumns.Add("job_number", "COALESCE(j1.pa_number, r.job_number)");
            DefaultViewColumns.Add("client_name", "COALESCE(cmp.company_name, r.client_name)");
            DefaultViewColumns.Add("employee_id", "r.employee_id");
            DefaultViewColumns.Add("employee_name", "emp.app_fullname");

            DefaultViewColumns.Add("payee_bank_id", "r.payee_bank_id");
            DefaultViewColumns.Add("payee_bank_name", "b2.bank_name");
            DefaultViewColumns.Add("payee_account_number", "r.payee_account_number");
            DefaultViewColumns.Add("payee_account_name", "r.payee_account_name");
            DefaultViewColumns.Add("payee_bank_branch", "r.payee_bank_branch");

            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "l1.legal_entity_name");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("business_unit_name", "b1.unit_name");
            DefaultViewColumns.Add("organization_id", "b1.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");
            DefaultViewColumns.Add("ca_status_id", "r.ca_status_id");
            DefaultViewColumns.Add("cash_advance_status", "s1.status_name");
            DefaultViewColumns.Add("total_amount", "d1.total_amount");
            DefaultViewColumns.Add("notes", "r.notes");

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
            DefaultViewOrders.Add("ca_number", "DESC");
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
