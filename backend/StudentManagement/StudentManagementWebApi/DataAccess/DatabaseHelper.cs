using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using StudentManagementWebApi.Models;

namespace StudentManagementWebApi.DataAccess;

public class DatabaseHelper
{
    private readonly string _connectionString;
    
    public DatabaseHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public DataTable ExecuteQuery(string query)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand command = new SqlCommand(query, connection);
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);
        return dataTable;
    }
    
    public int ExecuteNonQuery(string query, SqlParameter[] parameters)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddRange(parameters);
        int result = command.ExecuteNonQuery();
        return result;
    }
    
    public int ExecuteNonQuery(string query)
        {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand command = new SqlCommand(query, connection);
        int result = command.ExecuteNonQuery();
        return result;
    }
    
    public object ExecuteScalar(string sqlQuery)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        SqlCommand command = new SqlCommand(sqlQuery, connection);
        return command.ExecuteScalar();
    }
    
    
}