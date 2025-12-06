using SimpleMqtt;
public class MQTTBatteryMessageProcessing : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlBatteryRepository _SQLBatteryRepository;

    public MQTTBatteryMessageProcessing(SimpleMqttClient simpleMqttClient, ISqlBatteryRepository sqlBatteryRepository)
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
                    int maxValue = 8600;
                    batteryValue = (int)((double)batteryValue / maxValue * 100);
                    Console.WriteLine(batteryValue);
                    _SQLBatteryRepository.InsertBatteryLevel(batteryValue, robotId);
                }
            }
        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        
        await _simpleMqttClient.SubscribeToTopic("robot/2242722/battery");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}