using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Mappers;

public class StudentGroupMapper : IStudentGroupMapper
{
    public List<StudentGroupsDto> MapStudentGroupsEntityToStudentGroupDto(List<StudentGroupsEntity> studentGroupsEntities)
    {
        // Grouping by group_name and academic_year
        var groupedStudentGroups = studentGroupsEntities.GroupBy(s => new {s.GroupName, s.AcademicYear});
        var result = groupedStudentGroups.Select(group =>
        {
            var firstGroup = group.First(); // Assuming all objects in the group have the same group_name and academic_year
            return new StudentGroupsDto
            {
                StudentGroupID = firstGroup.StudentGroupID,
                GroupName = firstGroup.GroupName,
                AcademicYear = firstGroup.AcademicYear,
                Students = group
                    .Where(s => s.StudentEntity!= null) 
                    .Select(s => new StudentDto
                    {
                        StudentID = s.StudentEntity.StudentID,
                        FirstName = s.StudentEntity.FirstName,
                        LastName = s.StudentEntity.LastName,
                        Email = s.StudentEntity.Email,
                        Gender = s.StudentEntity.Gender,
                        Status = s.StudentEntity.Status
                    })
                    .ToList()
            };
        }).ToList();
        return result;

    }
}