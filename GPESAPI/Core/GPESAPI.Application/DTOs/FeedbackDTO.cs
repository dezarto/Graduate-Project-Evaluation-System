namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class FeedbackDTO
    {
        public int FeedbackId { get; set; }
        public int TeamId { get; set; }
        public int ProfessorId { get; set; }
        public string Comments { get; set; }
        public DateTime Date { get; set; }

        public TeamDTO Team { get; set; }
        public ProfessorDTO Professor { get; set; }
    }
}
