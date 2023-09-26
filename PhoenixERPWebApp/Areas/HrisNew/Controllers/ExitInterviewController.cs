using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.WebExtension.Extentions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{
    [Area(nameof(HrisNew))]
    public class ExitInterviewController : Controller
    {
        string url = ApiUrl.ExitInterviewnUrl;
        private readonly ApiClientFactory client;

        public ExitInterviewController(ApiClientFactory client)
        {
            this.client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddEdit(string terminationId)
        {
            var model = await client.Get<Termination>($"{url}/{terminationId}");
            return View(model);
        }

        public async Task<IActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = await client.Get<List<Termination>>(url) ?? new List<Termination>();
            var data = model.ToDataSourceResult(request);
            return Json(data, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }


        [HttpPost]
        public async Task<IActionResult> Save(Termination model)
        {
            model.IsInterview = true;
            return await Process(model, true);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Termination model)
        {
            model.StatusInterview = TerminationInterviewStatus.Approved;
            model.IsInterview = true;
            model.ExitInterviews = new List<ExitInterview>();
            return await Process(model, false);
        }

        private async Task<IActionResult> Process(Termination model, bool isDraft)
        {
            model.ExitInterviews = new List<ExitInterview>();
            var termination = await client.Get<Termination>($"{url}/{model.Id}");
            foreach (var category in termination.CategoryExitInterviews.OrderBy(x => x.No))
            {
                if (termination.ExitInterviews.Where(x => x.CategoryExitInterviewId == category.Id && x.Type == InputType.Radiobutton).Count() > 0)
                {
                    foreach (var item in termination.ExitInterviews.Where(x => x.CategoryExitInterviewId == category.Id).OrderBy(x => x.Type))
                    {
                        if (item.Type == InputType.Radiobutton)
                        {
                            if (string.IsNullOrEmpty(model.Id))
                            {
                                item.Id = Guid.NewGuid().ToString();
                            }

                            item.TerminationId = model.Id;
                            item.CategoryExitInterviewId = category.Id;
                            item.Result = Request.Form[$"{item.ExitInterviewItemId}"];
                            model.ExitInterviews.Add(item);
                        }
                    }
                }

                foreach (var item in termination.ExitInterviews.Where(x => x.CategoryExitInterviewId == category.Id).OrderBy(x => x.Type))
                {
                    if (item.Type == InputType.Checkbox || item.Type == InputType.TextArea)
                    {
                        if (string.IsNullOrEmpty(model.Id))
                        {
                            item.Id = Guid.NewGuid().ToString();
                        }

                        item.TerminationId = model.Id;
                        item.CategoryExitInterviewId = category.Id;
                        if (item.Type == InputType.Checkbox)
                        {
                            item.Result = (Request.Form[item.ExitInterviewItemId].Count() > 0).ToString();//Convert.ToBoolean(Request.Form[item.ExitInterviewItemId][0]).ToString();
                        }
                        else
                        {
                            item.Result = Request.Form[item.ExitInterviewItemId];
                        }

						//the property 'approvedby' on entity type 'exitinterview' has a temporary value. either set a permanent value explicitly or ensure that the database is configured to generate values for this property.
						//coba pasang value approveby
						model.ApprovedBy = null;
						model.ApprovedOn = null;
						model.ExitInterviews.Add(item);
                    }
                }
            }

            var response = await client.PutApiResponse<Termination>(url, model);
            if (isDraft)
            {
                return Json(new { success = response.Success, message = response.Message, data = response.Data as Termination });
            }
            else
            {
                return Json(new { success = response.Success, message = response.Message, url = Url.Action(nameof(Index)) });
            }
        }
    }
}