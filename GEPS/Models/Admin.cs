namespace GEPS.Models
{


    public class AdminEvaluationCriteria
    {
        public int CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public int Percent { get; set; }
    }

    public class AdminChecklistItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
