using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GEPS.Models
{
    public class TeacherViewResultModel
    {
        public Team Team { get; set; } 
        public Evaluation Evaluation { get; set; } 
        public List<Feedback> Feedback { get; set; }
    }
}
