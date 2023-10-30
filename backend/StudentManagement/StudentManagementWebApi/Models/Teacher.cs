using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using StudentManagementWebApi.Attributes;

namespace StudentManagementWebApi.Models;

public class Teacher
{
    [SwaggerExclude]
    public int TeacherID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SubjectName {get; set;}
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schedule? Schedule { get; set; }
}






