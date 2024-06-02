using StudentManagementWebApi.DataAccess;
using StudentManagementWebApi.Dtos;

namespace StudentManagementWebApi.Repositories;

public class LoginRepository : ILoginRepository
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly DataHelper _dataHelper;

    public LoginRepository(IConfiguration configuration)
    {
        _dataHelper = new DataHelper();
        _databaseHelper = new DatabaseHelper(configuration);
    }

    public void AddLogin(LoginDto loginDto)
    {
        try
        {
            var query =
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
            var query =
                $"SELECT HashedPassword FROM StudentLogins WHERE Login = '{loginDto.Login}' and StudentUUID = '{loginDto.StudentUuid}'";
            var dataTable = _databaseHelper.ExecuteQuery(query);
            if (dataTable.Rows.Count == 0) return false;
            var hashedPassword = dataTable.Rows[0]["HashedPassword"].ToString();
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
            var query =
                $"UPDATE StudentLogins Set BearerToken = '{token}', TokenExpirationDate='{expirationDate}' WHERE StudentUUID = '{studentUuid}'";
            _databaseHelper.ExecuteNonQuery(query);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string GetStudentUuid(string login)
    {
        //Todo is it good approach to use only login to get student uuid?
        var query = $"SELECT StudentUUID FROM StudentLogins WHERE Login = '{login}'";
        var dataTable = _databaseHelper.ExecuteQuery(query);
        if (dataTable.Rows.Count > 0) return dataTable.Rows[0]["StudentUUID"].ToString();
        return null;
    }

    public bool IsTokenValid(string token)
    {
        var query = $"SELECT Count(*) FROM StudentLogins WHERE BearerToken='{token}'";
        var dataTable = _databaseHelper.ExecuteQuery(query);
        var count = int.Parse(dataTable.Rows[0][0].ToString());
        return count > 0;
    }

    public string GetUserRole(LoginDto loginDto)
    {
        var query = $"SELECT Role FROM StudentLogins WHERE StudentUUID = '{loginDto.StudentUuid}'";
        var dataTable = _databaseHelper.ExecuteQuery(query);
        return dataTable.Rows[0]["Role"].ToString();
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