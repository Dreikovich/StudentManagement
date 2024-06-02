namespace StudentManagementWebApi.Entities;

public class SubjectGroupAssignmentEntity
{
    public int StudentGroupID { get; set; }
    public string GroupName { get; set; }
    public int SubjectID { get; set; }
    public string SubjectName { get; set; }
}