using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LogR.Service;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Search;

namespace LogR.Web.Controllers
{
    public class HomeController : Controller
    {
        ILogRetrivalService logRetrivalService;
        public HomeController(ILogRetrivalService logRetrivalService)
        {
            this.logRetrivalService = logRetrivalService;
        }
        
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        [Route("/")]
        public IActionResult Dashboard()
        {
            var summary = logRetrivalService.GetDashboardSummary();
            summary.ActiveTab = 1;
            return View(summary);
        }

        [Route("/app-logs")]
        public IActionResult AppLogs(AppLogSearchCriteria search)
        {
            var appData = logRetrivalService.GetAppLogs(search);
            appData.ActiveTab = 2;
            return View(appData);

        }

        [Route("/performance-logs")]
        public IActionResult PerformanceLogs(PerformanceLogSearchCriteria search)
        {
            var perfData = logRetrivalService.GetPerformanceLogs(search);
            perfData.ActiveTab = 3;
            return View(perfData);

        }

    }
}
