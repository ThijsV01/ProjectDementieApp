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

    public void SelectInteractionMoments()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        connection.Open();
        Console.WriteLine("servernaam:" +connection.Database);
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM InteractieMomenten";
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            int interactionMomentId = reader.GetInt32(0);
            int robotId = reader.GetInt32(1);
            DateTime timestamp = reader.GetDateTime(2);
            Console.WriteLine($"{interactionMomentId} - {robotId} - {timestamp}");
        }
        connection.Close();
    }


}