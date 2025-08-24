using System.Collections.Generic;
public class CommandHandler
{
    private readonly Stack<ICommand> _commands;
    public int Count => _commands.Count;
    public CommandHandler()
    {
        _commands = new Stack<ICommand>();
    }
    public bool HasCommands()
    {
        return _commands.Count > 0;
    }
    public void Add(ICommand command)
    {
        if (command == null)
            return;

        _commands.Push(command);
    }
    public void Undo()
    {
        if (_commands.Count > 0)
        {
            _commands.Pop().Undo();
        }
    }
    public void Clear()
    {
        _commands.Clear();
    }
}