namespace GEPS.Models
{
    public class EvaluateReasult
    {
        public int EvaluationId { get; set; }
        public int TeamId { get; set; }
        public string GeneralComments { get; set; }
        public double TotalScore { get; set; }
        public DateTime Date { get; set; }
        public List<EvaluationCriteriaResult> EvaluationCriterias { get; set; }
        public List<EvaluationChecklistItemResult> EvaluationChecklistItems { get; set; }
    }

    public class EvaluationCriteriaResult
    {
        public int CriteriaId { get; set; }
        public bool isChecked { get; set; }
        public double Score { get; set; }
        public string Feedback { get; set; }
    }

    public class EvaluationChecklistItemResult
    {
        public int ItemId { get; set; }
        public bool isChecked { get; set; }
        public string Feedback { get; set; }
    }
}

