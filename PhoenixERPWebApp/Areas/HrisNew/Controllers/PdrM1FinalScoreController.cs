using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CodeMarvel.Infrastructure.ModelShared;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NPOI.SS.Formula.Functions;
using Phoenix.WebExtension.RestApi;
using PhoenixERPWebApp.Models;

namespace PhoenixERPWebApp.Areas.HrisNew.Controllers
{

    [Area(nameof(HrisNew))]
    public class PdrM1FinalScoreController : Controller
    {
        string url = ApiUrl.PdrM1FinalScoreUrl;
        private readonly ApiClientFactory client;
        private readonly IMemoryCache cache;

        public PdrM1FinalScoreController(ApiClientFactory client, IMemoryCache cache)
        {
            this.client = client;
            this.cache = cache;
        }


        public IActionResult Index()
        {
            var model = new PdrApprailsalM1Header();
            model.infoEmployee = new InfoEmployee();
            ViewBag.m1Up = model.infoEmployee.Grade == "M1" || model.infoEmployee.Grade == "E1" || model.infoEmployee.Grade == "E2" || model.infoEmployee.Grade == "E3" || model.infoEmployee.Grade == "BOD";
            return View(model);
        }

        public async Task<IActionResult> ReadPdrApprailsalM1([DataSourceRequest] DataSourceRequest request, string employeeId, DateTime? startReview)
        {
            if (employeeId == null) employeeId = "0";
            if (startReview == null) startReview = DateTime.Now;
            var model = await client.Get<List<PdrApprailsalM1>>($"{url}/getPdrApprailsalM1List/{employeeId}/{Convert.ToDateTime(startReview).ToString("yyyy-MM-dd")}") ?? new List<PdrApprailsalM1>();

            return Json(model.ToDataSourceResult(request), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public async Task<IActionResult> ReadPdrPerformanceM1(string employeeId, DateTime? startReview)
        {


            var model = await client.Get<PdrEmployee>($"{url}/getPdrPerformancelM1List/{employeeId}/{Convert.ToDateTime(startReview).ToString("yyyy-MM-dd")}");
            if (model != null)
            {
                if (model.PdrRecomendation == null) model.PdrRecomendation = new PdrRecomendation();
                if (model.PdrDetailM2belows == null) model.PdrDetailM2belows = new List<PdrDetailM2below>();
                if (model.PdrOverallScores == null) model.PdrOverallScores = new List<PdrOverallScore>();
                model.Notes = HttpUtility.HtmlDecode(model.Notes);

                decimal? finalScore = 0;
                foreach (var item in model.PdrCategorys.OrderBy(x => x.No))
                {
                    if (item.No == 3)
                    {
                        var modelPDRCategory = model.PdrCategorys.Where(x => x.PdrParameters.Count > 0).OrderBy(x => x.No).ToList();

                        var poor = model.PdrGrades.Where(x => x.Id == PhoenixERPWebApp.Models.Grades.Poor).FirstOrDefault();
                        var fair = model.PdrGrades.Where(x => x.Id == PhoenixERPWebApp.Models.Grades.Fair).FirstOrDefault();
                        var good = model.PdrGrades.Where(x => x.Id == PhoenixERPWebApp.Models.Grades.Good).FirstOrDefault();
                        var veryGood = model.PdrGrades.Where(x => x.Id == PhoenixERPWebApp.Models.Grades.VeryGood).FirstOrDefault();
                        var excellent = model.PdrGrades.Where(x => x.Id == PhoenixERPWebApp.Models.Grades.Excellent).FirstOrDefault();

                        for (int i = 0; i < modelPDRCategory.Count(); i++)
                        {
                            decimal? averageScore = 0;
                            decimal? total = 0;
                            decimal? percentage = 0;
                            if (model.PdrOverallScores != null)
                            {
                                var overallScore = model.PdrOverallScores.Where(x => x.PdrCategoryId == modelPDRCategory[i].Id).FirstOrDefault();
                                if (overallScore != null)
                                {
                                    averageScore = overallScore.AverageScore;
                                    percentage = overallScore.Percentage;
                                    total = overallScore.Total;
                                    finalScore = model.FinalScore;
                                }
                                else
                                {
                                    if (modelPDRCategory[i].Percentage == null)
                                        percentage = 0;
                                    else
                                        percentage = modelPDRCategory[i].Percentage;
                                }
                            }
                            else
                            {
                                percentage = item.Percentage;
                            }

                            model.modelPDRCategory = modelPDRCategory;



                            model.modelPDRCategory[i].averageScore = averageScore;
                            model.modelPDRCategory[i].percentage = percentage;
                            model.modelPDRCategory[i].total = total;
                            model.modelPDRCategory[i].finalScore = finalScore;

                            model.poor = poor.Result;
                            model.fair = fair.Result;
                            model.good = good.Result;
                            model.veryGood = veryGood.Result;
                            model.excellent = excellent.Result;
                        }



                    }
                }
            }


            return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //[HttpGet]
        //public async Task<IActionResult> GetFixedAssetById(string id)
        //{
        //    var model = new FixedAssetDepreciation();
        //    model = await client.Get<FixedAssetDepreciation>($"{url}/fixedassetbyid/{id}");
        //    return Json(model, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        //}




    }
}