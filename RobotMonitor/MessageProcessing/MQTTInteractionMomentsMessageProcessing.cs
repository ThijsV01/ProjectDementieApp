using SimpleMqtt;
public class MQTTInteractionMomentsMessageProcessing : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlInteractionMomentsRepository _SQLInteractionMomentsRepository;

    public MQTTInteractionMomentsMessageProcessing(SimpleMqttClient simpleMqttClient, ISqlInteractionMomentsRepository sqlInteractionMomentsRepository)
    {
        _simpleMqttClient = simpleMqttClient;
        _SQLInteractionMomentsRepository = sqlInteractionMomentsRepository;

        _simpleMqttClient.OnMessageReceived += (sender, args) =>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            if (args.Topic == "robot/2242722/interactionmoment/add")
            {
                // int robotId = 1;
                // _SQLInteractionMomentsRepository.InsertMoment();
            }
            if (args.Topic == "robot/2242722/interactionmoment/delete")
            {
                // int robotId = 1;
                // _SQLInteractionMomentsRepository.DeleteMoment();
            }
            if (args.Topic == "robot/2242722/interactionmoment/edit")
            {
                // int robotId = 1;
                // _SQLInteractionMomentsRepository.EditMoment();
            }
        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        await _simpleMqttClient.SubscribeToTopic("robot/2242722/interactionmoment/#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}