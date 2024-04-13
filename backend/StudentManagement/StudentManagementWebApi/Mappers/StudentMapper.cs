using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Mappers;

public class StudentMapper: IStudentMapper
{
    public StudentDto MapStudentEntityToStudentDto(StudentEntity student)
    {
        return new StudentDto()
            {
                StudentID = student.StudentID,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                GroupName = student.GroupName,
                Gender = student.Gender,
                Status = student.Status,
                StudentUuid = student.StudentUuid.ToString()
            };
    }
    
    public StudentEntity MapStudentDtoToStudentEntity(StudentDto studentDto)
    {
        return new StudentEntity()
        {
            StudentID = studentDto.StudentID,
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            Email = studentDto.Email,
            Gender = studentDto.Gender,
            Status = studentDto.Status,
            StudentUuid = Guid.Parse(studentDto.StudentUuid)
        };
    }
}