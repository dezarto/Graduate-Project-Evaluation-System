namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class TeamPanelAssignment
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int TeamId { get; set; }
        public int PanelId { get; set; }

        public Team Team { get; set; }
        public Panel Panel { get; set; }
    }
}
