public class RandomRobotMonitorService : IRobotMonitorService
    {
        private readonly Random _random = new();

        public Task<int> GetBatteryStatusInMillivoltsAsync()
        {
            // Simuleer een batterijwaarde tussen 9.000 mV en 12.600 mV
            int randomValue = _random.Next(9000, 12600);
            return Task.FromResult(randomValue);
        }
}