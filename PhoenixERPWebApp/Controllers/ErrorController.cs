using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PhoenixERPWebApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public IActionResult Index()
        {
            return View();
        }

        // GET: Error
        public IActionResult Http()
        {
            return View();
        }
    }
}