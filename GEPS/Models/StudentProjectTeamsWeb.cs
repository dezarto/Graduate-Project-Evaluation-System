using GEPS.Controllers;

namespace GEPS.Models
{
    public class StudentProjectTeamsWeb
    {
        public int? TeamId { get; set; }
        public int? ProjectId { get; set; }
        public int? AdvisorId { get; set; }
        public bool? isActive { get; set; }
        public List<MemberList> Members { get; set; }
        public string? TeamName { get; set; }
        public string? Description { get; set; }
        public string? ProjectName { get; set; }
        public int? ReportId { get; set; }
        public DateTime? PresentationDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }

    public class MemberList
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
    }
}
