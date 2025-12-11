public interface ISqlCommandoRepository
{
    Task<List<CommandItem>> GetCommands(int robotID);
    void InsertCommand(string commandUsed, int robotId);
}