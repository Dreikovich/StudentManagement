using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;
    
    public StudentsController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    // [Authorize]
    [HttpGet]
    public IActionResult GetAllStudents([FromQuery] string search = null, [FromQuery] List<string> filters = null)
    {
        try
        {
            var students = _studentRepository.GetAllStudents(search, filters);
            return Ok(students);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpPost]
    public IActionResult AddStudent(StudentDto studentDto)
    {
        try
        {
            _studentRepository.AddStudent(studentDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpDelete]
    [Route("{studentId}")]
    public IActionResult DeleteStudent(int studentId)
    {
        try
        {
            // _studentRepository.DeleteStudent(studentId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
}