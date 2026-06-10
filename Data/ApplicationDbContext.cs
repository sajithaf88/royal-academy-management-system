using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoyalAcademy.Models;

namespace RoyalAcademy.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<Result> Results => Set<Result>();
        public DbSet<Fee> Fees => Set<Fee>();
        public DbSet<ClassSchedule> ClassSchedules => Set<ClassSchedule>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ======================
            // Decimal precision
            // ======================
            builder.Entity<Course>()
                .Property(c => c.Fee)
                .HasPrecision(18, 2);

            builder.Entity<Fee>()
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            // ======================
            // Student → Course (NO CASCADE)
            // ======================
            builder.Entity<Student>()
                .HasOne(s => s.Course)
                .WithMany()
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // ======================
            // ClassSchedule → Course (CASCADE OK)
            // ======================
            builder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Course)
                .WithMany()
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
