using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public interface IGradeRepository
{
    void AddGrade(GradeDto gradeDto);
}