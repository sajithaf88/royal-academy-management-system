using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Data;

namespace RoyalAcademy.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Student Report
        public async Task<IActionResult> StudentsReport()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // Attendance Report
        public async Task<IActionResult> AttendanceReport()
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .ToListAsync();

            return View(attendance);
        }

        // Result Report
        public async Task<IActionResult> ResultsReport()
        {
            var results = await _context.Results
                .Include(r => r.Student)
                .Include(r => r.Exam)
                .ToListAsync();

            return View(results);
        }

        // Fees Report
        public async Task<IActionResult> FeesReport()
        {
            var fees = await _context.Fees
                .Include(f => f.Student)
                .ToListAsync();

            return View(fees);
        }
    }
}
