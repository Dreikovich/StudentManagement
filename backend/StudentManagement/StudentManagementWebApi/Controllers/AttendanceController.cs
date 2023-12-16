using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceRepository _attendanceRepository;
    
    public AttendanceController(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }
    
    [HttpGet]
    public IActionResult GetAllAttendances([FromQuery] string group_name, [FromQuery] string academic_year)
    {
        try
        {
            var attendances = _attendanceRepository.GetAllAttendances(group_name, academic_year);
            
            return Ok(attendances);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
}