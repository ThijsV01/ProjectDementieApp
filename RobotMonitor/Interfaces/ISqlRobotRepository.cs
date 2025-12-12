public interface ISqlRobotRepository
{
    Task<List<Robot>> SelectAllRobots();
    void UpdateRobot(int robotId, string name,string model);
}