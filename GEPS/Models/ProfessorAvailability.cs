namespace GEPS.Models
{
    public class ProfessorAvailability
    {
        public int AvailabilityId { get; set; }
        public int ProfessorId { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
