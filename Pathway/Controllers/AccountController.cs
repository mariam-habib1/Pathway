using Microsoft.AspNetCore.Mvc;

namespace Pathway.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string? ReturnUrl = null)
        {
            // مؤقتًا: أي Username و Password يدخلوا هيعملوا Login
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (!string.IsNullOrEmpty(ReturnUrl))
                    return Redirect(ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Please enter username and password.";
            return View();
        }
    }
}