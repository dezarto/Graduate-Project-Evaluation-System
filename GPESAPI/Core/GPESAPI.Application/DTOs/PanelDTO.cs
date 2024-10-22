namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class PanelDTO
    {
        public int Id { get; set; }
        public int PanelId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<TeamPanelAssignmentDTO> TeamPanelAssignments { get; set; }
    }
}
