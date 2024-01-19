using StudentManagementWebApi.Models;

namespace StudentManagementWebApi.Dtos;

public class SubjectDto
{
    public int SubjectID { get; set; }
    public string SubjectName { get; set; }
    public List<TypeDto> SubjectComponents { get; set; }

}

public class TypeDto
{
    public int TypeID { get; set; }
    public string TypeName { get; set; }
    public int Hours { get; set; }
    public Teacher Teacher { get; set; }
}