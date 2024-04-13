using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface IStudentGroupsRepository
{
    public List<StudentGroupsDto> GetAllStudentGroups();
    
    
    
    
}