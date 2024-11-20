namespace GPESAPI.Application.DTOs
{
    public class StudentProjectTeamsWeb
    {
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public bool isActive { get; set; }
        public List<MemberList> TeamMembers { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
    }

    public class MemberList
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
    }
}
