namespace GPESAPI.Application.DTOs
{
    public class ChecklistItemDetailDTO
    {
        public int EvaluationId { get; set; }
        public int ItemId { get; set; }
        public bool isChecked { get; set; }
        public string Feedback { get; set; }
    }
}
