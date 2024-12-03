namespace GEPS.Models
{
    public class ProjectEvaluationSubmit
    {
        public int TeamId { get; set; }
        public string GeneralComments { get; set; }
        public int TotalScore { get; set; }
        public DateTime Date { get; set; }
        public List<EvaluationCriteria> EvaluationCriterias { get; set; }
        public List<EvaluationChecklistItem> EvaluationChecklistItems { get; set; }
    }

    public class EvaluationCriteriaItem
    {
        public int CriteriaId { get; set; }
        public bool IsChecked { get; set; }
        public int Score { get; set; }
        public string Feedback { get; set; }
    }

    public class EvaluationChecklistItem
    {
        public int ItemId { get; set; }
        public bool IsChecked { get; set; }
        public string Feedback { get; set; }
    }
}

