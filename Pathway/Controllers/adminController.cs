using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pathway.Data;

namespace Pathway.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // ADMIN DASHBOARD
        // =========================

        [HttpGet("/Admin")]
        [HttpGet("/Admin/Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.UsersCount = await _context.Users.CountAsync();
            ViewBag.CoursesCount = await _context.Courses.CountAsync();
            ViewBag.CategoriesCount = await _context.Categories.CountAsync();
            ViewBag.EnrollmentsCount = await _context.Enrollments.CountAsync();

            return View();
        }


        // =========================
        // MANAGE USERS
        // =========================

        [HttpGet("/Admin/Users")]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }


        // =========================
        // USER DETAILS
        // =========================

        [HttpGet("/Admin/Users/Details/{id}")]
        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // =========================
        // EDIT USER - GET
        // =========================

        [HttpGet("/Admin/Users/Edit/{id}")]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // =========================
        // EDIT USER - POST
        // =========================

        [HttpPost("/Admin/Users/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(
            int id,
            string name,
            string email,
            string role)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = name;
            user.Email = email;
            user.Role = role;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }


        // =========================
        // DELETE USER - GET
        // =========================

        [HttpGet("/Admin/Users/Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // =========================
        // DELETE USER - POST
        // =========================

        [HttpPost("/Admin/Users/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }


        // =========================
        // MANAGE COURSES
        // =========================

        [HttpGet("/Admin/Courses")]
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(courses);
        }


        // =========================
        // COURSE DETAILS
        // =========================

        [HttpGet("/Admin/Courses/Details/{id}")]
        public async Task<IActionResult> CourseDetails(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        // =========================
        // DELETE COURSE - GET
        // =========================

        [HttpGet("/Admin/Courses/Delete/{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        // =========================
        // DELETE COURSE - POST
        // =========================

        [HttpPost("/Admin/Courses/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourseConfirmed(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Courses));
        }


        // =========================
        // STATISTICS
        // =========================

        [HttpGet("/Admin/Statistics")]
        public async Task<IActionResult> Statistics()
        {
            ViewBag.UsersCount = await _context.Users.CountAsync();
            ViewBag.CoursesCount = await _context.Courses.CountAsync();
            ViewBag.CategoriesCount = await _context.Categories.CountAsync();
            ViewBag.EnrollmentsCount = await _context.Enrollments.CountAsync();

            return View();
        }
    }
}