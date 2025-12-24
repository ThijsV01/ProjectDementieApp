public interface ISqlActivitiesRepository
{
    Task<List<QuestionAnswers>> GetQuestions(int robotID);

    void InsertInteraction(GameResult gameResult);
    Task<List<string>> SelectActivities(int robotID);
    Task<List<GameResult>> GetSelectedActivityData(int robotID, string gameKind);
    void DeleteQuestion(int questionId);
    void AddQuestion(QuestionAnswers questionAnswers);
}