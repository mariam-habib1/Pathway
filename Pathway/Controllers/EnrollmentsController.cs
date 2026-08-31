using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathway.Services.Interfaces;
using Pathway.ViewModels;

namespace Pathway.Controllers
{
    [Authorize(Roles = "Student")]
    public class EnrollmentsController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // Reads the logged-in student's id from the auth cookie claims
        private int CurrentStudentId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /Enrollments/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            var dashboard = await _enrollmentService.GetStudentDashboardAsync(CurrentStudentId);
            return View(dashboard);
        }

        // POST: /Enrollments/Enroll/{courseId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var result = await _enrollmentService.EnrollAsync(CurrentStudentId, courseId);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

            return RedirectToAction("Details", "Courses", new { id = courseId });
        }

        // GET: /Enrollments/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var enrollment = await _enrollmentService.GetDetailsAsync(id, CurrentStudentId);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // POST: /Enrollments/UpdateProgress/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProgress(int id, UpdateProgressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid progress value.";
                return RedirectToAction("Details", new { id });
            }

            var result = await _enrollmentService.UpdateProgressAsync(id, CurrentStudentId, model.Progress);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction("Details", new { id });
        }

        // GET: /Enrollments/Cancel/{id}
        public async Task<IActionResult> Cancel(int id)
        {
            var enrollment = await _enrollmentService.GetDetailsAsync(id, CurrentStudentId);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        // POST: /Enrollments/Cancel/{id}
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var result = await _enrollmentService.CancelAsync(id, CurrentStudentId);

            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction("MyCourses");
        }
    }
}