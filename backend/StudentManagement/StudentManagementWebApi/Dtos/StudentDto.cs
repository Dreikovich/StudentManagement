using System.Text.Json.Serialization;
using StudentManagementWebApi.Attributes;

namespace StudentManagementWebApi.Dtos;

public class StudentDto
{   
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int StudentID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public string Status { get; set; }
    
    public string StudentUuid { get; set; }
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? GroupName { get; set; }
}