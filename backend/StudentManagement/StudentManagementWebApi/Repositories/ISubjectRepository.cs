using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface ISubjectRepository
{
    List<SubjectDto> GetAllSubjects();
    
    SubjectDto GetSubjectById(int subjectId, int typeId);
    
    void CreateSubject(SubjectCreationDto subjectCreationDto);
}