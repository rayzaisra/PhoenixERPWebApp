using CommonTool;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DataAccess.PhoenixERP.General
{
    public partial class team_member : IEntity, IBaseRecord
    {
        #region Default IBaseRecord

        public static readonly Boolean IsConstant;

        public static readonly String EntityId;

        public static readonly String EntityName;

        public static readonly String EntityDisplayName;

        public static readonly String DefaultView;

        public static readonly Dictionary<String, String> DefaultViewColumns;

        public static readonly Dictionary<String, String> DefaultViewColumnTitles;

        public static readonly Dictionary<String, String> DefaultViewOrders;

        public static readonly Dictionary<String, dynamic> FilterDynamicBuilder;

        #endregion

        static team_member()
        {
            IsConstant = false;
            EntityName = team_member.GetTableName();
            EntityDisplayName = team_member.GetTableName(true);
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

                        + " r.team_id, "
                        + " t1.team_name, "
                        + " r.application_user_id, "
                        + " u4.app_fullname AS team_member_name, "
                        + " t1.organization_id, "
                        + " o1.organization_name, "

                        + " u0.app_fullname AS record_created_by, "
                        + " u1.app_fullname AS record_modified_by, "
                        + " u2.app_fullname AS record_approved_by, "
                        + " u3.app_fullname AS record_owner, "
                        + " t.team_name AS record_owning_team "
                        + " FROM "
                        + " dbo.team_member AS r "
                        + " LEFT OUTER JOIN dbo.team AS t1 ON t1.id = r.team_id "
                        + " LEFT OUTER JOIN dbo.organization AS o1 ON o1.id = t1.organization_id "
                        + " LEFT OUTER JOIN dbo.application_user AS u4 ON u4.id = r.application_user_id "
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
            
            DefaultViewColumns.Add("team_id", "r.team_id");
            DefaultViewColumns.Add("team_name", "r.team_name");
            DefaultViewColumns.Add("application_user_id", "r.application_user_id");
            DefaultViewColumns.Add("team_member_name", "u4.app_fullname");
            DefaultViewColumns.Add("organization_id", "r.organization_id");
            DefaultViewColumns.Add("organization_name", "o1.organization_name");

            DefaultViewColumns.Add("record_created_by", "u0.app_fullname");
            DefaultViewColumns.Add("record_modified_by", "u1.app_fullname");
            DefaultViewColumns.Add("record_approved_by", "u2.app_fullname");
            DefaultViewColumns.Add("record_owner", "u3.app_fullname");
            DefaultViewColumns.Add("record_owning_team", "t.team_name");

            DefaultViewColumnTitles = new Dictionary<String, String>();
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            foreach (var c in DefaultViewColumns)
            {
                DefaultViewColumnTitles.Add(c.Key, ti.ToTitleCase(c.Key.Replace("_", " ")));
            }

            DefaultViewOrders = new Dictionary<String, String>();
            DefaultViewOrders.Add("unit_name", "ASC");
        }

        public Boolean CheckIfConstant()
        {
            return team_member.IsConstant;
        }

        public String GetEntityId()
        {
            return EntityId;
        }

        public String GetEntityName()
        {
            return EntityName;
        }

        public String GetEntityDisplayName()
        {
            return EntityDisplayName;
        }

        public String GetDefaultView()
        {
            return DefaultView;
        }

        public Dictionary<String, String> GetDefaultViewColumns()
        {
            return DefaultViewColumns;
        }

        public Dictionary<String, String> GetDefaultViewColumnTitles()
        {

            return DefaultViewColumnTitles;
        }

        public Dictionary<String, String> GetDefaultViewOrders()
        {
            return DefaultViewOrders;
        }

        public Dictionary<String, dynamic> GetFilterDynamicBuilder()
        {
            return FilterDynamicBuilder;
        }
    }
}
