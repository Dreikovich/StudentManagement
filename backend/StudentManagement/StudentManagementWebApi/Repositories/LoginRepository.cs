using System.Data;
using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly DataHelper _dataHelper;
    private readonly DatabaseHelper _databaseHelper;
    
    public LoginRepository(IConfiguration configuration)
    {
        _dataHelper = new DataHelper();
        _databaseHelper = new DatabaseHelper(configuration);
    }

    public void AddLogin(LoginDto loginDto)
    {
        try
        {
            string query =
                $"INSERT INTO StudentLogins (StudentUUID, Login, HashedPassword) VALUES ('{loginDto.StudentUuid}', '{loginDto.Login}', '{HashPassword(loginDto.Password)}')";
            _databaseHelper.ExecuteNonQuery(query);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public bool ValidateLogin(LoginDto loginDto)
    {
        try
        {
            string studentUuid = GetStudentUuid(loginDto.Login);
            loginDto.StudentUuid = studentUuid;
            if (studentUuid == null)
            {
                return false;
            }
            string query = $"SELECT HashedPassword FROM StudentLogins WHERE Login = '{loginDto.Login}' and StudentUUID = '{studentUuid}'";
            DataTable dataTable =  _databaseHelper.ExecuteQuery(query);
            if (dataTable.Rows.Count == 0)
            {
                return false;
            }
            string hashedPassword = dataTable.Rows[0]["HashedPassword"].ToString();
            return VerifyPassword(loginDto.Password, hashedPassword);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void SaveTokenToDatabase(string token, DateTime expirationDate, string studentUuid)
    {
        try
        {
            string query = $"UPDATE StudentLogins Set BearerToken = '{token}', TokenExpirationDate='{expirationDate}' WHERE StudentUUID = '{studentUuid}'";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private string GetStudentUuid(string login)
    {
        string query = $"SELECT StudentUUID FROM StudentLogins WHERE Login = '{login}'";
        DataTable dataTable = _databaseHelper.ExecuteQuery(query);
        if (dataTable.Rows.Count > 0)
        {
            return dataTable.Rows[0]["StudentUUID"].ToString();
        }
        return null;
    }

    public bool IsTokenValid(string token)
    {
        string query = $"SELECT Count(*) FROM StudentLogins WHERE BearerToken='{token}'";
        DataTable dataTable = _databaseHelper.ExecuteQuery(query);
        var count = int.Parse(dataTable.Rows[0][0].ToString());
        return count > 0;
    }

    private string HashPassword(string password)
    {
         return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    private bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}