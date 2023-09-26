using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixERPWebApp
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent //: ViewComponent
    {
        private readonly ApiClientFactory client;
        private readonly IMemoryCache cache;

        public MenuViewComponent(ApiClientFactory client, IMemoryCache cache)
        {
            this.client = client;
            this.cache = cache;
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var models = await GetAllMenu();
        //    return Content(models);
        //}

        public async Task<string> GetAllMenu(string UserIdentityName, string area)
        {
            //var list = new List<Menu>();
            //var obj = HttpContext.Session.Get(nameof(GetAllMenu)).ToObject();

            //if (obj == null)
            //{
            //    list = await client.Get<List<Menu>>(ApiUrl.MenuPrivilegeUrl);

            //    if (list != null)
            //    {
            //        HttpContext.Session.Set(nameof(GetAllMenu), list.ToByteArray());
            //    }
            //}
            //else
            //{
            //    list = obj as List<Menu>;
            //}

            //if (list == null || list.Count == 0) return "";

            //return CreateMenu(list);

            var menus = new List<Menu>();
            if (!cache.TryGetValue(UserIdentityName+ "-Menu-Privilege", out menus))
            {
                menus = await client.Get<List<Menu>>(ApiUrl.MenuPrivilegeUrl);

                var options = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(8),
                    AbsoluteExpiration = DateTime.Today.AddHours(8)
                };

                cache.Set(UserIdentityName+ "-Menu-Privilege", menus, options);
            }

            if (menus == null || menus.Count == 0) return "";
            return CreateMenu(menus, area);
        }

        private string CreateMenu(List<Menu> menus, string area)
        {
            //var area = ViewContext.RouteData.Values["area"].ToSafeString();
            var id = "";
            var sid = "65aca65f-edc8-4611-8d45-9a01fb2c90fa";
            var home = "";

            switch (area.ToLower())
            {
                case "pm":
                case "projectmanagement":
                    id = "3ad4fcc2-c1cd-4337-8625-341bce401a02";
                    home = "/projectmanagement";
                    break;
                case "finance":
                case "core":
                case "approvalfinance":
                    id = "4f02b731-680b-4719-beba-08836cb65c3a";
                    home = "/finance/home";
                    break;
                case "hris":
                case "hrisnew":
                case "company":
                    id = "abf71d22-073e-496e-a0ec-8db53410744f";
                    home = "/hris";
                    break;
                case "media":
                    id = "6441f398-68b9-43e9-9feb-39b4db712c7e";
                    home = "/media/home";
                    break;
                case "um":
                case "settings":
                    sid = "65aca65f-edc8-4611-8d45-9a01fb2c90fa";
                    home = "/um/home";
                    break;
                default:
                    break;
            }

            menus = menus.Where(x => x.Id.Equals(id) || x.Id.Equals(sid)).ToList();

            //if (menus == null || menus.Count == 0)
            //    return "";

            var m = new StringBuilder();

            GetMenuLink(menus, false, ref m);

            var html =
                @"<div id=""m_ver_menu"" class=""m-aside-menu  m-aside-menu--skin-dark m-aside-menu--submenu-skin-dark"" m-menu-vertical=""1"" m-menu-scrollable=""1"" m-menu-dropdown-timeout=""500"" style=""position: relative;display:none;"">
	                <ul class=""m-menu__nav"">
                        <li class=""m-menu__item"">
                            <a href=""" + home + @""" class=""m-menu__link"">
                                <i class=""m-menu__link-icon flaticon-home""></i>
                                <span class=""m-menu__link-text"">Home</span>
                            </a>
                        </li>
                        %menu%
	                </ul>
                </div>";
            var xyz = html.Replace("%menu%", m.ToString());
            return html.Replace("%menu%", m.ToString());
        }

        private void GetMenuLink(IEnumerable<Menu> list, bool isSubMenu, ref StringBuilder m)
        {
            foreach (var current in list.OrderBy(x => x.MenuIndex))
            {
                if (current.Children.Count == 0)
                {
                    if (!isSubMenu)
                        m.AppendLine(CreateParentMenu(current));
                    else
                        m.AppendLine(CreateChildMenu(current));

                }
                else
                {
                    var html = $@"
                    <li class=""m-menu__item  m-menu__item--submenu"" aria-haspopup=""true"" m-menu-submenu-toggle=""hover"">
                        <a href=""javascript:;"" class=""m-menu__link m-menu__toggle"">
                            <i class=""m-menu__link-icon flaticon-price-tag""></i>
                            <span class=""m-menu__link-text translate"" data-args=""{current.MenuName}"">{current.MenuName}</span>
                            <i class=""m-menu__ver-arrow la la-angle-right""></i>
                        </a>
                        <div class=""m-menu__submenu"">
                            <span class=""m-menu__arrow""></span>
                            <ul class=""m-menu__subnav"">";

                    m.AppendLine(html);

                    GetMenuLink(current.Children.OrderBy(x => x.MenuName), true, ref m);

                    var end = $@"</ul></div></li>";
                    m.AppendLine(end);
                }
            }
        }

        private string CreateParentMenu(Menu menu)
        {
            return $@"
                <li class=""m-menu__item"">
                    <a href=""{menu.MenuLink}"" class=""m-menu__link"">
                        <i class=""m-menu__link-icon flaticon-calendar-with-a-clock-time-tools""></i>
                        <span class=""m-menu__link-text translate"" data-args=""{menu.MenuName}"">{menu.MenuName}</span>
                    </a>
                </li>";
        }

        private string CreateChildMenu(Menu menu)
        {
            return $@"
                <li class=""m-menu__item"" aria-haspopup=""true"">
					<a href=""{menu.MenuLink}"" class=""m-menu__link"">
						<i class=""m-menu__link-bullet m-menu__link-bullet--dot""><span></span></i>
						<span class=""m-menu__link-text"">{menu.MenuName}</span>
					</a>
				</li>";
        }
    }
}
