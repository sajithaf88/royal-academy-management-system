using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Data;
using RoyalAcademy.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RoyalAcademy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: Fees
        // =========================
        public async Task<IActionResult> Index()
        {
            var fees = _context.Fees
                .Include(f => f.Student);

            return View(await fees.ToListAsync());
        }

        // =========================
        // GET: Fees/Details/5
        // =========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var fee = await _context.Fees
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fee == null)
                return NotFound();

            return View(fee);
        }

        // =========================
        // GET: Fees/Create
        // =========================
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(
                _context.Students
                .Select(s => new
                {
                    Id = s.Id,
                    Display = s.StudentCode + " - " + s.FullName
                }),
                "Id",
                "Display");

            return View();
        }

        // =========================
        // POST: Fees/Create
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,Amount,PaymentDate")] Fee fee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentId"] = new SelectList(
                _context.Students
                .Select(s => new
                {
                    Id = s.Id,
                    Display = s.StudentCode + " - " + s.FullName
                }),
                "Id",
                "Display",
                fee.StudentId);

            return View(fee);
        }

        // =========================
        // GET: Fees/Edit/5
        // =========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var fee = await _context.Fees.FindAsync(id);
            if (fee == null)
                return NotFound();

            ViewData["StudentId"] = new SelectList(
                _context.Students
                .Select(s => new
                {
                    Id = s.Id,
                    Display = s.StudentCode + " - " + s.FullName
                }),
                "Id",
                "Display",
                fee.StudentId);

            return View(fee);
        }

        // =========================
        // POST: Fees/Edit/5
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Amount,PaymentDate")] Fee fee)
        {
            if (id != fee.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Fees.Any(e => e.Id == fee.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentId"] = new SelectList(
                _context.Students
                .Select(s => new
                {
                    Id = s.Id,
                    Display = s.StudentCode + " - " + s.FullName
                }),
                "Id",
                "Display",
                fee.StudentId);

            return View(fee);
        }

        // =========================
        // GET: Fees/Delete/5
        // =========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var fee = await _context.Fees
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (fee == null)
                return NotFound();

            return View(fee);
        }

        // =========================
        // POST: Fees/Delete/5
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fee = await _context.Fees.FindAsync(id);
            if (fee != null)
                _context.Fees.Remove(fee);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
