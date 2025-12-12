using System.Data;
using Microsoft.Data.SqlClient;
public class SqlRobotRepository : ISqlRobotRepository
{
    private string _connectionString;

    public SqlRobotRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Robot>> SelectAllRobots()
    {
        var robots = new List<Robot>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Robot";

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var item = new Robot
            {
                robotID = reader.GetInt32(0),
                Naam = reader.GetString(1),
                Model = reader.GetString(2)
            };
            robots.Add(item);
        }
        return robots;
    }
    public async void UpdateRobot(int robotId, string name, string model)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"UPDATE Robot SET Naam = @Naam, Model = @Model WHERE RobotID = @RobotID";

            command.Parameters.Add("@Naam", SqlDbType.NVarChar, 100).Value = name;
            command.Parameters.Add("@Model", SqlDbType.NVarChar, 100).Value = model;
            command.Parameters.Add("@RobotID", SqlDbType.Int).Value = robotId;

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating robot: " + ex.Message);
        }
    }

}

public class Robot
{
    public int robotID { get; set; }
    public string? Naam { get; set; }
    public string? Model { get; set; }
}