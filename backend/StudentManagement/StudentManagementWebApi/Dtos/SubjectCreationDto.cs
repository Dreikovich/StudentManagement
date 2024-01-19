namespace StudentManagementWebApi.Dtos;

public class SubjectCreationDto
{
    public string SubjectName { get; set; }
    public List<SubjectComponentDto> SubjectComponents { get; set; }
}

public class SubjectComponentDto
{

    public int TypeID { get; set; }
    public int Hours { get; set; }
    public int TeacherID  { get; set; }

}