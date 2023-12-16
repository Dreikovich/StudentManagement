using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;
using StudentManagementWebApi.Entities;
using StudentManagementWebApi.Mappers;

namespace StudentManagementWebApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly IStudentMapper _studentMapper;
    private readonly DataHelper _dataHelper;
    
    public StudentRepository(IConfiguration configuration)
    {
        _databaseHelper = new DatabaseHelper(configuration);
        _dataHelper = new DataHelper();
        _studentMapper = new StudentMapper();
    }
    
    public List<StudentDto> GetAllStudents()
    {
        try
        {
            string query = "Select S.StudentID, S.FirstName, S.LastName, S.Email, SG.GroupName from students as S" +
                           " Left Join StudentGroupAssignment SGA on S.StudentID = SGA.StudentID" +
                           " Left Join StudentGroups SG on SGA.StudentGroupID = SG.StudentGroupID";
            var studentsDataTable = _databaseHelper.ExecuteQuery(query);
            var studentEntities = _dataHelper.DataTableToList<StudentEntity>(studentsDataTable);
            var studentDtos = new List<StudentDto>();
            foreach (var entity in studentEntities)
            {
                studentDtos.Add(_studentMapper.MapStudentEntityToStudentDto(entity));
            }

            return studentDtos;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public void AddStudent(StudentDto studentDto)
    {
        try
        {
            var studentEntity = _studentMapper.MapStudentDtoToStudentEntity(studentDto);
            string query = $"INSERT INTO Students (FirstName, LastName, Email) VALUES ('{studentEntity.FirstName}', '{studentEntity.LastName}', '{studentEntity.Email}')";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}