using Microsoft.AspNetCore.Mvc;

namespace Pathway.Controllers
{
    public class AdminController : Controller
    {
     
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UserDetails(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditUser(int id, object model)
        {
            return RedirectToAction("Users");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            return RedirectToAction("Users");
        }

        [HttpGet]
        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Statistics()
        {
            return View();
        }
    }
}