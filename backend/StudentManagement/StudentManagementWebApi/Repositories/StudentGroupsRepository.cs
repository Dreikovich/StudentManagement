using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;
using StudentManagementWebApi.Mappers;

namespace StudentManagementWebApi.Repositories;

public class StudentGroupsRepository : IStudentGroupsRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly IStudentGroupMapper _studentGroupMapper;
    private readonly DataHelper _dataHelper;

    public StudentGroupsRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _studentGroupMapper = new StudentGroupMapper();
        _dataHelper = new DataHelper();
    }

    public List<StudentGroupsDto> GetAllStudentGroups()
    {
        string query = "select * from studentGroups " +
                       "JOIN StudentGroupAssignment SGA on studentGroups.StudentGroupID = SGA.StudentGroupID " +
                       "JOIN Students S on S.StudentID = SGA.StudentID";
        var studentGroupsDataTable = _databaseHelper.ExecuteQuery(query);
        var studentGroupsEntities = _dataHelper.DataTableToList<StudentGroupsEntity>(studentGroupsDataTable);
        return _studentGroupMapper.MapStudentGroupsEntityToStudentGroupDto(studentGroupsEntities);

    }
}
