public interface ISqlBatteryRepository
{
    Task<int> GetBatteryStatusInMillivoltsAsync(int robotID);
    void InsertBatteryLevel(int batteryValue, int robotId);
}
