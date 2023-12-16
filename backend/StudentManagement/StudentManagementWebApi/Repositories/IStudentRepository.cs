using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface IStudentRepository
{
    List<StudentDto> GetAllStudents();
    // StudentDto GetStudentById(int studentId);
    void AddStudent(StudentDto student);
    // void UpdateStudent(StudentDto student, int studentId);
    // void DeleteStudent(int studentId);
}