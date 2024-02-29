using StudentManagementWebApi.Attributes;

namespace StudentManagementWebApi.Entities;

public class StudentEntity
{
    public int StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public string? GroupName { get; set; }
    
    public string Gender { get; set; }
    public string Status { get; set;}
    
}