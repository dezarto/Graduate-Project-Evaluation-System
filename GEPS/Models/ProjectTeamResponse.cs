﻿namespace GEPS.Models
{
    public class ProjectTeamResponse
    {
        public int TeamId { get; set; }
        public int TeamPresentationId { get; set; }
        public List<StudentInfo> StudentsList { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public bool IsEvaluated { get; set; }
        public string EvaluatingTeacherFullName { get; set; }
        public string EvaluatingTeacherMail { get; set; }
        public string ProjectName { get; set; }
    }

    public class StudentInfo
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
    }
}