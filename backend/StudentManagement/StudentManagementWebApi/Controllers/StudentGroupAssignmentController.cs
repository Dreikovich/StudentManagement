using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGroupAssignmentController : ControllerBase
{
    private readonly IStudentGroupAssignmentRepository _studentGroupAssignmentRepository;

    public StudentGroupAssignmentController(IStudentGroupAssignmentRepository studentGroupAssignmentRepository)
    {
        _studentGroupAssignmentRepository = studentGroupAssignmentRepository;
    }

    [HttpPost]
    public IActionResult AddStudentGroupAssignment(StudentGroupAssignmentDto studentGroupAssignmentDto)
    {
        try
        {
            _studentGroupAssignmentRepository.AddStudentGroupAssignment(studentGroupAssignmentDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}