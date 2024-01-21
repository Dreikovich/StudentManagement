using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Repositories;

public class SubjectGroupAssignmentRepository : ISubjectGroupAssignmentRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;
    public SubjectGroupAssignmentRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
    }
    
    public List<SubjectGroupAssignmentDto> GetSubjectGroupAssignments()
    {
        string query = "select * from SubjectStudentGroup as SSG " +
                       "Join Subjects S on SSG.SubjectID = S.SubjectID " +
                       "Join StudentGroups SG on SSG.StudentGroupID = SG.StudentGroupID";
        var subjectGroupAssignmentsDataTable = _databaseHelper.ExecuteQuery(query);
        var subjectGroupAssignmentsEntities = _dataHelper.DataTableToList<SubjectGroupAssignmentEntity>(subjectGroupAssignmentsDataTable);
        var result = subjectGroupAssignmentsEntities.Select(s => new SubjectGroupAssignmentDto
        {
            SubjectID = s.SubjectID,
            SubjectName = s.SubjectName,
            StudentGroupID = s.StudentGroupID,
            GroupName = s.GroupName
        }).ToList();
        return result;
    }
    
    public void AddSubjectGroupAssignment(SubjectGroupAssignmentCreateDto subjectGroupAssignmentCreateDto)
    {
        try
        {
            string query = $"INSERT INTO SubjectStudentGroup (SubjectID, StudentGroupID) VALUES ('{subjectGroupAssignmentCreateDto.SubjectID}', '{subjectGroupAssignmentCreateDto.StudentGroupID}')";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}