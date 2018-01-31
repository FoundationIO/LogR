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

        [Route("/stats")]
        public IActionResult Stats()
        {
            var data = logRetrivalService.GetStats();
            data.ActiveTab = 4;
            return View(data);
        }

        [Route("/api/list/searches")]
        public ReturnListModel<string> Searches(BaseSearchCriteria search)
        {
            var lst = new List<string>();
            lst.Add("Search Item 1");
            lst.Add("Search Item 2");
            lst.Add("Search Item 3");
            lst.Add("Search Item 4");
            lst.Add("Search Item 5");
            return new ReturnListModel<string>(lst);
        }

        [Route("/api/list/apps")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> AppNames(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetAppNames( Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/loglevels")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> LogLevels(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetSeverityNames(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/machines")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> MachineNames(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetMachineNames(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/users")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> UserNames(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetUserNames(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/functions")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> Functions(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetFunctions(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/files")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> FileNames(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetFiles(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/ips")]
        public ReturnListWithSearchModel<string, BaseSearchCriteria> Ips(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetIps(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/processIds")]
        public ReturnListWithSearchModel<int, BaseSearchCriteria> ProcessIds(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetProcessIds(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }

        [Route("/api/list/threadIds")]
        public ReturnListWithSearchModel<int, BaseSearchCriteria> ThreadIds(BaseSearchCriteria search)
        {
            var appLst = logRetrivalService.GetThreadIds(Common.Enums.StoredLogType.AppLog, search);
            return appLst;
        }
    }
}
