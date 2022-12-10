namespace Day9RopeBridge;

public class Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public Position Offset(int byX, int byY)
    {
        return new Position(X+byX, Y+byY);
    }

    public int DistanceX(Position otherPosition) => Math.Abs(X - otherPosition.X);

    public int DistanceY(Position otherPosition) => Math.Abs(Y - otherPosition.Y);

    public int DistanceTotal(Position otherPosition) => DistanceX(otherPosition) + DistanceY(otherPosition);

    public bool InRange(Position otherPosition)
    {
        int deltaX = Math.Abs(X - otherPosition.X);
        int deltaY = Math.Abs(Y - otherPosition.Y);

        return otherPosition.DistanceX(this) < 2 &&
               otherPosition.DistanceY(this) < 2;
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}