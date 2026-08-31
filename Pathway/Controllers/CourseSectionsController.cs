using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pathway.Data;
using Pathway.Models;

namespace Pathway.Controllers
{
    public class CourseSectionsController : Controller
    {
        private readonly AppDbContext _context;

        public CourseSectionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CourseSections
        public async Task<IActionResult> Index()
        {
            var sections = await _context.CourseSections
                .Include(s => s.Course)
                .OrderBy(s => s.CourseId)
                .ThenBy(s => s.Order)
                .ToListAsync();

            return View(sections);
        }

        // GET: CourseSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.CourseSections
                .Include(s => s.Course)
                .Include(s => s.Lessons)
                .FirstOrDefaultAsync(s => s.SectionId == id);

            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // GET: CourseSections/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Title)
                .ToListAsync();

            return View();
        }

        // POST: CourseSections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseSection section)
        {
            if (ModelState.IsValid)
            {
                _context.CourseSections.Add(section);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Title)
                .ToListAsync();

            return View(section);
        }

        // GET: CourseSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.CourseSections.FindAsync(id);

            if (section == null)
            {
                return NotFound();
            }

            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Title)
                .ToListAsync();

            return View(section);
        }

        // POST: CourseSections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseSection section)
        {
            if (id != section.SectionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.CourseSections.Update(section);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectionExists(section.SectionId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Title)
                .ToListAsync();

            return View(section);
        }

        // GET: CourseSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var section = await _context.CourseSections
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.SectionId == id);

            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }

        // POST: CourseSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var section = await _context.CourseSections.FindAsync(id);

            if (section != null)
            {
                _context.CourseSections.Remove(section);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SectionExists(int id)
        {
            return _context.CourseSections.Any(s => s.SectionId == id);
        }
    }

}