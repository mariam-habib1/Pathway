using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathway.Data;
using Pathway.Services;
using Pathway.ViewModels;
using System.Security.Claims;

namespace Pathway.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly AppDbContext _context; // for category dropdown; swap for ICategoryRepository once Fadwa's ready

        public CoursesController(ICourseService courseService, AppDbContext context)
        {
            _courseService = courseService;
            _context = context;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        private bool IsAdmin => User.IsInRole("Admin");

        // GET: /Courses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        // GET: /Courses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseDetailsAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // GET: /Courses/Create
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create()
        {
            var model = new CourseCreateViewModel
            {
                Categories = await GetCategoryListAsync()
            };
            return View(model);
        }

        // POST: /Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoryListAsync();
                return View(model);
            }

            var result = await _courseService.CreateCourseAsync(model, CurrentUserId);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                model.Categories = await GetCategoryListAsync();
                return View(model);
            }

            return RedirectToAction(nameof(MyCourses));
        }

        // GET: /Courses/Edit/5
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseForEditAsync(id, CurrentUserId, IsAdmin);
            if (course == null) return Forbid();

            var model = new CourseEditViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                CategoryId = course.CategoryId,
                Categories = await GetCategoryListAsync()
            };
            return View(model);
        }

        // POST: /Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Edit(int id, CourseEditViewModel model)
        {
            if (id != model.CourseId) return BadRequest();

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategoryListAsync();
                return View(model);
            }

            var result = await _courseService.UpdateCourseAsync(model, CurrentUserId, IsAdmin);
            if (!result.Success)
            {
                if (result.ErrorMessage!.Contains("not authorized"))
                    return Forbid();

                ModelState.AddModelError("", result.ErrorMessage!);
                model.Categories = await GetCategoryListAsync();
                return View(model);
            }

            return RedirectToAction(nameof(MyCourses));
        }

        // GET: /Courses/Delete/5
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetCourseForEditAsync(id, CurrentUserId, IsAdmin);
            if (course == null) return Forbid();
            return View(course);
        }

        // POST: /Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id, CurrentUserId, IsAdmin);
            if (!result.Success)
            {
                if (result.ErrorMessage!.Contains("not authorized"))
                    return Forbid();
                return NotFound();
            }

            return RedirectToAction(nameof(MyCourses));
        }

        // GET: /Courses/MyCourses
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> MyCourses()
        {
            var courses = await _courseService.GetInstructorCoursesAsync(CurrentUserId);
            return View(courses);
        }

        // GET: /Courses/Instructor/5
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Instructor(int id)
        {
            var courses = await _courseService.GetInstructorCoursesAsync(id);
            return View(courses);
        }

        // GET: /Courses/Search?keyword=...
        [AllowAnonymous]
        public async Task<IActionResult> Search(string keyword)
        {
            var courses = await _courseService.SearchCoursesAsync(keyword ?? "");
            return View(courses);
        }

        // GET: /Courses/ByCategory/5
        [AllowAnonymous]
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var courses = await _courseService.GetCoursesByCategoryAsync(categoryId);
            return View(courses);
        }

        private async Task<List<SelectListItemVM>> GetCategoryListAsync()
        {
            return await Task.FromResult(_context.Categories
                .Select(c => new SelectListItemVM { Value = c.CategoryId, Text = c.Name })
                .ToList());
        }
    }
}
