using System.Text.Json;
using SimpleMqtt;
public class MQTTRobotMessageProcessing : IHostedService
{
    private SimpleMqttClient _simpleMqttClient;
    private ISqlRobotRepository _SQLRobotRepository;

    public MQTTRobotMessageProcessing(SimpleMqttClient simpleMqttClient, ISqlRobotRepository sqlRobotRepository)
    {
        _simpleMqttClient = simpleMqttClient;
        _SQLRobotRepository = sqlRobotRepository;

        _simpleMqttClient.OnMessageReceived += (sender, args) =>
        {
            Console.WriteLine($"Topic: {args.Topic} Message: {args.Message}");
            if (args.Topic == "robot/2242722/robot/edit")
            {
                var robot = JsonSerializer.Deserialize<Robot>(args.Message!);
                if (robot!=null)
                {
                    _SQLRobotRepository.UpdateRobot(robot.robotID,robot.Naam!,robot.Model!);
                }
            }
        };
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        await _simpleMqttClient.SubscribeToTopic("robot/2242722/robot/#");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _simpleMqttClient.Dispose();
        return Task.CompletedTask;
    }
}