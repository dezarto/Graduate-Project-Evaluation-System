namespace GPESAPI.Application.DTOs
{
    public class TeamCreator
    {
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string TeamName { get; set; }
        public int AdvisorId { get; set; }
        public List<StudenLists> StudentList { get; set; }
    }

    public class StudenLists
    {
        public string StudentFullName { get; set; }
        public string StudenNumber { get; set; }
    }
}
