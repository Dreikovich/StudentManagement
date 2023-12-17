using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using StudentManagementWebApi.Attributes;

namespace StudentManagementWebApi.Models;

public class Teacher
{
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int TeacherID { get; set; }
    public string TeacherFirstName { get; set; }
    public string TeacherLastName { get; set; }
    public string TeacherEmail { get; set; }
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SubjectName {get; set;}
    [SwaggerExclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Schedule? Schedule { get; set; }
    
    public string Degree { get; set; }
}






