namespace GPESAPI.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string StudentNumber { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int ProfessorId { get; set; }
    }
}
