using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface IStudentGroupAssignmentRepository
{
    void AddStudentGroupAssignment(StudentGroupAssignmentDto studentGroupAssignmentDto);
}