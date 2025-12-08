public interface ISqlCommandoRepository
{
    Task<List<CommandItem>> GetCommands();
    void InsertCommand(string commandUsed, int robotId);
}