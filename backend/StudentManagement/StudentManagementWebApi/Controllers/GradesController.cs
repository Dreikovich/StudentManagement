using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
private readonly IGradeRepository _gradeRepository;
    
    public GradesController(IGradeRepository gradeRepository)
    {
        _gradeRepository = gradeRepository;
    }
    
    [HttpPost]
    public IActionResult AddGrade([FromBody] GradeDto gradeDto)
    {
        try
        {
            _gradeRepository.AddGrade(gradeDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
}