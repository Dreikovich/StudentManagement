using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Repositories;

public interface ISubjectTypesRepository
{
    List<SubjectTypesEntity> GetSubjectTypes();
}