using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class PayrollCompareableController : Controller
    {
        string url = ApiUrl.PayrollCompareableUrl;
        private readonly ApiClientFactory client;
        string menuUnique = "PAYROLLCOMPARABLE";
        public PayrollCompareableController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request, int month, int year)
        {
            var model = await client.Get<List<PayrollCompareable>>($"{url}/{month}/{year}") ?? new List<PayrollCompareable>();
            
            var IsAllowOpenEncryptEmployeePayroll = await client.Get<bool>($"{ApiUrl.ManageUserUrl}/IsAllowOpenEncryptEmployeePayroll/{menuUnique}");
            if (!IsAllowOpenEncryptEmployeePayroll)
            {
                foreach (var item in model)
                {
                    item.StringCurrentBasicSalary = "***********";
                    item.StringCurrentThp = "***********";
                    item.StringOneMonthBeforeBasicSalary = "***********";
                    item.StringOneMonthBeforeThp = "***********";
                    item.StringTwoMonthBeforeBasicSalary = "***********";
                    item.StringTwoMonthBeforeThp = "***********";
                }
            }
            else
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        if (!string.IsNullOrEmpty(item.StringCurrentBasicSalary))
                        {
                            item.StringCurrentBasicSalary = item.StringCurrentBasicSalary.ToString();
                        }
                        else
                        {
                            item.StringCurrentBasicSalary = "";
                        }


                        if (!string.IsNullOrEmpty(item.StringCurrentThp))
                        {
                            item.StringCurrentThp = item.StringCurrentThp.ToString();
                        }
                        else
                        {
                            item.StringCurrentThp = "";
                        }

                        if (!string.IsNullOrEmpty(item.StringOneMonthBeforeBasicSalary))
                        {
                            item.StringOneMonthBeforeBasicSalary = item.StringOneMonthBeforeBasicSalary.ToString();
                        }
                        else
                        {
                            item.StringOneMonthBeforeBasicSalary = "";
                        }

                        if (!string.IsNullOrEmpty(item.StringOneMonthBeforeThp))
                        {
                            item.StringOneMonthBeforeThp = item.StringOneMonthBeforeThp.ToString();
                        }
                        else
                        {
                            item.StringOneMonthBeforeThp = "";
                        }

                        if (!string.IsNullOrEmpty(item.StringTwoMonthBeforeBasicSalary))
                        {
                            item.StringTwoMonthBeforeBasicSalary = item.StringTwoMonthBeforeBasicSalary.ToString();
                        }
                        else
                        {
                            item.StringTwoMonthBeforeBasicSalary = "";
                        }

                        if (!string.IsNullOrEmpty(item.StringTwoMonthBeforeThp))
                        {
                            item.StringTwoMonthBeforeThp = item.StringTwoMonthBeforeThp.ToString();
                        }
                        else
                        {
                            item.StringTwoMonthBeforeThp = "";
                        }

                      
                    }
                } 
               
            }
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
    }
}