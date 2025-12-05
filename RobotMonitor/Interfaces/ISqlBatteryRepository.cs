public interface ISqlBatteryRepository
{
    Task<int> GetBatteryStatusInMillivoltsAsync();
    void InsertBatteryLevel(int batteryValue, int robotId);
}
