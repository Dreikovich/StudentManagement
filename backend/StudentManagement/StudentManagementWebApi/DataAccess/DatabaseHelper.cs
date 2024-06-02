using System.Data;
using System.Data.SqlClient;

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
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(query, connection);
        var adapter = new SqlDataAdapter(command);
        var dataTable = new DataTable();
        adapter.Fill(dataTable);
        return dataTable;
    }

    public int ExecuteNonQuery(string query, SqlParameter[] parameters)
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(query, connection);
        command.Parameters.AddRange(parameters);
        var result = command.ExecuteNonQuery();
        return result;
    }

    public int ExecuteNonQuery(string query)
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(query, connection);
        var result = command.ExecuteNonQuery();
        return result;
    }

    public object ExecuteScalar(string sqlQuery)
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(sqlQuery, connection);
        return command.ExecuteScalar();
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}