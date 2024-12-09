namespace GPESAPI.Application.DTOs
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public bool isActive { get; set; }
    }
}
