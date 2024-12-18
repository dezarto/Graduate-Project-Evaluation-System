namespace GPESAPI.Application.DTOs
{
    public class TeamCreator
    {
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string TeamName { get; set; }
        public List<StudenLists> StudentList { get; set; }
        public int SelectedProfessorId { get; set; }
    }

    public class StudenLists
    {
        public string StudentFullName { get; set; }
        public string StudentNumber { get; set; }
    }
}
