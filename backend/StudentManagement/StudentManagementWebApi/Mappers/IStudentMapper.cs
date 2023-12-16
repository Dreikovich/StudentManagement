using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Mappers;

public interface IStudentMapper
{
    public StudentDto MapStudentEntityToStudentDto(StudentEntity student);
    public StudentEntity MapStudentDtoToStudentEntity(StudentDto studentDto);
}