using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ebsco.Shared.Caching.ExampleUsage.ServiceAbstractions;
using Ebsco.Shared.Caching.Interfaces;
using Moq;
using StackExchange.Redis;

namespace Ebsco.Shared.Caching.ExampleUsage.Controllers
{
    public class HomeController : Controller
    {
        private IExampleService _service;
        public HomeController(IExampleService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var exampleData = _service.GetExampleData("test value");
            return View(exampleData);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}