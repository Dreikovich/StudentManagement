using StudentManagementWebApi.DataAccess;

namespace StudentManagementWebApi.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    
    private  DatabaseHelper _databaseHelper;
    
    public ScheduleRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
    }
    
    public void GetSchedule()
    {
        _databaseHelper.ExecuteQuery("SELECT * FROM Schedule");
    }
}