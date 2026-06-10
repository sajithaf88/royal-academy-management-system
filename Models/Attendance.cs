using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoyalAcademy.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }   // FK (internal)

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }
    }
}
