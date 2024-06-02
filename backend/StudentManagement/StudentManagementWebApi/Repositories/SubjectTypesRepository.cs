using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Entities;

namespace StudentManagementWebApi.Repositories;

public class SubjectTypesRepository : ISubjectTypesRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;

    public SubjectTypesRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
    }

    public List<SubjectTypesEntity> GetSubjectTypes()
    {
        var query = "select * from SubjectTypes";
        var subjectTypesDataTable = _databaseHelper.ExecuteQuery(query);
        var subjectTypesEntities = _dataHelper.DataTableToList<SubjectTypesEntity>(subjectTypesDataTable);
        return subjectTypesEntities;
    }
}