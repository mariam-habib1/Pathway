using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Controllers
{
    [Authorize(Roles = "Instructor,Admin")]
    public class CourseSectionsController : Controller
    {
        private readonly ICourseSectionService _sectionService;

        public CourseSectionsController(ICourseSectionService sectionService)
        {
            _sectionService = sectionService;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsAdmin => User.IsInRole("Admin");

        // GET: /CourseSections/Course/{courseId}
        public async Task<IActionResult> Course(int courseId)
        {
            var page = await _sectionService.GetSectionsForCourseAsync(courseId, CurrentUserId, IsAdmin);
            if (page == null)
                return NotFound();

            return View(page);
        }

        // GET: /CourseSections/Create/{courseId}
        [Authorize(Roles = "Instructor")]
        public IActionResult Create(int courseId)
        {
            return View(new CourseSectionFormViewModel { CourseId = courseId });
        }

        // POST: /CourseSections/Create/{courseId}
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int courseId, CourseSectionFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _sectionService.CreateAsync(courseId, CurrentUserId, IsAdmin, model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Course), new { courseId });
        }

        // GET: /CourseSections/Edit/{id}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _sectionService.GetForEditAsync(id, CurrentUserId, IsAdmin);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /CourseSections/Edit/{id}
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseSectionFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _sectionService.UpdateAsync(id, CurrentUserId, IsAdmin, model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Course), new { courseId = result.CourseId });
        }

        // GET: /CourseSections/Delete/{id}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _sectionService.GetForEditAsync(id, CurrentUserId, IsAdmin);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /CourseSections/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _sectionService.DeleteAsync(id, CurrentUserId, IsAdmin);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            if (result.CourseId == null)
                return RedirectToAction("Index", "Courses");

            return RedirectToAction(nameof(Course), new { courseId = result.CourseId });
        }

        // GET: /CourseSections/BuildContent/{courseId}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> BuildContent(int courseId)
        {
            var model = await _sectionService.GetContentBuilderAsync(courseId, CurrentUserId, IsAdmin);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /CourseSections/BuildContent
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuildContent(CourseContentBuilderViewModel model)
        {
            var result = await _sectionService.SaveContentAsync(model.CourseId, CurrentUserId, IsAdmin, model);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction(nameof(Course), new { courseId = model.CourseId });
        }
    }
}
