

namespace GEPS.Models
{
    public class ProjectDetails
    {
        public int Id { get; set; }

        public string projectTitle { get; set; } = String.Empty;

        public string projectDescription { get; set; } = String.Empty;

        public string projectTeamName { get; set; } = String.Empty;

        public string projectAdvisory { get; set; } = String.Empty;

        public string TeamMember1FullName { get; set; } = String.Empty;

        public string TeamMember1StudentNumber { get; set; } = String.Empty;

        public string TeamMember2FullName { get; set; } = String.Empty;

        public string TeamMember2StudentNumber { get; set; } = String.Empty;

        public string TeamMember3FullName { get; set; } = String.Empty;

        public string TeamMember3StudentNumber { get; set; } = String.Empty;
    }
}