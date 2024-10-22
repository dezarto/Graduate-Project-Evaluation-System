namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class TeamMember
    {
        public int TeamMemberId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }

        public Team Team { get; set; }
        public User User { get; set; }
    }
}
