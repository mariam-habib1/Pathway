using Microsoft.AspNetCore.Mvc;

namespace Pathway.Controllers
{
    public class EnrollmentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
