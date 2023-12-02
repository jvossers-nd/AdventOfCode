namespace Day9RopeBridge;

public class Knot
{
    public List<Visit> VisitHistory { get; set; }
    public Knot()
    {
        VisitHistory = new List<Visit>();
        RegisterVisit(new Visit(0, 0));
    }

    public Position Position => VisitHistory.Last().Position;

    public void RegisterVisit(Visit visit)
    {
        VisitHistory.Add(visit);
    }
}


public class Rope
{
    public List<Knot> Knots { get; set; }

    public Rope(int knotCount)
    {
        Knots = new List<Knot>();
        Enumerable.Range(1, knotCount).ToList().ForEach(i => Knots.Add(new Knot()));

    }
    
    public int Solution => Knots.Last().VisitHistory.Select(v => v.Position.ToString()).Distinct().Count();

    public void ExecuteCommands(List<Command> commands)
    {
        foreach (var command in commands)
        {
            for (int i = 0; i < command.Steps; i++)
            {
                // head
                int headDeltaX = command.Direction switch
                {
                    Direction.Right => 1,
                    Direction.Left => -1,
                    _ => 0
                };
                int headDeltaY = command.Direction switch
                {
                    Direction.Up => 1,
                    Direction.Down => -1,
                    _ => 0
                };

                if (headDeltaX == 0 && headDeltaY == 0)
                {
                    continue;
                }

                Knots[0].RegisterVisit(new Visit(Knots[0].Position.Offset(headDeltaX, headDeltaY)));

                for (int j = 1; j < Knots.Count; j++)
                {
                    // follow 
                    var tail = Knots[j];
                    var head = Knots[j-1];

                    if (!tail.Position.InRange(head.Position))
                    {
                        // head fell out of range
                        // where there to do to? we have 8 options

                        var candidatePositions = new List<Position>
                        {
                            new(tail.Position.X+0, tail.Position.Y+1),
                            new(tail.Position.X+1, tail.Position.Y+1),
                            new(tail.Position.X+1, tail.Position.Y+0),
                            new(tail.Position.X+1, tail.Position.Y-1),
                            new(tail.Position.X+0, tail.Position.Y-1),
                            new(tail.Position.X-1, tail.Position.Y-1),
                            new(tail.Position.X-1, tail.Position.Y+0),
                            new(tail.Position.X-1, tail.Position.Y+1),
                        };

                        var newPosition = candidatePositions
                            .Where(p => head.Position.InRange(p))
                            .MinBy(p => head.Position.DistanceTotal(p));

                        tail.RegisterVisit(new Visit(newPosition));
                    }
                }
            }
        }
    }
}