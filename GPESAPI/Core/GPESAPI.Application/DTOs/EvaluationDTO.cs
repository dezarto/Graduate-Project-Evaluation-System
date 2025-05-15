namespace GPESAPI.Application.DTOs
{
    public class EvaluationDTO
    {
        public int EvaluationId { get; set; }
        public int TeamId { get; set; }
        public int ProfessorId { get; set; }
        public double EvaluationScore { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
