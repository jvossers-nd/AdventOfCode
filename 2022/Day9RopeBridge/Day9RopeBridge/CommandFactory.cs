namespace Day9RopeBridge;

public class CommandFactory
{
    public List<Command> Create(IEnumerable<string> commandStrings)
    {
        return commandStrings.Select((s) =>
        {
            var parts = s.Split(' ');

            return new Command()
            {
                Direction = parts[0] switch
                {
                    "U" => Direction.Up,
                    "R" => Direction.Right,
                    "D" => Direction.Down,
                    "L" => Direction.Left, 
                    _ => throw new Exception("invalid direction.")
                },
                Steps = int.Parse(parts[1]) 
            };
                
        }).ToList();
    }
}