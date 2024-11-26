namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Evaluation
    {
        public int EvaluationId { get; set; }
        public int TeamId { get; set; }
        public int ProfessorId { get; set; }
        public int EvaluationScore { get; set; }
        public string Comments { get; set; } = String.Empty;
        public DateTime Date { get; set; }
    }
}
