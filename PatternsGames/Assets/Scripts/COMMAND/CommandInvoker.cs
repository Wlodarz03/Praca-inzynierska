public static class CommandInvoker
{
    private static CommandHistory history = new CommandHistory(20);

    public static void ExecuteCommand(ICommand command)
    {
        bool changed = command.Execute();
        if (changed)
        {
            history.Add(command);
        }
    }

    public static void UndoCommand()
    {
        history.Undo();
    }

    public static void RedoCommand()
    {
        history.Redo();
    }
    public static void ClearHistory()
    {
        history.ClearHistory();
    }
    public static CommandHistory GetHistory()
    {
        return history;
    }
}
