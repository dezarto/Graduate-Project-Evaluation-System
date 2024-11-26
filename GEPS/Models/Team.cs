namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = String.Empty;
        public string TeamStatus { get; set; } = String.Empty;
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public List<int> TeamMemberIds { get; set; }
        public List<int> SubmissionIds { get; set; }
        public int EvaluationId { get; set; }
    }
}
