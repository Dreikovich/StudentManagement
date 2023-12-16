using StudentManagementWebApi.Models;

namespace StudentManagementWebApi.Dtos;

public class AttendanceDto
{
    public int AttendanceID { get; set; }
    public StudentDto Student{ get; set; }
    public int SubjectID { get; set; }
    public string SubjectName { get; set; }
    public string SessionName { get; set; }
    public int GroupID { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string Status { get; set; }
    public string Comments { get; set; }
    public string Auditorium { get; set; }
    public Teacher Teacher { get; set; }
    
}