namespace StudentManagementWebApi.Dtos;

public class AttendanceCreationDto
{
    public int SubjectID { get; set; }
    public int GroupID { get; set; }
    public int TypeID { get; set; }
    public int StudentID { get; set; }
    public int TeacherID { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public string? Comments { get; set; }
    public string Auditorium { get; set; }
}