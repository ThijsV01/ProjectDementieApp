using System.Data;
using Microsoft.Data.SqlClient;
public class SqlSensorRepository : ISqlSensorRepository
{
    private string _connectionString;

    public SqlSensorRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertSensorData(int sensorValue, string sensorType)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            Console.WriteLine(sensorType);
            int sensorId = -1;

            using (SqlCommand command1 = connection.CreateCommand())
            {
                command1.CommandText = "SELECT SensorID FROM Sensor WHERE Type = @Type";
                command1.Parameters.Add("@Type", SqlDbType.VarChar).Value = sensorType;

                using SqlDataReader reader = await command1.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    sensorId = reader.GetInt32(0);
                }
            }

            if (sensorId == -1)
            {
                Console.WriteLine("SensorType not found.");
                return;
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO SensorMeting (SensorId, Datum, Tijdstip, Waarde) " +
                    "VALUES (@SensorId, @Datum, @Tijdstip, @Waarde)";

                command.Parameters.Add("@SensorId", SqlDbType.Int).Value = sensorId;
                command.Parameters.Add("@Datum", SqlDbType.DateTime).Value = DateTime.Now.Date;
                command.Parameters.Add("@Tijdstip", SqlDbType.Time).Value = DateTime.Now.TimeOfDay;
                command.Parameters.Add("@Waarde", SqlDbType.Int).Value = sensorValue;

                await command.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inserting sensor data: " + ex.Message);
        }
    }

    public async Task<List<SensorMeting>> SelectSensorDataObstacleDistance(int robotID)
    {
        var sensorDataObstacleDistance = new List<SensorMeting>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT TOP 5 sm.Datum, sm.Tijdstip, sm.Waarde FROM SensorMeting sm JOIN Sensor s ON sm.SensorID = s.SensorID WHERE s.Type = 'Ultrasonic Distance' AND s.RobotID = @RobotID ORDER BY sm.Datum DESC, sm.Tijdstip DESC;";
        command.Parameters.AddWithValue("@RobotID", robotID);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var item = new SensorMeting
            {
                Datum = reader.GetDateTime(0),
                Tijdstip = reader.GetTimeSpan(1),
                Waarde = reader.GetInt32(2)
            };
            sensorDataObstacleDistance.Add(item);
        }
        return sensorDataObstacleDistance;
    }
    public async Task<List<SensorMeting>> SelectSensorDataPIRMotion(int robotID)
    {
        var sensorDataPIRMotion = new List<SensorMeting>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT TOP 5 sm.Datum, sm.Tijdstip, sm.Waarde FROM SensorMeting sm JOIN Sensor s ON sm.SensorID = s.SensorID WHERE s.Type = 'PIR Motion' AND s.RobotID = @RobotID ORDER BY sm.Datum DESC, sm.Tijdstip DESC;";
        command.Parameters.AddWithValue("@RobotID", robotID);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var item = new SensorMeting
            {
                Datum = reader.GetDateTime(0),
                Tijdstip = reader.GetTimeSpan(1),
                Waarde = reader.GetInt32(2)
            };
            sensorDataPIRMotion.Add(item);
        }
        return sensorDataPIRMotion;
    }
}

public class SensorMeting
{
    public int SensorID { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan Tijdstip { get; set; }
    public int Waarde { get; set; }
}