using System.Data;
using Microsoft.Data.SqlClient;
using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly DataHelper _dataHelper;
    private readonly DatabaseHelper _databaseHelper;
    public GradeRepository(IConfiguration configuration)
    {
        _dataHelper = new DataHelper();
        _databaseHelper = new DatabaseHelper(configuration);
        
    }
    
    public void AddGrade(GradeDto gradeDto)
    {
        try
        {
            string query =
                $"INSERT INTO Grades (StudentID, SubjectID, TypeID, TeacherID, GradeValue) VALUES ('{gradeDto.StudentID}', '{gradeDto.SubjectID}', '{gradeDto.TypeID}', '{gradeDto.TeacherID}', '{gradeDto.GradeValue}')";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
