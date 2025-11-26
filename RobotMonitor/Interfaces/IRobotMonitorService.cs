public interface IRobotMonitorService
{
    Task<int> GetBatteryStatusInMillivoltsAsync();
}
