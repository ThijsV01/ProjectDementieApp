public interface ISqlActivitiesRepository
{
    Task<List<QuestionAnswers>> GetQuestions(int robotID);

    void InsertInteraction(GameResult gameResult);
    Task<List<string>> SelectActivities(int robotID);
    Task<List<GameResult>> GetSelectedActivityData(int robotID, string gameKind);
}