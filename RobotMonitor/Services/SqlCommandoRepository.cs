using System.Data;
using Microsoft.Data.SqlClient;
public class SqlCommandoRepository : ISqlCommandoRepository
{
    private string _connectionString;

    public SqlCommandoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<CommandItem>> GetCommands()
    {
        var commands = new List<CommandItem>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT TOP 5 * FROM Commando ORDER BY Tijdstip DESC;";

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var item = new CommandItem
            {
                Datum = reader.GetDateTime(2),
                Tijdstip = reader.GetTimeSpan(3),
                Soort = reader.GetString(4)
            };
            commands.Add(item);
        }

        return commands;
    }
    public async void InsertCommand(string commandUsed, int robotId)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Commando (RobotId, Datum, Tijdstip, Soort) VALUES (@RobotId, @Datum, @Tijdstip, @Soort)";
            command.Parameters.Add("@RobotId", SqlDbType.Int).Value = robotId;
            command.Parameters.Add("@Datum", SqlDbType.Date).Value = DateTime.Now.Date;
            command.Parameters.Add("@Tijdstip", SqlDbType.Time).Value = DateTime.Now.TimeOfDay;
            command.Parameters.Add("@Soort", SqlDbType.VarChar).Value = commandUsed;

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inserting command: " + ex.Message);
        }
    }
}

public class CommandItem
{
    public DateTime Datum { get; set; }
    public TimeSpan Tijdstip { get; set; }
    public string? Soort { get; set; }
}