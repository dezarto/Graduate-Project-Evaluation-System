namespace GEPS.Models
{
    public class ProjectTeamResult
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public List<AllProfessorList> ProfessorsTeams { get; set; }
        public List<StudentLists> Members { get; set; }
    }

    public class AllProfessorList
    {
        public int ProfessorId { get; set; }
        public string FullName { get; set; }
        public string mailAddress { get; set; }
        public double EvaluationScore { get; set; }
        public string GeneralComments { get; set; }
        public List<EvaluationCriteriaResult> EvaluationCriterias { get; set; }
    }

    public class StudentLists
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
    }
}
