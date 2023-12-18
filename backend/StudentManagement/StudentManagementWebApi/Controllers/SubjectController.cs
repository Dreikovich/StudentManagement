using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectController: ControllerBase
{   
    private readonly ISubjectRepository _subjectRepository;
    
public SubjectController(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }
    
    [HttpGet]
    public IActionResult GetAllSubjects()
    {
        try
        {
            var subjects = _subjectRepository.GetAllSubjects();
            return Ok(subjects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}