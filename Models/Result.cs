using System.ComponentModel.DataAnnotations;

namespace RoyalAcademy.Models
{
    public class Result
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ExamId { get; set; }

        public int MarksObtained { get; set; }

        public Student? Student { get; set; }
        public Exam? Exam { get; set; }
    }
}
