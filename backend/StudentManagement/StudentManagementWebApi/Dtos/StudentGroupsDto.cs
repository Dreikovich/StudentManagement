namespace StudentManagementWebApi.Dtos;

public class StudentGroupsDto
{
    public int StudentGroupID { get; set; }
    public List<StudentDto> Students { get; set; }
    public string GroupName { get; set; }
    public string AcademicYear { get; set; }
}