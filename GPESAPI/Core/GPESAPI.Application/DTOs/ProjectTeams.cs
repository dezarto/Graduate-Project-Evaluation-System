namespace GPESAPI.Application.DTOs
{
    public class ProjectTeams
    {
        public int TeamId { get; set; }
        public int TeamPresentationId { get; set; }
        public List<StudentList> Members { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public bool isEvaluated { get; set; }
        public string EvaluatingTeacherFullName { get; set; }
        public string EvaluatingTeacherMail { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public bool isApproval { get; set; }
        public bool IsActive { get; set; }
        public DateTime PresentationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class StudentList
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
    }
}
