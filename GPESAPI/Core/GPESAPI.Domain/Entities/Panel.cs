namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Panel
    {
        public int Id { get; set; }
        public int PanelId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public ICollection<TeamPanelAssignment> TeamPanelAssignments { get; set; }
    }
}
