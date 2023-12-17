using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Mappers;

public interface IStudentGroupMapper
{
    List<StudentGroupsDto> MapStudentGroupsEntityToStudentGroupDto(List<StudentGroupsEntity> studentGroupsEntities);
}