namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int TeamId { get; set; }
        public int ProfessorId { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }
    }
}
