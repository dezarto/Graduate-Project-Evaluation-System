namespace GPESAPI.Domain.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public bool isActive { get; set; }
    }
}
