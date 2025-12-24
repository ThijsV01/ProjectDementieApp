using System.Text.Json;
using SimpleMqtt;
public class MQTTActivitiesMessageProcessing : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlActivitiesRepository _SQLActivitiesRepository;

    public MQTTActivitiesMessageProcessing(SimpleMqttClient simpleMqttClient, ISqlActivitiesRepository sqlActivitiesRepository)
    {
        _simpleMqttClient = simpleMqttClient;
        _SQLActivitiesRepository = sqlActivitiesRepository;

        _simpleMqttClient.OnMessageReceived += async (sender, args) =>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            if (args.Topic == "robot/2242722/activities/quizstarted")
            {
                if (int.TryParse(args.Message, out var robotId))
                {
                    List<QuestionAnswers> questionsAnswers = await _SQLActivitiesRepository.GetQuestions(robotId);
                    var list = JsonSerializer.Serialize(questionsAnswers);
                    await _simpleMqttClient.PublishMessage(list, "robot/2242722/activities/quizquestions");
                }
            }
            if (args.Topic == "robot/2242722/activities/endofinteraction")
            {
                var interactionResult = JsonSerializer.Deserialize<GameResult>(args.Message!);
                _SQLActivitiesRepository.InsertInteraction(interactionResult!);

            }
            if (args.Topic == "robot/2242722/activities/quizquestion/add")
            {
                var quizQuestion = JsonSerializer.Deserialize<QuestionAnswers>(args.Message!);
                _SQLActivitiesRepository.AddQuestion(quizQuestion!);

            }
            if (args.Topic == "robot/2242722/activities/quizquestion/delete")
            {
                if (int.TryParse(args.Message, out var quizQuestionId))
                {
                _SQLActivitiesRepository.DeleteQuestion(quizQuestionId!);
                }

            }

        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        await _simpleMqttClient.SubscribeToTopic("robot/2242722/activities/#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}