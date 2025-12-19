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
            command.Parameters.Add("@Datum", SqlDbType.DateTime).Value = result.EndTime.Date;
            command.Parameters.Add("@TijdstipStart", SqlDbType.Time).Value = result.StartTime.TimeOfDay;
            command.Parameters.Add("@TijdstipEind", SqlDbType.Time).Value = result.EndTime.TimeOfDay;
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
}
public class GameResult
{
    public int RobotId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? KindOfGame { get; set; }
    public int AverageReactionTimeMs { get; set; }
    public int CorrectlyAnsweredPercentage { get; set; }
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