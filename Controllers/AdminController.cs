using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ICourseService _courseService;

        public AdminController(IAdminService adminService, ICourseService courseService)
        {
            _adminService = adminService;
            _courseService = courseService;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /Admin  and  /Admin/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var dashboard = await _adminService.GetDashboardAsync();
            return View(dashboard);
        }

        // GET: /Admin/Users
        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            var users = await _adminService.GetAllUsersAsync();
            return View(users);
        }

        // GET: /Admin/Users/Details/{id}
        [HttpGet("Users/Details/{id:int}")]
        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _adminService.GetUserDetailsAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: /Admin/Users/Edit/{id}
        [HttpGet("Users/Edit/{id:int}")]
        public async Task<IActionResult> UserEdit(int id)
        {
            var model = await _adminService.GetUserForEditAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /Admin/Users/Edit/{id}
        [HttpPost("Users/Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(int id, AdminUserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _adminService.UpdateUserAsync(id, model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Users));
        }

        // GET: /Admin/Users/Delete/{id}
        [HttpGet("Users/Delete/{id:int}")]
        public async Task<IActionResult> UserDelete(int id)
        {
            var user = await _adminService.GetUserDetailsAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /Admin/Users/Delete/{id}
        [HttpPost("Users/Delete/{id:int}")]
        [ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDeleteConfirmed(int id)
        {
            var result = await _adminService.DeleteUserAsync(id, CurrentUserId);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Users));
        }

        // GET: /Admin/Courses
        [HttpGet("Courses")]
        public async Task<IActionResult> Courses()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }

        // GET: /Admin/Courses/Details/{id}
        [HttpGet("Courses/Details/{id:int}")]
        public async Task<IActionResult> CourseDetails(int id)
        {
            var course = await _courseService.GetDetailsAsync(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // POST: /Admin/Courses/Delete/{id}
        [HttpPost("Courses/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseDelete(int id)
        {
            var result = await _courseService.DeleteAsync(id, CurrentUserId, isAdmin: true);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Courses));
        }

        // GET: /Admin/Statistics
        [HttpGet("Statistics")]
        public async Task<IActionResult> Statistics()
        {
            var stats = await _adminService.GetStatisticsAsync();
            return View(stats);
        }
    }
}
