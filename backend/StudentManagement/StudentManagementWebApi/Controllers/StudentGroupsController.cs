using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGroupsController : ControllerBase
{
    private readonly IStudentGroupsRepository _studentGroupsRepository;

    public StudentGroupsController(IStudentGroupsRepository studentGroupsRepository)
    {
        _studentGroupsRepository = studentGroupsRepository;
    }

    [HttpGet]
    public IActionResult GetAllStudentGroups()
    {
        try
        {
            var studentGroups = _studentGroupsRepository.GetAllStudentGroups();
            return Ok(studentGroups);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}