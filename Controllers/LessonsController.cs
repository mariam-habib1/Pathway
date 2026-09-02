using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Controllers
{
    [Authorize]
    public class LessonsController : Controller
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsAdmin => User.IsInRole("Admin");

        // GET: /Lessons/Section/{sectionId}
        public async Task<IActionResult> Section(int sectionId)
        {
            var page = await _lessonService.GetLessonsForSectionAsync(sectionId, CurrentUserId, IsAdmin);
            if (page == null)
                return NotFound();

            return View(page);
        }

        // GET: /Lessons/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _lessonService.GetLessonDetailsAsync(id, CurrentUserId, IsAdmin);
            if (lesson == null)
                return NotFound();

            return View(lesson);
        }

        // GET: /Lessons/Create/{sectionId}
        [Authorize(Roles = "Instructor")]
        public IActionResult Create(int sectionId)
        {
            return View(new LessonFormViewModel { SectionId = sectionId });
        }

        // POST: /Lessons/Create/{sectionId}
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int sectionId, LessonFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _lessonService.CreateAsync(sectionId, CurrentUserId, IsAdmin, model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Section), new { sectionId });
        }

        // GET: /Lessons/Edit/{id}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _lessonService.GetForEditAsync(id, CurrentUserId, IsAdmin);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /Lessons/Edit/{id}
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _lessonService.UpdateAsync(id, CurrentUserId, IsAdmin, model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Section), new { sectionId = result.SectionId });
        }

        // GET: /Lessons/Delete/{id}
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _lessonService.GetForEditAsync(id, CurrentUserId, IsAdmin);
            if (model == null)
                return NotFound();

            return View(model);
        }

        // POST: /Lessons/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Instructor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _lessonService.DeleteAsync(id, CurrentUserId, IsAdmin);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            if (result.SectionId == null)
                return RedirectToAction("Index", "Courses");

            return RedirectToAction(nameof(Section), new { sectionId = result.SectionId });
        }
    }
}
