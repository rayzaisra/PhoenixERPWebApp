using CodeMarvel.Infrastructure;
using CodeMarvel.Infrastructure.Options;
using DataAccess.PhoenixERP;
using DataAccess.PhoenixERP.General;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPoco;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace BusinessLogic.Services.General
{
    public partial class ApplicationMenuService
    {
        public List<dynamic> GetDetailMenuByModuleId(String ApplicationModuleId)
        {
            List<dynamic> results = new List<dynamic>();

            try
            {
                var data = application_menu.Fetch(" WHERE application_module_id=@0 AND parent_menu_id is null AND organization_id=@1 order by order_index asc", ApplicationModuleId, Context.OrganizationId);
                List<string> childNodes = null;
                foreach (var item in data)
                {
                    dynamic menu = new ExpandoObject();
                    menu.id = item.id;
                    menu.url = item.menu_name;
                    menu.text = item.menu_name;
                    menu.is_group = item.is_group ?? false;
                    menu.icon = item.is_group ?? false == true ? "fa fa-dot-circle m--font-info" :
                        string.Format("{0} {1}", String.IsNullOrEmpty(item.icon_class) ? "fa fa-briefcase" : item.icon_class, "m--font-success ");
                    menu.state = new ExpandoObject();
                    menu.state.opened = false;
                    menu.state.disabled = false;
                    menu.state.selected = false;

                    menu.children = GetDetailMenuByParentId(item.id, ref childNodes, item.is_group ?? false); // array of strings or objects
                    menu.li_attr = new ExpandoObject(); //attributes for the generated LI node
                    menu.a_attr = new ExpandoObject(); // attributes for the generated A node
                    results.Add(menu);
                    appLogger.Debug(JsonConvert.SerializeObject(menu));
                }
                appLogger.Debug(JsonConvert.SerializeObject(results));
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            return results;
        }

        protected List<dynamic> GetDetailMenuByParentId(String parentId, ref List<string> childNodes, bool isGroup)
        {
            List<dynamic> results = new List<dynamic>();
            if (childNodes == null)
            {
                childNodes = new List<string>();
                childNodes.Add(parentId);
            }

            try
            {
                if (string.IsNullOrEmpty(parentId)) return results;
                var data = application_menu.Fetch(" WHERE parent_menu_id=@0 order by order_index asc", parentId);
                appLogger.Debug(application_menu.repo.LastCommand);
                foreach (var item in data)
                {
                    if (childNodes.Contains(item.id)) continue;
                    dynamic menu = new ExpandoObject();
                    menu.id = item.id;
                    menu.url = item.action_url;
                    menu.parent = item.parent_menu_id;
                    menu.text = item.menu_name;
                    menu.is_group = item.is_group ?? false;
                    menu.icon = null;
                    if (isGroup)
                    {
                        menu.icon = item.is_group ?? false == true ? "fa fa-dot-circle m--font-info" :
                        string.Format("{0} {1}", String.IsNullOrEmpty(item.icon_class) ? "fa fa-briefcase" : item.icon_class, "m--font-success ");
                    }
                    menu.myParentIsGroup = isGroup;
                    menu.state = new ExpandoObject();
                    menu.state.opened = false;
                    menu.state.disabled = false;
                    menu.state.selected = false;
                    menu.children = new object(); // array of strings or objects
                    if (item.parent_menu_id != null)
                    {
                        menu.children = GetDetailMenuByParentId(item.id, ref childNodes, false);
                    }
                    menu.li_attr = new ExpandoObject(); //attributes for the generated LI node
                    menu.a_attr = new ExpandoObject(); // attributes for the generated A node
                    results.Add(menu);
                }
            }
            catch (Exception ex)
            {
                appLogger.Error(ex);
                throw;
            }
            return results;
        }

        public bool OrderMenu(String recordId, String parentTargetId, int newOrder)
        {
            var result = false;
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    db.Execute(";EXEC usp_menu_order @@id = @0 , @@order_index = @1 ,@@parent_menu_id = @2",
                                recordId, newOrder, parentTargetId);
                    result = true;
                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result;

        }

        public String GenerateMenu()
        {
            StringBuilder result = new StringBuilder();
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    var context = StaticHttpContextAccessor.Current;
                    var appSetting = context.RequestServices.GetService<IOptions<AppSettingsOption>>().Value;
                    var qry = application_menu.DefaultView;
                    qry += " WHERE r.id IS NOT NULL AND r.parent_menu_id IS NULL AND r.is_active=1 ";
                    var sql = Sql.Builder.Append(qry);
                    sql.Append(" ORDER BY r.order_index ASC");

                    var data = db.Fetch<vw_application_menu>(sql);
                    List<string> childNodes = null;
                    List<string> ParentNodes = new List<string>();


                    foreach (var item in data)
                    {
                        if (item.is_group ?? false)
                        {
                            if (ParentNodes.Contains(item.id)) continue;
                            ParentNodes.Add(item.id);
                            result.Append("<li class='m-menu__section '>");
                            result.Append(string.Format("<h4 class='m-menu__section-text'>{0}</h4>", item.menu_name));
                            result.Append("<i class='m-menu__section-icon flaticon-more-v2'></i>");
                            result.Append("</li>");

                            //var childGroup = db.ExecuteScalar<int>("SELECT count(id) as total FROM dbo.application_menu WHERE parent_menu_id = @0 AND is_active=1 ", item.id);
                            var Groupchilds = db.Fetch<application_menu>(" WHERE parent_menu_id = @0 AND is_active=1 ",item.id);
                            foreach (var Groupchild in Groupchilds)
                            {

                                var child = db.ExecuteScalar<int>("SELECT count(id) as total FROM dbo.application_menu WHERE parent_menu_id = @0 AND is_active=1 ", Groupchild.id);
                                if (child > 0)
                                {
                                    result.Append("<li class='m-menu__item  m-menu__item--submenu' aria-haspopup='true' m-menu-submenu-toggle='hover'>");
                                    result.Append(string.Format("<a href='javascript:;' class='m-menu__link m-menu__toggle'>"));
                                    result.Append(string.Format("<i class='m-menu__link-icon {0}' ></i>", Groupchild.icon_class));
                                    result.Append(string.Format("<span class='m-menu__link-text'>{0}</span>", Groupchild.menu_name));
                                    result.Append("<i class='m-menu__ver-arrow la la-angle-right'></i>");
                                    result.Append("</a>");

                                    result.Append("<div class='m-menu__submenu '>");
                                    result.Append("<span class='m-menu__arrow'></span>");
                                    result.Append("<ul class='m-menu__subnav'>");

                                    result.Append(Child(Groupchild.id, ref childNodes));

                                    result.Append("</ul>");
                                    result.Append("</div>");


                                    result.Append("</li>");
                                }
                                else
                                {
                                    result.Append("<li class='m-menu__item' aria-haspopup='true'>");
                                    result.Append(string.Format("<a href='{0}{1}' class='m-menu__link ' title=''>", appSetting.WebServerUrl, Groupchild.action_url));
                                    result.Append(string.Format("<i class='{0}'><span></span></i>", "m-menu__link-bullet m-menu__link-bullet--dot"));
                                    result.Append(string.Format("<span class='m-menu__link-text'>{0}</span>", Groupchild.menu_name));
                                    result.Append("</a>");
                                    result.Append("</li>");
                                }
                            }

                        }
                        else
                        {
                            var child = db.ExecuteScalar<int>("SELECT count(id) as total FROM dbo.application_menu WHERE parent_menu_id = @0 AND is_active=1 ", item.id);
                            if (child > 0)
                            {
                                result.Append("<li class='m-menu__item  m-menu__item--submenu' aria-haspopup='true' m-menu-submenu-toggle='hover'>");
                                result.Append(string.Format("<a href='javascript:;' class='m-menu__link m-menu__toggle'>"));
                                result.Append(string.Format("<i class='m-menu__link-icon {0}' ></i>", item.icon_class));
                                result.Append(string.Format("<span class='m-menu__link-text'>{0}</span>", item.menu_name));
                                result.Append("<i class='m-menu__ver-arrow la la-angle-right'></i>");
                                result.Append("</a>");

                                result.Append("<div class='m-menu__submenu '>");
                                result.Append("<span class='m-menu__arrow'></span>");
                                result.Append("<ul class='m-menu__subnav'>");

                                result.Append(Child(item.id, ref childNodes));

                                result.Append("</ul>");
                                result.Append("</div>");


                                result.Append("</li>");
                            }
                            else
                            {
                                result.Append("<li class='m-menu__item ' aria-haspopup='true' m-menu-link-redirect='1' >");
                                //result.Append(string.Format("<a href='{0}' class='m-menu__link ' title='{1}'>", item.action_url, item.menu_name));
                                result.Append(String.Format("<a href='{0}{1}' class='m-menu__link '>", appSetting.WebServerUrl, item.action_url));
                                result.Append(string.Format("<i class='m-menu__link-icon  {0}'></i>", item.icon_class));
                                result.Append(string.Format("<span class='m-menu__link-text'>{0}</span>", item.menu_name));
                                result.Append("</a>");
                                result.Append("</li>");
                            }
                        }






                    }

                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result.ToString();
        }

        private String Child(string parentID, ref List<string> ChildNodes)
        {
            var context = StaticHttpContextAccessor.Current;
            var appSetting = context.RequestServices.GetService<IOptions<AppSettingsOption>>().Value;
            StringBuilder result = new StringBuilder();
            if (ChildNodes == null)
            {
                ChildNodes = new List<string>();
                ChildNodes.Add(parentID);
            }
            using (var db = new PhoenixERPRepo())
            {
                try
                {
                    if (string.IsNullOrEmpty(parentID)) return result.ToString();
                    var qry = application_menu.DefaultView;
                    qry += " WHERE r.id IS NOT NULL AND r.parent_menu_id IS NOT NULL AND r.is_active=1 ";
                    var sql = Sql.Builder.Append(qry);
                    sql.Append(" AND r.parent_menu_id=@0 ", parentID);
                    sql.Append(" ORDER BY r.order_index ASC");

                    var data = db.Fetch<vw_application_menu>(sql);

                    foreach (var item in data)
                    {
                        if (ChildNodes.Contains(item.id)) continue;
                        var child = db.ExecuteScalar<int>("SELECT count(id) as total FROM application_menu WHERE parent_menu_id = @0 AND is_active=1", item.id);
                        if (child > 0)
                        {
                            result.Append("<li class='m-menu__item  m-menu__item--submenu' aria-haspopup='true' m-menu-submenu-toggle='hover' m-menu-link-redirect='1'>");
                            result.Append(string.Format("<a href='javascript:;' class='m-menu__link m-menu__toggle'>"));
                            result.Append(string.Format("<i class='{0}'><span></span></i>", "m-menu__link-bullet m-menu__link-bullet--dot"));
                            result.Append(string.Format("<span class='m-menu__link-text'>{0}</span>", item.menu_name));
                            result.Append("<i class='m-menu__ver-arrow la la-angle-right'></i>");
                            result.Append("</a>");

                            result.Append("<div class='m-menu__submenu '>");
                            result.Append("<span class='m-menu__arrow'></span>");
                            result.Append("<ul class='m-menu__subnav'>");

                            result.Append(Child(item.id, ref ChildNodes));

                            result.Append("</ul>");
                            result.Append("</div>");


                            result.Append("</li>");
                        }
                        else
                        {
                            result.Append("<li class='m-menu__item ' aria-haspopup='true' m-menu-link-redirect='1' >");
                            result.Append(String.Format("<a href='{0}{1}' class='m-menu__link '>", appSetting.WebServerUrl, item.action_url));
                            result.Append(string.Format("<i class='{0}'><span></span></i>", "m-menu__link-bullet m-menu__link-bullet--dot"));
                            result.Append(string.Format("<span class='m-menu__link-text '> {0}</span>", item.menu_name));
                            result.Append("</a>");
                            result.Append("</li>");
                        }
                    }
                    //result.Append("</ul>");


                }
                catch (Exception ex)
                {
                    appLogger.Error(ex);
                }
            }
            return result.ToString();
        }



    }
}
