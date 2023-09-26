using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMarvel.Infrastructure.WebNetCore;
using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Areas.Finance.Controllers
{
    [Area("Finance")]
    public class MasterFixedAssetController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            return View();
        }
    }
}