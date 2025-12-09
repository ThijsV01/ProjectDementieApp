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

    public async Task<List<TimeSpan>> SelectInteractionMoments()
    {
        List<TimeSpan>interactionTimes=new List<TimeSpan>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Tijdstip FROM InteractieMoment";
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            TimeSpan timeSpan=new TimeSpan();
            timeSpan=reader.GetTimeSpan(0);
            interactionTimes.Add(timeSpan);
        }
        connection.Close();

        return interactionTimes;
    }


}