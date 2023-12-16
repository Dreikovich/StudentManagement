using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Models;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;
    
    public TeachersController(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            string query = "SELECT * FROM Teachers ";
            DataTable dataTable = _databaseHelper.ExecuteQuery(query);
            var teachers = _dataHelper.DataTableToList<Teacher>(dataTable);
            return Ok(teachers);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet("teachers-with-subjects-and-schedule")]
    public IActionResult GetTeachersWithSubjectsAndSchedule()
    {
        try
        {
            string query = @"SELECT T.TeacherID, T.FirstName, T.LastName, T.Email, S.SubjectName, Sch.StartTime, Sch.EndTime, Sch.Room, Sch.Weekday
                FROM Teachers as T
                JOIN TeacherSubject TS on T.TeacherId = TS.TeacherID
                JOIN Subjects S on TS.SubjectID = S.SubjectID
                JOIN Schedule Sch on S.SubjectID = Sch.SubjectID";
            DataTable dataTable = _databaseHelper.ExecuteQuery(query);
            var teachers = _dataHelper.DataTableToList<Teacher>(dataTable);
            return Ok(teachers);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public IActionResult GetTeacher(int id)
    {
        try
        {
            string query = $"SELECT * FROM Teachers WHERE TeacherId = {id}";
            DataTable dataTable = _databaseHelper.ExecuteQuery(query);
            var teacher = _dataHelper.DataTableToObject<Teacher>(dataTable);

            if (teacher == null)
            {
                return NotFound($"Teacher with ID {id} not found.");
            }
            return Ok(teacher);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult AddTeacher([FromBody] Teacher teacher)
    {
        try
        {
            string query = "INSERT INTO Teachers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@FirstName", teacher.TeacherFirstName),
                new SqlParameter("@LastName", teacher.TeacherLastName),
                new SqlParameter("@Email", teacher.TeacherEmail)
                
            };
            _databaseHelper.ExecuteNonQuery(query, parameters);
            return Ok("Teacher added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateTeacher(int id, [FromBody] Teacher teacher)
    {
        try
        {
            string checkQuery = $"SELECT COUNT(*) FROM Teachers WHERE TeacherId = {id}";
            int teacherCount = (int)_databaseHelper.ExecuteScalar(checkQuery);

            if (teacherCount == 0)
            {
                return NotFound($"Teacher with ID {id} not found.");
            }
            string query = $"UPDATE Teachers SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE TeacherId = {id}";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@FirstName", teacher.TeacherFirstName),
                new SqlParameter("@LastName", teacher.TeacherLastName),
                new SqlParameter("@Email", teacher.TeacherEmail)
            };
            int rowsAffected = _databaseHelper.ExecuteNonQuery(query, parameters);
            if (rowsAffected > 0)
            {
                return Ok($"Teacher with {id} updated successfully.");
            }
            else
            {
                return StatusCode(500, $"Internal Server Error: Failed to update teacher with ID {id}.");
            }
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteTeacher(int id)
    {
        try
        {
            string checkQuery = $"SELECT COUNT(*) FROM Teachers WHERE TeacherId = {id}";
            int teacherCount = (int)_databaseHelper.ExecuteScalar(checkQuery);

            if (teacherCount == 0)
            {
                return NotFound($"Teacher with ID {id} not found.");
            }
            string deleteQuery = $"DELETE FROM Teachers WHERE TeacherId = {id}";
            int rowsAffected = _databaseHelper.ExecuteNonQuery(deleteQuery);

            if (rowsAffected > 0)
            {
                return Ok($"Teacher with ID {id} deleted successfully.");
            }
            else
            {
                return StatusCode(500, $"Internal Server Error: Failed to delete teacher with ID {id}.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    

}