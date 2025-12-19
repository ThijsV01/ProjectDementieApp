public interface ISqlActivitiesRepository
{
    Task<List<QuestionAnswers>> GetQuestions(int robotID);

    void InsertInteraction(GameResult gameResult);
}