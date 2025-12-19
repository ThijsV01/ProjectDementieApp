using System.Data;
using Microsoft.Data.SqlClient;
public class SqlActivitiesRepository : ISqlActivitiesRepository
{
    private string _connectionString;

    public SqlActivitiesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<QuestionAnswers>> GetQuestions(int robotID)
    {
        var questions = new List<QuestionAnswers>();

        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Question, CorrectAnswer, WrongAnswer, WrongAnswer2 FROM Quiz WHERE RobotID = @RobotID ";
        command.Parameters.AddWithValue("@RobotID", robotID);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var item = new QuestionAnswers
            {
                Question = reader.GetString(0),
                CorrectAnswer = reader.GetString(1),
                WrongAnswer = reader.GetString(2),
                WrongAnswer2 = reader.GetString(3)
            };
            questions.Add(item);
        }
        return questions;
    }
    public async void InsertInteraction(GameResult result)
    {
        try
        {
            Console.WriteLine(result);
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Interactie
                (
                    RobotId,Datum,TijdstipStart,TijdstipEind,Soort,GemReactieSnelheid,CorrectBeantwoord,Status, SimonSaysAantal
                )
                VALUES
                (
                    @RobotId,@Datum,@TijdstipStart,@TijdstipEind,@Soort,@GemReactieSnelheid,@CorrectBeantwoord,@Status, @SimonSaysAantal
                )";
            command.Parameters.Add("@RobotId", SqlDbType.Int).Value = result.RobotId;
            command.Parameters.Add("@Datum", SqlDbType.DateTime).Value = result.Date;
            command.Parameters.Add("@TijdstipStart", SqlDbType.Time).Value = result.StartTime;
            command.Parameters.Add("@TijdstipEind", SqlDbType.Time).Value = result.EndTime;
            command.Parameters.Add("@Soort", SqlDbType.NVarChar, 50).Value = result.KindOfGame;
            command.Parameters.Add("@GemReactieSnelheid", SqlDbType.Decimal).Value = result.AverageReactionTimeMs;
            command.Parameters.Add("@CorrectBeantwoord", SqlDbType.Decimal).Value = result.CorrectlyAnsweredPercentage;
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = result.InteractionState;
            command.Parameters.Add("@SimonSaysAantal", SqlDbType.Int).Value = result.SimonSaysAmount;

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inserting interaction: " + ex.Message);
        }
    }
    public async Task<List<string>> SelectActivities(int robotID)
    {
        List<string> activities = new List<string>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT DISTINCT Soort FROM Interactie WHERE RobotID = @RobotID AND Soort IS NOT NULL AND Soort <> 'no game chosen'";
        command.Parameters.AddWithValue("@RobotID", robotID);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            activities.Add(reader.GetString(0));
        }

        return activities;
    }
    public async Task<List<GameResult>> GetSelectedActivityData(int robotID, string activity)
    {
        List<GameResult> allSelectedActivityData = new List<GameResult>();
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Interactie WHERE Soort=@Soort AND RobotID=@RobotID";
        command.Parameters.AddWithValue("@RobotID", robotID);
        command.Parameters.AddWithValue("@Soort", activity);
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var item = new GameResult
            {
                RobotId = reader.GetInt32(1),
                Date=reader.GetDateTime(2),
                StartTime = reader.GetTimeSpan(3),
                EndTime = reader.GetTimeSpan(4),
                KindOfGame = reader.GetString(5),
                AverageReactionTimeMs = reader.GetDecimal(6),
                CorrectlyAnsweredPercentage = reader.GetDecimal(7),
                InteractionState = reader.GetString(8),
            };
            if (activity == "Simon Says" )
            {
                item.SimonSaysAmount = reader.GetInt32(9);
            }
            else
            {
                item.SimonSaysAmount = 0;
            }

            allSelectedActivityData.Add(item);
        }

        return allSelectedActivityData;
    }
}
public class GameResult
{
    public int RobotId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? KindOfGame { get; set; }
    public decimal AverageReactionTimeMs { get; set; }
    public decimal CorrectlyAnsweredPercentage { get; set; }
    public string? InteractionState { get; set; }
    public int? SimonSaysAmount { get; set; }
}
public class QuestionAnswers
{
    public string? Question { get; set; }
    public string? CorrectAnswer { get; set; }
    public string? WrongAnswer { get; set; }
    public string? WrongAnswer2 { get; set; }
}