namespace GPESAPI.Domain.Entities
{
    public class ProjectSubmission
    {
        public int SubmissionId { get; set; }
        public int TeamId { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
