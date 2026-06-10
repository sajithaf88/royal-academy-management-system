using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Data;
using System.Security.Claims;

namespace RoyalAcademy.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===============================
        // HOME → ALWAYS GO TO DASHBOARD
        // ===============================
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        // ===============================
        // DASHBOARD
        // ===============================
        public async Task<IActionResult> Dashboard()
        {
            // ===============================
            // ADMIN DASHBOARD
            // ===============================
            if (User.IsInRole("Admin"))
            {
                ViewBag.TotalStudents = await _context.Students.CountAsync();
                ViewBag.TotalTeachers = await _context.Teachers.CountAsync();
                ViewBag.TotalCourses = await _context.Courses.CountAsync();
                ViewBag.TotalExams = await _context.Exams.CountAsync();
                ViewBag.TotalFees = await _context.Fees.SumAsync(f => (decimal?)f.Amount) ?? 0;

                return View("AdminDashboard");
            }

            // ===============================
            // STUDENT DASHBOARD
            // ===============================
            if (User.IsInRole("Student"))
            {
                // ✅ ALWAYS use claims (never User.Identity.Name)
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = User.FindFirstValue(ClaimTypes.Email);

                // 1️⃣ Try finding student by UserId
                var student = await _context.Students
                    .Include(s => s.Course)
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                // 2️⃣ AUTO-LINK student by email (CRITICAL FIX)
                if (student == null && !string.IsNullOrEmpty(email))
                {
                    student = await _context.Students
                        .Include(s => s.Course)
                        .FirstOrDefaultAsync(s => s.Email == email);

                    if (student != null)
                    {
                        student.UserId = userId;
                        await _context.SaveChangesAsync();
                    }
                }

                // 3️⃣ Still not linked
                if (student == null)
                {
                    return View("NoStudent"); // "Account Not Linked" page
                }

                // ===============================
                // LOAD STUDENT DATA
                // ===============================
                var exams = await _context.Exams
                    .Where(e => e.CourseId == student.CourseId)
                    .ToListAsync();

                var attendance = await _context.Attendances
                    .Where(a => a.StudentId == student.Id)
                    .ToListAsync();

                ViewBag.Student = student;
                ViewBag.Exams = exams;
                ViewBag.Attendance = attendance;

                return View("StudentDashboard");
            }

            // ===============================
            // FALLBACK
            // ===============================
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}