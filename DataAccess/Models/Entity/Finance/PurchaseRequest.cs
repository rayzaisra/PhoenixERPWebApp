//using CommonTool;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTool;
using DataAccess;
using Newtonsoft.Json;

namespace DataAccess.PhoenixERP.Finance
{
    public partial class purchase_request : IEntity, IBaseRecord
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


        static purchase_request()
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

                        + " r.is_posted, "
                        + " r.posted_on, "
                        + " r.request_datetime, "
                        + " r.request_number, "
                        + " r.business_unit_id, "
                        + " b1.unit_name, "
                        + " r.legal_entity_id, "
                        + " l1.legal_entity_name, "
                        + " r.affiliation_id, "
                        + " a1.affiliation_name, "
                        + " r.task_id, "
                        + " r.subtask_id, "
                        + " r.account_id, "
                        + " r.is_private_vendor, "
                        + " r.remarks, "
                        + " r.organization_id, "
                        + " o1.organization_name, "
                        + " COALESCE(prd.total_amount, 0) AS total_amount, "
                        + " CONCAT('[', STUFF(( "
                        + " SELECT "
                        + " ', {\"id\":\"' + rfq.id + '\",\"rfq_number\":\"' + rfq.request_for_quotation_number + '\"}' "
                        + " FROM "
                        + " fn.request_for_quotation rfq "
                        + " WHERE "
                        + " rfq.purchase_request_id = r.id FOR XML PATH('')),  "
                        + " 1, 1, ''),']') AS rfq_list, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "
                        + " FROM fn.purchase_request AS r "
                        + " LEFT OUTER JOIN( "
                        + " SELECT prd.purchase_request_id, SUM(prd.amount)AS total_amount FROM fn.purchase_request_detail prd "
                        + " WHERE prd.is_active = 1 AND COALESCE (prd.is_deleted, 0) = 0 "
                        + " GROUP BY prd.purchase_request_id "
                        + " ) AS prd ON prd.purchase_request_id = r.id " 

                        + " LEFT OUTER JOIN dbo.business_unit AS b1 "
                        + " ON r.business_unit_id = b1.id "
                        + " LEFT OUTER JOIN dbo.legal_entity AS l1 "
                        + " ON r.legal_entity_id = l1.id "
                        + " LEFT OUTER JOIN dbo.affiliation AS a1 "
                        + " ON r.affiliation_id = a1.id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 "
                        + " ON r.organization_id = o1.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u0 "
                        + " ON r.created_by = u0.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u1 "
                        + " ON r.modified_by = u1.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u2 "
                        + " ON r.approved_by = u2.id "
                        + " LEFT OUTER JOIN dbo.application_user AS u3 "
                        + " ON r.owner_id = u3.id  "
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
            DefaultViewColumns.Add("is_active", "r.is_active");
            DefaultViewColumns.Add("is_locked", "r.is_locked");
            DefaultViewColumns.Add("is_default", "r.is_default");
            DefaultViewColumns.Add("owner_id", "r.owner_id");

            DefaultViewColumns.Add("is_posted", "r.is_posted");
            DefaultViewColumns.Add("posted_on", "r.posted_on");
            DefaultViewColumns.Add("request_datetime", "r.request_datetime");
            DefaultViewColumns.Add("request_number", "r.request_number");
            DefaultViewColumns.Add("business_unit_id", "r.business_unit_id");
            DefaultViewColumns.Add("unit_name", "b1.unit_name");
            DefaultViewColumns.Add("legal_entity_id", "r.legal_entity_id");
            DefaultViewColumns.Add("legal_entity_name", "l1.legal_entity_name");
            DefaultViewColumns.Add("affiliation_id", "r.affiliation_id");
            DefaultViewColumns.Add("affiliation_name", "a1.affiliation_name");
            DefaultViewColumns.Add("task_id", "r.task_id");
            DefaultViewColumns.Add("subtask_id", "r.subtask_id");
            DefaultViewColumns.Add("account_id", "r.account_id");
            DefaultViewColumns.Add("is_private_vendor", "r.is_private_vendor");
            DefaultViewColumns.Add("remarks", "r.remarks");
            DefaultViewColumns.Add("organization_id", "r.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");
            DefaultViewColumns.Add("total_amount", "COALESCE(prd.total_amount, 0)");
            //DefaultViewColumns.Add("request_for_quotation_id", "rfq.id");
            //DefaultViewColumns.Add("request_for_quotation_number", "rfq.request_for_quotation_number");
            
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
            DefaultViewOrders.Add("request_datetime", "DESC");
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
