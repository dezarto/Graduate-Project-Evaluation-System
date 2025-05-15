namespace GPESAPI.Domain.Entities
{
    public class Evaluation
    {
        public int EvaluationId { get; set; }
        public int TeamId { get; set; }
        public int ProfessorId { get; set; }
        public double EvaluationScore { get; set; }
        public string GeneralComments { get; set; }
        public DateTime Date { get; set; }
    }
}
