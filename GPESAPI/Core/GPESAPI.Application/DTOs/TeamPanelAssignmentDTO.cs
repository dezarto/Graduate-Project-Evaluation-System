namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class TeamPanelAssignmentDTO
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public int TeamId { get; set; }
        public int PanelId { get; set; }

        public TeamDTO Team { get; set; }
        public PanelDTO Panel { get; set; }
    }
}
