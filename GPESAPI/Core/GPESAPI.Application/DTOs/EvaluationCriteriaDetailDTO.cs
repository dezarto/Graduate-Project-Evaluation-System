namespace GPESAPI.Application.DTOs
{
    public class EvaluationCriteriaDetailDTO
    {
        public int EvaluationId { get; set; }
        public int CriteriaId { get; set; }
        public bool isChecked { get; set; }
        public double Score { get; set; }
        public string Feedback { get; set; }
    }
}
