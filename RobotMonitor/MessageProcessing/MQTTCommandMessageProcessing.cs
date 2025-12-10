using SimpleMqtt;
public class MQTTCommandMessageProcessing : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlCommandoRepository _SQLCommandoRepository;

    public MQTTCommandMessageProcessing(SimpleMqttClient simpleMqttClient, ISqlCommandoRepository sqlCommandoRepository)
    {
        _simpleMqttClient = simpleMqttClient;
        _SQLCommandoRepository = sqlCommandoRepository;

        _simpleMqttClient.OnMessageReceived += (sender, args) =>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            if (args.Topic == "robot/2242722/command/start")
            {
                _SQLCommandoRepository.InsertCommand("Robot has been started. It will drive at the interactiontimes.", int.Parse(args.Message!));
            }
            if (args.Topic == "robot/2242722/command/stop")
            {
                _SQLCommandoRepository.InsertCommand("Robot has been stopped. Click start if it should drive at the interactiontimes.", int.Parse(args.Message!));
            }
        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        await _simpleMqttClient.SubscribeToTopic("robot/2242722/command/#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}