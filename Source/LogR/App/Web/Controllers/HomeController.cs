using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Search;
using Microsoft.AspNetCore.Mvc;

namespace LogR.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard","LogExplorer");
        }
    }
}
