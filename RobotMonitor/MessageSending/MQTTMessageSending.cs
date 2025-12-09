using SimpleMqtt;

public class MQTTMessageSending : BackgroundService
{
    private readonly SimpleMqttClient _simpleMqttClient;
    private readonly ISqlInteractionMomentsRepository _sqlInteractionMomentsRepository;
    private readonly List<TimeSpan> completedTimes = [];

    public MQTTMessageSending(SimpleMqttClient simpleMqttClient, ISqlInteractionMomentsRepository sqlInteractionMomentsRepository)
    {
        _simpleMqttClient = simpleMqttClient;
        _sqlInteractionMomentsRepository = sqlInteractionMomentsRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var times = await _sqlInteractionMomentsRepository.SelectInteractionMoments();
            var now = DateTime.Now.TimeOfDay;

            foreach (var time in times)
            {
                if (now >= time && !completedTimes.Contains(time))
                {
                    await _simpleMqttClient.PublishMessage(time.ToString(), "robot/2242722/interactionmoment");
                    completedTimes.Add(time);
                }
            }

            if (now.TotalMinutes < 1)
            {
                completedTimes.Clear();
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}