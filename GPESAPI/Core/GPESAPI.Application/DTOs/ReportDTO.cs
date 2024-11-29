namespace GPESAPI.Application.DTOs
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int TeamId { get; set; }
        public string FilePath { get; set; }
        public DateTime ReportDate { get; set; }

        public TeamDTO Team { get; set; }
    }
}
