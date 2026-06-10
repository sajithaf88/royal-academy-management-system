using System;
using System.ComponentModel.DataAnnotations;

namespace RoyalAcademy.Models
{
    public class Fee
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public Student? Student { get; set; }
    }
}
