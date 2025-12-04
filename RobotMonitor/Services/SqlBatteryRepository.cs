using Microsoft.Data.SqlClient;
public class SqlBatteryRepository : ISqlBatteryRepository
{
    private string _connectionString;

    public SqlBatteryRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> GetBatteryStatusInMillivoltsAsync()
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT TOP 1 Percentage FROM BatterijStatus ORDER BY Tijdstip DESC";

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt32(0);   // kolom 0 = Millivolts
        }

        return 0; // geen data in tabel
    }
}