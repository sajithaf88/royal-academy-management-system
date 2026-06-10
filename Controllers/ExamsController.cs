using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Data;
using RoyalAcademy.Models;

namespace RoyalAcademy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================
        // GET: Exams
        // ============================
        public async Task<IActionResult> Index()
        {
            var exams = _context.Exams
                .Include(e => e.Course);

            return View(await exams.ToListAsync());
        }

        // ============================
        // GET: Exams/Create
        // ============================
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "CourseName"
            );

            return View();
        }

        // ============================
        // POST: Exams/Create
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 🔴 Reload dropdown if validation fails
            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "CourseName",
                exam.CourseId
            );

            return View(exam);
        }

        // ============================
        // GET: Exams/Edit/5
        // ============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return NotFound();

            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "CourseName",
                exam.CourseId
            );

            return View(exam);
        }

        // ============================
        // POST: Exams/Edit
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exam exam)
        {
            if (id != exam.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "CourseName",
                exam.CourseId
            );

            return View(exam);
        }

        // ============================
        // GET: Exams/Delete/5
        // ============================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null) return NotFound();

            return View(exam);
        }

        // ============================
        // POST: Exams/Delete
        // ============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
