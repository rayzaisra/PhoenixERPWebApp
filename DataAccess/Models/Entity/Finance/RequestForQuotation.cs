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
    public partial class request_for_quotation : IEntity, IBaseRecord
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

        static request_for_quotation()
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

                        + " r.internal_rfq_id, "
                        + " r.request_for_quotation_number, "
                        + " r.request_for_quotation_date, "
                        + " r.purchase_request_id, "
                        + " pr.request_number, "
                        + " r.legal_entity_id, "
                        + " leg.legal_entity_name, "
                        + " r.business_unit_id, "
                        + " bu.unit_name AS business_unit_name, "
                        + " r.affiliation_id, "
                        + " aff.affiliation_name, "
                        + " leg.organization_id, "
                        + " o1.organization_name, "
                        + " r.brand_name, "
                        + " r.currency_id, "
                        + " ccy.currency_code, "
                        + " ccy.currency_name, "
                        + " r.exchange_rate, "
                        + " r.payment_type_id, "
                        + " pt.type_name AS payment_type_name, "
                        + " r.delivery_place, "
                        + " r.delivery_date, "
                        + " r.closing_date, "
                        + " r.term_of_payment, "
                        + " r.job_id, "
                        + " pa.id as job_pa_id, "
                        + " pa.code as pa_number, "
                        + " pe.id as job_pe_id, "
                        + " pe.code as pe_number, "
                        + " r.is_prefered_vendor, "
                        + " r.approval_id, "
                        + " apv.approval_number, "
                        + " CONCAT('[', STUFF "
                        + "  ((SELECT "
                        + "    ', {\"id\":\"' + rfq_v.vendor_id + '\",\"company_name\":\"' + c.company_name + '\", \"quotation_id\": ' + CASE WHEN vq.id is null THEN  'null' ELSE CONCAT('\"', vq.id, '\"') END + ' }' "
                        + "  FROM fn.request_for_quotation_vendor rfq_v "
                        + "  LEFT OUTER JOIN fn.company c "
                        + "    ON c.id = rfq_v.vendor_id "
                        + "    LEFT OUTER JOIN fn.vendor_quotation vq ON vq.id =[fn].[ufn_get_vendor_quotation_id](rfq_v.request_for_quotation_id, rfq_v.vendor_id) "

                        + "  WHERE rfq_v.request_for_quotation_id = r.id "
                        + "  FOR xml PATH('')), 1, 1, ''), ']') AS vendor_list, "


                        + " CONCAT('[', STUFF "
                        + "  ((SELECT "
                        + "  ', {\"id\":\"' + rfq_t.id + '\",\"task\":\"' + rfq_t.task + '\", \"subtask\": \"' + rfq_t.subtask + '\", \"quantity\": \"' + CASE WHEN rfq_t.quantity is null THEN  'null' ELSE CONCAT('\"', rfq_t.quantity, '\"') END + ' }'"
                        + " FROM fn.request_for_quotation_task rfq_t "
                        + " WHERE rfq_t.request_for_quotation_id = r.id "
                        + " FOR xml PATH('')), 1, 1, ''), ']') AS task_list, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "

                        + " FROM             fn.request_for_quotation AS r "
                        + " LEFT OUTER JOIN  fn.internal_rfq AS irfq ON r.internal_rfq_id = irfq.id "
                        + " LEFT OUTER JOIN fn.purchase_request AS pr ON r.purchase_request_id = pr.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS leg ON r.legal_entity_id = leg.id "
                        + " LEFT OUTER JOIN dbo.business_unit AS bu ON r.business_unit_id = bu.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS aff ON r.affiliation_id = aff.id "
                        + " LEFT OUTER JOIN dbo.currency AS ccy ON r.currency_id = ccy.id "
                        + " LEFT OUTER JOIN  fn.payment_type AS pt ON r.payment_type_id = pt.id "
                        + " LEFT OUTER JOIN pm.pca AS pa ON r.job_id = pa.job_id "
                        + " LEFT OUTER JOIN pm.pce AS pe  ON r.job_id = pe.job_id "
                        + " LEFT OUTER JOIN dbo.approval AS apv ON r.approval_id = apv.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON leg.organization_id = o1.id "
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

            DefaultViewColumns.Add("internal_rfq_id", "r.internal_rfq_id");
            DefaultViewColumns.Add("request_for_quotation_number", "r.request_for_quotation_number");
            DefaultViewColumns.Add("request_for_quotation_date", "r.request_for_quotation_date");
            DefaultViewColumns.Add("purchase_request_id", "r.purchase_request_id");
            DefaultViewColumns.Add("request_number", "pr.request_number");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "leg.legal_entity_name");
            DefaultViewColumns.Add("organization_id", "leg.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("business_unit_name", "bu.unit_name");
            DefaultViewColumns.Add("affiliation_id", "r.affiliation_id");
            DefaultViewColumns.Add("affiliation_name", "aff.affiliation_name");
            DefaultViewColumns.Add("brand_name", "r.brand_name");
            DefaultViewColumns.Add("currency_id", "r.currency_id");
            DefaultViewColumns.Add("currency_code", "ccy.currency_code");
            DefaultViewColumns.Add("currency_name", "ccy.currency_name");
            DefaultViewColumns.Add("exchange_rate", "r.exchange_rate");
            DefaultViewColumns.Add("payment_type_id", "r.payment_type_id");
            DefaultViewColumns.Add("payment_type_name", "pt.type_name");
            DefaultViewColumns.Add("delivery_place", "r.delivery_place");
            DefaultViewColumns.Add("delivery_date", "r.delivery_date");
            DefaultViewColumns.Add("closing_date", "r.closing_date");
            DefaultViewColumns.Add("term_of_payment", "r.term_of_payment");

            DefaultViewColumns.Add("job_id", "r.job_id");
            DefaultViewColumns.Add("job_pa_id", "r.job_pa_id");
            DefaultViewColumns.Add("pa_number", "pa.pa_number");
            DefaultViewColumns.Add("job_pe_id", "r.job_pe_id");
            DefaultViewColumns.Add("pe_number", "pe.pe_number");
            DefaultViewColumns.Add("is_prefered_vendor", "r.is_prefered_vendor");
            DefaultViewColumns.Add("approval_id", "r.approval_id");
            DefaultViewColumns.Add("approval_number", "apv.approval_number");

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
            DefaultViewOrders.Add("request_for_quotation_number", "ASC");
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
