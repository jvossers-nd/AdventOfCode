namespace Day9RopeBridge;

public class Visit
{
    public Position Position { get; }

    public Visit(int x, int y)
    {
        Position = new Position(x, y);
    }

    public Visit(Position position)
    {
        Position = position;
    }
}