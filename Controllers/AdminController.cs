using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pathway.Common;
using Pathway.Data;
using Pathway.ViewModels.Admin;

namespace Pathway.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserListItemViewModel
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserListItemViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditUserViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            var allowedRoles = new[] { Roles.Admin, Roles.Instructor, Roles.Student };
            if (!allowedRoles.Contains(model.Role))
            {
                ModelState.AddModelError("Role", "الدور المحدد غير صحيح");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var normalizedEmail = model.Email.Trim().ToLower();

            var emailTaken = await _context.Users
                .AnyAsync(u => u.UserId != id && u.Email.ToLower() == normalizedEmail);

            if (emailTaken)
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل لمستخدم آخر");
                return View(model);
            }

            user.Name = model.Name.Trim();
            user.Email = normalizedEmail;
            user.Role = model.Role;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم تعديل بيانات المستخدم بنجاح";
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var hasCourses = await _context.Courses.AnyAsync(c => c.InstructorId == id);
            var hasEnrollments = await _context.Enrollments.AnyAsync(e => e.StudentId == id);

            if (hasCourses || hasEnrollments)
            {
                TempData["ErrorMessage"] = "لا يمكن حذف هذا المستخدم لأنه مرتبط بكورسات أو تسجيلات. يمكنك تعديل بياناته بدلاً من حذفه.";
                return RedirectToAction("Users");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "تم حذف المستخدم بنجاح";
            return RedirectToAction("Users");
        }
    }
}