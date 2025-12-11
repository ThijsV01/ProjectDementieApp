using System.Data;
using HiveMQtt.MQTT5.Types;
using Microsoft.Data.SqlClient;
using SimpleMqtt;
public class SqlInteractionMomentsRepository : ISqlInteractionMomentsRepository
{
    private string _connectionString;

    public SqlInteractionMomentsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<InteractieMoment>> SelectInteractionMoments(int robotID)
    {
        List<InteractieMoment> interactionTimes = new List<InteractieMoment>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM InteractieMoment ORDER BY Tijdstip ASC";
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var item = new InteractieMoment
            {
                InteractieID = reader.GetInt32(0),
                RobotID = reader.GetInt32(1),
                Tijdstip = reader.GetTimeSpan(2)
            };
            interactionTimes.Add(item);
        }

        return interactionTimes;
    }

    public async void DeleteMoment(int interactieMomentId)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM InteractieMoment WHERE InteractieMomentID = @Id";
            command.Parameters.Add("@Id", SqlDbType.Int).Value = interactieMomentId;

            await command.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deleting moment: " + ex.Message);
        }
    }
    public async void AddMoment(int robotId, TimeOnly time)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM InteractieMoment 
                    WHERE RobotId = @RobotId AND Tijdstip = @Tijdstip
                )
                BEGIN
                    INSERT INTO InteractieMoment (RobotId, Tijdstip) 
                    VALUES (@RobotId, @Tijdstip)
                END
            ";
            command.Parameters.Add("@RobotId", SqlDbType.Int).Value = robotId;
            command.Parameters.Add("@Tijdstip", SqlDbType.Time).Value = time;

            await command.ExecuteNonQueryAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding moment: " + ex.Message);
        }
    }
}

public class InteractieMoment
{
    public int InteractieID { get; set; }
    public int RobotID { get; set; }
    public TimeSpan Tijdstip { get; set; }
}