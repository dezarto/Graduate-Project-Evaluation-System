namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class ProjectSubmissionDTO
    {
        public int SubmissionId { get; set; }
        public int TeamId { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmissionDate { get; set; }

        public TeamDTO Team { get; set; }
    }
}
