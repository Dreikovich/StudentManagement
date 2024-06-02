using Microsoft.AspNetCore.Mvc;
using StudentManagementWebApi.Repositories;

namespace StudentManagementWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectTypesController : ControllerBase
{
    private readonly ISubjectTypesRepository _subjectTypesRepository;

    public SubjectTypesController(ISubjectTypesRepository subjectTypesRepository)
    {
        _subjectTypesRepository = subjectTypesRepository;
    }

    [HttpGet]
    public IActionResult GetSubjectTypes()
    {
        try
        {
            var subjectTypes = _subjectTypesRepository.GetSubjectTypes();
            return Ok(subjectTypes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}