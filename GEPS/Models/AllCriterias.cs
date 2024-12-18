namespace GEPS.Models
{
    public class AllCriterias
    {
        public List<EvaluationCriterias> EvaluationCriteriaDatas { get; set; }
        public List<ChecklistItems> ChecklistItemDatas { get; set; }
    }

    public class EvaluationCriterias
    {
        public int CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public double Precent { get; set; }
    }

    public class ChecklistItems
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
