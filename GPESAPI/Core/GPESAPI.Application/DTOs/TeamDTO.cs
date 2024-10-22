namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }

        public ProjectDTO Project { get; set; }
        public ProfessorDTO Advisor { get; set; }
        public ICollection<TeamMemberDTO> TeamMembers { get; set; }
        public ICollection<ProjectSubmissionDTO> ProjectSubmissions { get; set; }
        public EvaluationDTO Evaluation { get; set; }
    }
}
