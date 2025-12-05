using SimpleMqtt;
public class MQTTMessageProcessingService : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlBatteryRepository _SQLBatteryRepository;

    public MQTTMessageProcessingService(SimpleMqttClient simpleMqttClient, ISqlBatteryRepository sqlBatteryRepository)
    {
        _simpleMqttClient=simpleMqttClient;
        _SQLBatteryRepository=sqlBatteryRepository;

        _simpleMqttClient.OnMessageReceived += (sender, args)=>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            if (args.Topic == "robot/2242722/battery")
            {
                if (int.TryParse(args.Message, out var batteryValue))
                {
                    int robotId=1;
                    _SQLBatteryRepository.InsertBatteryLevel(batteryValue, robotId);
                }
            }
        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        
        await _simpleMqttClient.SubscribeToTopic("robot/2242722/#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}