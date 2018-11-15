using GDPR.MaxMind;
using Microsoft.AspNetCore.Mvc;

namespace GDPR.Controllers
{
    [ServiceFilter(typeof(GDPRFilter))]
    public class UnavailableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}