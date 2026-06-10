using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RoyalAcademy.Models
{
    public class Student
    {
        public int Id { get; set; }

        [BindNever] // 🔥 CRITICAL FIX
        public string StudentCode { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int CourseId { get; set; }

        public Course? Course { get; set; }

        public string? UserId { get; set; }
    }
}