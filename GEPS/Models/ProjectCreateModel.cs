using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GEPS.Models
{
    public class ProjectCreateModel
    {
        public Project Project { get; set; }
        public Team Team { get; set; }
        public TeamMember TeamMember { get; set; }
        public Professor Professor { get; set; }
    }
}