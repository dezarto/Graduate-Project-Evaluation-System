namespace GraduateProjectEvaluationSystemAPI.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int TeamId { get; set; }
        public string FilePath { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
