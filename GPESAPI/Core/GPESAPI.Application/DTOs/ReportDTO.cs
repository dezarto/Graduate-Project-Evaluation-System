namespace GPESAPI.Application.DTOs
{
    public class ReportDTO
    {
        public int ReportId { get; set; }
        public int TeamId { get; set; }
        public string FilePath { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
