using SimpleMqtt;
public class MQTTSensorMessageProcessing : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlSensorRepository _SQLSensorRepository;

    public MQTTSensorMessageProcessing(SimpleMqttClient simpleMqttClient, ISqlSensorRepository sqlSensorRepository)
    {
        _simpleMqttClient=simpleMqttClient;
        _SQLSensorRepository=sqlSensorRepository;

        _simpleMqttClient.OnMessageReceived += (sender, args)=>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            if (args.Topic == "robot/2242722/sensor/obstacledistance")
            {
                string sensorType="Ultrasonic Distance";
                if (int.TryParse(args.Message, out var sensorValue))
                {
                    _SQLSensorRepository.InsertSensorData(sensorValue,sensorType);
                }
            }
            if (args.Topic == "robot/2242722/sensor/humandetected")
            {
                
                string sensorType="PIR Motion";
                if (int.TryParse(args.Message, out var sensorValue))
                {
                    _SQLSensorRepository.InsertSensorData(sensorValue,sensorType);
                }
            }
        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        
        await _simpleMqttClient.SubscribeToTopic("robot/2242722/sensor/#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}