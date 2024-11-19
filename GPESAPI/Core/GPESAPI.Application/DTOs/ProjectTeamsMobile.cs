using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPESAPI.Application.DTOs
{
    public class ProjectTeamsMobile
    {
        public int TeamId { get; set; }
        public int TeamPresentationId { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public bool isEvaluated { get; set; }
        public string EvaluatingTeacherFullName { get; set; }
        public string EvaluatingTeacherMail { get; set; }
        public string ProjectName { get; set; }
    }
}
