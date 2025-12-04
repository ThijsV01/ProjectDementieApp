public interface ISqlBatteryRepository
{
    Task<int> GetBatteryStatusInMillivoltsAsync();
}
