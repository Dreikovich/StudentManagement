using StudentManagementWebApi.Models;

namespace StudentManagementWebApi.Entities;

public class SubjectEntity
{
    public int SubjectID { get; set; }
    public string SubjectName { get; set; }
    public Teacher Teacher { get; set; }
    public int TypeID { get; set; }
    public string TypeName { get; set; }
    public int Hours { get; set; }
}