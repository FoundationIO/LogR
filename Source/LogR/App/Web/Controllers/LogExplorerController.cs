using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Search;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LogR.Web.Controllers
{
    public class LogExplorerController : Controller
    {
        private ILogRetrivalService logRetrivalService;

        public LogExplorerController(ILogRetrivalService logRetrivalService)
        {
            this.logRetrivalService = logRetrivalService;
        }

        [Route("/dashboard")]
        public IActionResult Dashboard()
        {
            //Look at https://blog.platformular.com/2012/04/12/create-a-dashboard-experience-in-asp-net-mvc/
            //for creating dashboard
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

        [Route("/stats")]
        public IActionResult Stats()
        {
            var data = logRetrivalService.GetStats();
            data.ActiveTab = 4;
            return View(data);
        }

        [Route("/list/apps")]
        public ReturnListModel<string> AppNames(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }

        [Route("/list/severities")]
        public ReturnListModel<string> SeverityNames(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }

        [Route("/list/machines")]
        public ReturnListModel<string> MachineNames(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }

        [Route("/list/users")]
        public ReturnListModel<string> UserNames(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }

        [Route("/list/ips")]
        public ReturnListModel<string> Ips(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }

        [Route("/list/perf-modules")]
        public ReturnListModel<string> PerfModuleNames(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }

        [Route("/list/perf-statuses")]
        public ReturnListModel<string> PerfStatusNames(BaseSearchCriteria search)
        {
            return new ReturnListModel<string>(new List<string>());
        }
    }
}
