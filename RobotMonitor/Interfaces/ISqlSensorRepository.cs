public interface ISqlSensorRepository
{
    Task InsertSensorData(int sensorValue, string sensorType);
    Task<List<SensorMeting>> SelectSensorDataPIRMotion(int robotID);
    Task<List<SensorMeting>> SelectSensorDataObstacleDistance(int robotID);
}