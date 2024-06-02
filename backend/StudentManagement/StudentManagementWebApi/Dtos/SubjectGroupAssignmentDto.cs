namespace StudentManagementWebApi.Dtos;

public class SubjectGroupAssignmentDto
{
    public int StudentGroupID { get; set; }
    public string GroupName { get; set; }
    public int SubjectID { get; set; }
    public string SubjectName { get; set; }
}

public class SubjectGroupAssignmentCreateDto
{
    public int StudentGroupID { get; set; }
    public int SubjectID { get; set; }
}