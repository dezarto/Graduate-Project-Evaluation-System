namespace GPESAPI.Application.DTOs
{
    public class TeamPresentationDTO
    {
        public int TeamPresentationId { get; set; }
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public int Professor1Id { get; set; }
        public int Professor2Id { get; set; }
        public DateTime PresentationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool isEvaluated { get; set; }
    }
}
