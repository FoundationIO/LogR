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

        [Route("/list/searches")]
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

        [Route("/list/apps")]
        public ReturnListModel<string> AppNames(BaseSearchCriteria search)
        {
            var appLst = new List<string>();
            appLst.Add("App Item 1");
            appLst.Add("App Item 2");
            appLst.Add("App Item 3");
            appLst.Add("App Item 4");
            appLst.Add("App Item 5");
            return new ReturnListModel<string>(appLst);
        }

        [Route("/list/loglevels")]
        public ReturnListModel<string> LogLevels(BaseSearchCriteria search)
        {
            var loglevels= new List<string>()
            {
                "INFO",
                "ERROR", "WARNING",
                "DEBUG",
                "TRACE",
                "CRITICAL",
                "SQ",
                "SQL-BEGIN",
                "SQL-ROLLBACK",
                "SQL-COMMIT"
            };
            return new ReturnListModel<string>(loglevels);
        }

        [Route("/list/machines")]
        public ReturnListModel<string> MachineNames(BaseSearchCriteria search)
        {
            var machineLst = new List<string>();
            machineLst.Add("MachineNames Item 1");
            machineLst.Add("MachineNames Item 2");
            machineLst.Add("MachineNames Item 3");
            machineLst.Add("MachineNames Item 4");
            machineLst.Add("MachineNames Item 5");
            return new ReturnListModel<string>(machineLst);
        
        }

        [Route("/list/users")]
        public ReturnListModel<string> UserNames(BaseSearchCriteria search)
        {
            var userLst = new List<string>();
            userLst.Add("UserNames 1");
            userLst.Add("UserNames  2");
            userLst.Add("UserNames  3");
            userLst.Add("UserNames 4");
            userLst.Add("UserNames 5");
            return new ReturnListModel<string>(userLst);
        }

        [Route("/list/functions")]
        public ReturnListModel<string> Functions(BaseSearchCriteria search)
        {
            var functions = new List<string>();
            functions.Add("Function 1");
            functions.Add("Function 2");
            functions.Add("Function 3");
            functions.Add("Function 4");
            functions.Add("Function 5");
            return new ReturnListModel<string>(functions);
        }

        [Route("/list/files")]
        public ReturnListModel<string> FileNames(BaseSearchCriteria search)
        {
            var fileNames = new List<string>();
            fileNames.Add("File 1");
            fileNames.Add("File 2");
            fileNames.Add("File 3");
            fileNames.Add("File 4");
            fileNames.Add("File 5");
            return new ReturnListModel<string>(fileNames);
        }

        [Route("/list/ips")]
        public ReturnListModel<string> Ips(BaseSearchCriteria search)
        {
            var IpsLst = new List<string>();
            IpsLst.Add("Ips 1");
            IpsLst.Add("Ips  2");
            IpsLst.Add("Ips  3");
            IpsLst.Add("Ips 4");
            IpsLst.Add("Ips 5");
            return new ReturnListModel<string>(IpsLst);
        }

        [Route("/list/perf-modules")]
        public ReturnListModel<string> PerfModuleNames(BaseSearchCriteria search)
        {
            var PerfModuleNamesLst = new List<string>();
            PerfModuleNamesLst.Add("PerfModuleNames 1");
            PerfModuleNamesLst.Add("PerfModuleNames  2");
            PerfModuleNamesLst.Add("PerfModuleNames  3");
            PerfModuleNamesLst.Add("PerfModuleNames 4");
            PerfModuleNamesLst.Add("PerfModuleNames 5");
            return new ReturnListModel<string>(PerfModuleNamesLst);
          
        }

        [Route("/list/perf-statuses")]
        public ReturnListModel<string> PerfStatusNames(BaseSearchCriteria search)
        {
            var PerfStatusNamesLst = new List<string>();
            PerfStatusNamesLst.Add("PerfStatusNames 1");
            PerfStatusNamesLst.Add("PerfStatusNames  2");
            PerfStatusNamesLst.Add("PerfStatusNames  3");
            PerfStatusNamesLst.Add("PerfStatusNames 4");
            PerfStatusNamesLst.Add("PerfStatusNames 5");
            return new ReturnListModel<string>(PerfStatusNamesLst);
        }

        [Route("/list/processIds")]
        public ReturnListModel<string> ProcessIds(BaseSearchCriteria search)
        {
            var ProcessLst = new List<string>();
            ProcessLst.Add("Process Id 1");
            ProcessLst.Add("Process Id  2");
            ProcessLst.Add("Process Id  3");
            ProcessLst.Add("Process Id 4");
            ProcessLst.Add("Process Id 5");
            return new ReturnListModel<string>(ProcessLst);
        }

        [Route("/list/threadIds")]
        public ReturnListModel<string> ThreadIds(BaseSearchCriteria search)
        {
            var ThreadLst = new List<string>();
            ThreadLst.Add("Thread ID 1");
            ThreadLst.Add("Thread ID  2");
            ThreadLst.Add("Thread ID  3");
            ThreadLst.Add("Thread ID 4");
            ThreadLst.Add("Thread ID 5");
            return new ReturnListModel<string>(ThreadLst);
        }
    }
}
