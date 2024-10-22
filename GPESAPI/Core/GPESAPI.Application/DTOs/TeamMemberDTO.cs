namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class TeamMemberDTO
    {
        public int TeamMemberId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }

        public TeamDTO Team { get; set; }
        public UserDTO User { get; set; }
    }
}
