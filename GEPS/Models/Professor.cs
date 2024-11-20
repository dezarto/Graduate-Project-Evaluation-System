namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Professor
    {
        public int ProfessorId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }


        public List<int> UserIds { get; set; } = new List<int>();
        public List<int> AvailabilityIds { get; set; } = new List<int>();
        public List<int> EvaluationIds { get; set; } = new List<int>();
        public List<int> FeedbackIds { get; set; } = new List<int>();
    }
}
