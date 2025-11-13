public class CommandHistory
{
    private ICommand[] buffer;
    private int currentIndex = -1;
    private int start = 0;
    private int count = 0;
    private int maxSize;

    public CommandHistory(int size)
    {
        buffer = new ICommand[size];
        maxSize = size;
    }

    public void Add(ICommand command)
    {
        // jeżeli dodajemy po Undo -> kasujemy przyszłość
        if (currentIndex < count - 1)
        {
            count = currentIndex + 1;
        }

        int writeRel = currentIndex + 1;
        int writeAbs = (start + writeRel) % maxSize;
        buffer[writeAbs] = command;

        if (count < maxSize)
        {
            count = writeRel + 1;
            currentIndex = writeRel;
        }
        else
        {
            start = (start + 1) % maxSize;
            currentIndex = count - 1;
        }
    }

    public bool CanUndo => count > 0 && currentIndex >= 0;
    public bool CanRedo => count > 0 && currentIndex < count - 1;

    public void Undo()
    {
        if (CanUndo)
        {
            int abs = (start + currentIndex) % maxSize;
            buffer[abs].Undo();
            currentIndex--;
        }
    }

    public void Redo()
    {
        if (CanRedo)
        {
            currentIndex++;
            int abs = (start + currentIndex) % maxSize;
            buffer[abs].Execute();
        }
    }

    public void ClearHistory()
    {
        buffer = new ICommand[maxSize];
        count = 0;
        start = 0;
        currentIndex = -1;
    }
}
