using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Data;
using RoyalAcademy.Models;

namespace RoyalAcademy.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View for both roles
        public async Task<IActionResult> Index()
        {
            var results = _context.Results
                .Include(r => r.Student)
                .Include(r => r.Exam);

            return View(await results.ToListAsync());
        }

        // Only Admin can Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "ExamName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Result result)
        {
            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Results
                .Include(r => r.Student)
                .Include(r => r.Exam)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results.FindAsync(id);
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
