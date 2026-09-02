using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pathway.Models;
using Pathway.Services.Interfaces;

namespace Pathway.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;

        public HomeController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            var featuredCourses = courses.OrderByDescending(c => c.CreatedAt).Take(6).ToList();
            return View(featuredCourses);
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
