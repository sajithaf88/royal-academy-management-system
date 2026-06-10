using System.ComponentModel.DataAnnotations;

namespace RoyalAcademy.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string CourseName { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        public decimal Fee { get; set; }
    }
}
