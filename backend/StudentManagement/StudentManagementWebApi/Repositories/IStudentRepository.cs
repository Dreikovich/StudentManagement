using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface IStudentRepository
{
    List<StudentDto> GetAllStudents(string search, List<string> filters);

    StudentDto GetStudentById(int studentId);

    // StudentDto GetStudentById(int studentId);
    void AddStudent(StudentDto student);
    // void UpdateStudent(StudentDto student, int studentId);
    //void DeleteStudent(int studentId);
}