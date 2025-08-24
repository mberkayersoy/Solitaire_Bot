public interface ICommand
{
    void Apply();
    void Undo();
    MoveType MoveType { get; }
}
