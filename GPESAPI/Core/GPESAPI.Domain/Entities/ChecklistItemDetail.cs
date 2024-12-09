namespace GPESAPI.Domain.Entities
{
    public class ChecklistItemDetail
    {
        public int EvaluationId { get; set; }
        public int ItemId { get; set; }
        public bool isChecked { get; set; }
        public string Feedback { get; set; }
    }
}
