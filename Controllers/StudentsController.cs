using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Data;
using RoyalAcademy.Models;

namespace RoyalAcademy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // INDEX
        // ===============================
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.Course)
                .ToListAsync();

            return View(students);
        }

        // ===============================
        // DETAILS
        // ===============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // ===============================
        // CREATE (GET)
        // ===============================
        public IActionResult Create()
        {
            ViewBag.CourseId = new SelectList(
                _context.Courses,
                "Id",
                "CourseName"
            );

            return View();
        }

        // ===============================
        // CREATE (POST) ✅ FIXED
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            // Auto-generate student code
            student.StudentCode = "RA" + DateTime.Now.Ticks.ToString()[^5..];

            if (!ModelState.IsValid)
            {
                ViewBag.CourseId = new SelectList(
                    _context.Courses,
                    "Id",
                    "CourseName",
                    student.CourseId
                );
                return View(student);
            }

            // Prevent duplicate email
            bool emailExists = await _context.Students
                .AnyAsync(s => s.Email == student.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Student email already exists.");

                ViewBag.CourseId = new SelectList(
                    _context.Courses,
                    "Id",
                    "CourseName",
                    student.CourseId
                );

                return View(student);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // EDIT (GET)
        // ===============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            ViewBag.CourseId = new SelectList(
                _context.Courses,
                "Id",
                "CourseName",
                student.CourseId
            );

            return View(student);
        }

        // ===============================
        // EDIT (POST)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.CourseId = new SelectList(
                    _context.Courses,
                    "Id",
                    "CourseName",
                    student.CourseId
                );
                return View(student);
            }

            // Preserve StudentCode
            var existingStudent = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingStudent == null)
                return NotFound();

            student.StudentCode = existingStudent.StudentCode;

            _context.Update(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // DELETE (GET)
        // ===============================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // ===============================
        // DELETE (POST)
        // ===============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}