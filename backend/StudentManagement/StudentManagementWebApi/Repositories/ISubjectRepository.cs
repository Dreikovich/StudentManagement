using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface ISubjectRepository
{
    List<SubjectDto> GetAllSubjects();
    
    void CreateSubject(SubjectCreationDto subjectCreationDto);
}