using System.Data;
using Microsoft.Data.SqlClient;
public class SqlBatteryRepository : ISqlBatteryRepository
{
    private string _connectionString;

    public SqlBatteryRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> GetBatteryStatusInMillivoltsAsync(int robotID)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT TOP 1 Percentage FROM BatterijStatus WHERE RobotID = @RobotID ORDER BY Tijdstip DESC ";
        command.Parameters.AddWithValue("@RobotID", robotID);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt32(0);
        }

        return 0;
    }
    public async void InsertBatteryLevel(int batteryValue, int robotId)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO BatterijStatus (RobotId, Tijdstip, Percentage) VALUES (@RobotId, @Tijdstip, @Percentage)";
            command.Parameters.Add("@RobotId", SqlDbType.Int).Value = robotId;
            command.Parameters.Add("@Tijdstip", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@Percentage", SqlDbType.Int).Value = batteryValue;

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inserting battery level: " + ex.Message);
        }
    }
}