namespace GPESAPI.Domain.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public int TeamId { get; set; }
        public string FilePath { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
