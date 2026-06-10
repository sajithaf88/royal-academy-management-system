using System.ComponentModel.DataAnnotations;

namespace RoyalAcademy.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        public string ExamName { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public int MaxMarks { get; set; }
    }
}
