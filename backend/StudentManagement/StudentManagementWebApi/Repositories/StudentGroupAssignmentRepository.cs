using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public class StudentGroupAssignmentRepository : IStudentGroupAssignmentRepository
{
    private DatabaseHelper _databaseHelper;
    
    public StudentGroupAssignmentRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
    }
    
    public void AddStudentGroupAssignment(StudentGroupAssignmentDto studentGroupAssignmentDto)
    {
        try
        {
            string query = $"INSERT INTO StudentGroupAssignment (StudentID, StudentGroupID) VALUES ('{studentGroupAssignmentDto.StudentID}', '{studentGroupAssignmentDto.StudentGroupID}')";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}