using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pathway.Models;
using Pathway.Data;

public class LessonsController : Controller
{
    private readonly AppDbContext _context;

    public LessonsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Lessons
    public async Task<IActionResult> Index()
    {
        var lessons = await _context.Lessons
            .Include(l => l.Section)
            .OrderBy(l => l.SectionId)
            .ThenBy(l => l.Order)
            .ToListAsync();

        return View(lessons);
    }

    // GET: Lessons/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons
            .Include(l => l.Section)
            .FirstOrDefaultAsync(l => l.LessonId == id);

        if (lesson == null)
        {
            return NotFound();
        }

        return View(lesson);
    }

    // GET: Lessons/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Sections = await _context.CourseSections
            .OrderBy(s => s.CourseId)
            .ThenBy(s => s.Order)
            .ToListAsync();

        return View();
    }

    // POST: Lessons/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        if (ModelState.IsValid)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Sections = await _context.CourseSections
            .OrderBy(s => s.CourseId)
            .ThenBy(s => s.Order)
            .ToListAsync();

        return View(lesson);
    }

    // GET: Lessons/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons.FindAsync(id);

        if (lesson == null)
        {
            return NotFound();
        }

        ViewBag.Sections = await _context.CourseSections
            .OrderBy(s => s.CourseId)
            .ThenBy(s => s.Order)
            .ToListAsync();

        return View(lesson);
    }

    // POST: Lessons/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Lesson lesson)
    {
        if (id != lesson.LessonId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Lessons.Update(lesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(lesson.LessonId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Sections = await _context.CourseSections
            .OrderBy(s => s.CourseId)
            .ThenBy(s => s.Order)
            .ToListAsync();

        return View(lesson);
    }

    // GET: Lessons/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons
            .Include(l => l.Section)
            .FirstOrDefaultAsync(l => l.LessonId == id);

        if (lesson == null)
        {
            return NotFound();
        }

        return View(lesson);
    }

    // POST: Lessons/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);

        if (lesson != null)
        {
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool LessonExists(int id)
    {
        return _context.Lessons.Any(l => l.LessonId == id);
    }

}