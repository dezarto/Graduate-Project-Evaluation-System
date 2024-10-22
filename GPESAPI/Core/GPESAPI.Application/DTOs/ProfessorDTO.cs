namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class ProfessorDTO
    {
        public int ProfessorId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }

        
        public List<int> UserIds { get; set; }
        public List<int> AvailabilityIds { get; set; }
        public List<int> EvaluationIds { get; set; }
        public List<int> FeedbackIds { get; set; }
    }

}
