using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface ISubjectGroupAssignmentRepository
{
    List<SubjectGroupAssignmentDto> GetSubjectGroupAssignments();
    
    void AddSubjectGroupAssignment(SubjectGroupAssignmentCreateDto subjectGroupAssignmentCreateDto);
    
}