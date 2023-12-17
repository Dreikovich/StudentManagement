namespace StudentManagementWebApi.Entities;

public class StudentGroupsEntity
{
    public int StudentGroupID { get; set; }
    public StudentEntity StudentEntity { get; set; }
    public string GroupName { get; set; }
    public string AcademicYear { get; set; }
}