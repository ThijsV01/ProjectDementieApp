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
                string[] parts = args.Message!.Split(';');

                if (int.TryParse(parts[0], out var robotID))
                {
                    TimeOnly time = TimeOnly.Parse(parts[1]);
                    _SQLInteractionMomentsRepository.AddMoment(robotID,time);
                }
            }
            if (args.Topic == "robot/2242722/interactionmoment/delete")
            {
                if (int.TryParse(args.Message, out var interactionMomentId))
                {
                    _SQLInteractionMomentsRepository.DeleteMoment(interactionMomentId);
                }
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