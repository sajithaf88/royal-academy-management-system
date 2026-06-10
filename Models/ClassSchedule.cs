namespace RoyalAcademy.Models
{
    public class ClassSchedule
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime ClassDate { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Subject { get; set; }
    }
}
