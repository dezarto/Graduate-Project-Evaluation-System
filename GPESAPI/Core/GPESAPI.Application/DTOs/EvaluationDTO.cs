namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class EvaluationDTO
    {
        public int Id { get; set; }
        public int EvaluationId { get; set; }
        public int TeamId { get; set; }
        public int ProfessorId { get; set; }
        public int EvaluationScore { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }

        public TeamDTO Team { get; set; }
        public ProfessorDTO Professor { get; set; }
    }
}
