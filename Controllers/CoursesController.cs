using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsAdmin => User.IsInRole("Admin");

        // GET: /Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync();
            return View(courses);
        }

        // GET: /Courses/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            int? currentUserId = (User.Identity != null && User.Identity.IsAuthenticated)
                ? CurrentUserId
                : null;

            var course = await _courseService.GetDetailsAsync(id, currentUserId);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // GET: /Courses/Create
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create()
        {
            var model = await _courseService.GetEmptyFormAsync();
            return View(model);
        }

        // POST: /Courses/Create
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryOptions = (await _courseService.GetEmptyFormAsync()).CategoryOptions;
                return View(model);
            }

            var result = await _courseService.CreateAsync(CurrentUserId, model);

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("BuildContent", "CourseSections", new { courseId = result.CourseId });
        }

        // GET: /Courses/Edit/{id}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _courseService.GetForEditAsync(id, CurrentUserId, IsAdmin);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /Courses/Edit/{id}
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var refreshed = await _courseService.GetForEditAsync(id, CurrentUserId, IsAdmin);
                model.CategoryOptions = refreshed?.CategoryOptions ?? new();
                return View(model);
            }

            var result = await _courseService.UpdateAsync(id, CurrentUserId, IsAdmin, model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                model.CategoryOptions = (await _courseService.GetForEditAsync(id, CurrentUserId, IsAdmin))?.CategoryOptions ?? new();
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: /Courses/Delete/{id}
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetDetailsAsync(id);
            if (course == null)
                return NotFound();

            if (!IsAdmin && course.InstructorId != CurrentUserId)
                return Forbid();

            return View(course);
        }

        // POST: /Courses/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Instructor,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _courseService.DeleteAsync(id, CurrentUserId, IsAdmin);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Courses/MyCourses
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> MyCourses()
        {
            var courses = await _courseService.GetByInstructorAsync(CurrentUserId);
            return View(courses);
        }

        // GET: /Courses/Instructor/{id}
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Instructor(int id)
        {
            var courses = await _courseService.GetByInstructorAsync(id);
            return View(courses);
        }

        // GET: /Courses/Search?keyword=...
        public async Task<IActionResult> Search(string? keyword)
        {
            var courses = await _courseService.SearchAsync(keyword ?? string.Empty);
            ViewData["Keyword"] = keyword;
            ViewData["Title"] = string.IsNullOrWhiteSpace(keyword) ? "Courses" : $"Search results for \"{keyword}\"";
            return View("Index", courses);
        }

        // GET: /Courses/ByCategory/{categoryId}
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var courses = await _courseService.GetByCategoryAsync(categoryId);
            ViewData["Title"] = "Courses";
            return View("Index", courses);
        }
    }
}