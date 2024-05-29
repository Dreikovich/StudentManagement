namespace StudentManagementWebApi.Dtos;

public class GradeDto
{
    public int StudentID { get; set; }
    public int SubjectID { get; set; }
    public int TypeID { get; set; }
    public int TeacherID { get; set; }
    public string GradeValue { get; set; }
    public Guid? StudentUuid { get; set; }
}