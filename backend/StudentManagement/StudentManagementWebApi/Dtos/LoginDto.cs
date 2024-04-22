namespace StudentManagementWebApi.Dtos;

public class LoginDto
{
    public string? StudentUuid { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}