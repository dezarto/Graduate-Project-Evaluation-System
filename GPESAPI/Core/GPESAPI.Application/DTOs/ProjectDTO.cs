﻿namespace GraduateProjectEvaluationSystemAPI.Application.DTOs
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public ICollection<TeamDTO> Teams { get; set; }
    }
}