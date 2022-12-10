namespace Day9RopeBridge;

public class Rope
{
    public Rope()
    {
        HeadVisitHistory = new List<Visit>();
        RegisterHeadVisit(new Visit(0, 0));

        TailVisitHistory = new List<Visit>();
        RegisterTailVisit(new Visit(0, 0));
    }

    public Position HeadPosition => HeadVisitHistory.Last().Position;
    public Position TailPosition => TailVisitHistory.Last().Position;

    public int SolutionPart1
    {
        get => TailVisitHistory.Select(v => v.Position.ToString()).Distinct().Count();
    }

    private void RegisterHeadVisit(Visit visit)
    {
        HeadVisitHistory.Add(visit);
    }

    private void RegisterTailVisit(Visit visit)
    {
        TailVisitHistory.Add(visit);
    }

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

                RegisterHeadVisit(new Visit(HeadPosition.Offset(headDeltaX, headDeltaY)));

                if (!TailPosition.InRange(HeadPosition))
                {
                    // head fell out of range
                    // where there to do to? we have 8 options
                    var candidatePositions = new List<Position>
                    {
                        new(TailPosition.X+0, TailPosition.Y+1),
                        new(TailPosition.X+1, TailPosition.Y+1),
                        new(TailPosition.X+1, TailPosition.Y+0),
                        new(TailPosition.X+1, TailPosition.Y-1),
                        new(TailPosition.X+0, TailPosition.Y-1),
                        new(TailPosition.X-1, TailPosition.Y-1),
                        new(TailPosition.X-1, TailPosition.Y+0),
                        new(TailPosition.X-1, TailPosition.Y+1),
                    };

                    var newPosition = candidatePositions
                        .Where(p => HeadPosition.InRange(p))
                        .MinBy(p => HeadPosition.DistanceTotal(p));

                    RegisterTailVisit(new Visit(newPosition));
                }
            }
        }
    }

    public List<Visit> HeadVisitHistory { get; set; }
    public List<Visit> TailVisitHistory { get; set; }


}