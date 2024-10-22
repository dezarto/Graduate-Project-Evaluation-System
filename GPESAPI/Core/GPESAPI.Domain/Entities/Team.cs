namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public List<int> TeamMemberIds { get; set; }
        public List<int> SubmissionIds { get; set; }
        public int EvaluationId { get; set; }
    }
}
