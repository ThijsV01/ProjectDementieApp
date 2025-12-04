using SimpleMqtt;
public class MQTTMessageProcessingService : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlBatteryRepository _SQLBatteryRepository;

    public MQTTMessageProcessingService(SimpleMqttClient simpleMqttClient, ISqlBatteryRepository sqlBatteryRepository)
    {
        _simpleMqttClient=simpleMqttClient;
        _SQLBatteryRepository=sqlBatteryRepository;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.OnMessageReceived += (sender, args)=>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            _SQLBatteryRepository.GetBatteryStatusInMillivoltsAsync();
        };
        await _simpleMqttClient.SubscribeToTopic("#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}