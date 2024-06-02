namespace StudentManagementWebApi.Entities;

public class LoginEntity
{
    public int StudentUuid { get; set; } // Foreign key linked to StudentEntity
    public string Login { get; set; }
    public string HashedPassword { get; set; }
    public string BearerToken { get; set; }
    public DateTime TokenExpirationDate { get; set; }


    //public StudentEntity Student { get; set; }
}