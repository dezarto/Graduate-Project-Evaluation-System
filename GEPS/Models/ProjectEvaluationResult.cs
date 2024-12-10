namespace GEPS.Models
{
    public class ProjectEvaluationResult
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int ProjectId { get; set; }
        public int AdvisorId { get; set; }
        public bool IsActive { get; set; }

        public List<ProfessorTeam> EvaluationCriteria { get; set; }

    }



    //  ProjectResultController içerisinde ki GetProjectTeamResult bölmesi için eklenmiştir. ProjectTeamResult, ProfessorTeam, EvaluationCriteria modellerini kullanır.
    public class ProjectTeamResult
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public List<ProfessorTeam> ProfessorsTeams { get; set; }

    }


    public class ProfessorTeam
    {
        public int ProfessorId { get; set; }
        public string FullName { get; set; }
        public string MailAddress { get; set; }
        public int EvaluationScore { get; set; }
        public string GeneralComments { get; set; }
        public List<EvaluationCriteria> EvaluationCriterias { get; set; }
    }

    public class EvaluationCriteria
    {
        public int CriteriaId { get; set; }
        public bool IsChecked { get; set; }
        public int Score { get; set; }
        public string Feedback { get; set; }
    }

}
