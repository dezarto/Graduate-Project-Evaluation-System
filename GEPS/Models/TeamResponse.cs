namespace GEPS.Models
{
    public class TeamResponse
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int projectId { get; set; }
        public int AdvisorId { get; set; }
        public bool IsActive { get; set; }
    }
}
