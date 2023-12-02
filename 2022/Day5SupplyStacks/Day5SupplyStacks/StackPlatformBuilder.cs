namespace Day5SupplyStacks;

public class StackPlatformBuilder
{
    public StackPlatform Build(string[] lines)
    {
        var p = new StackPlatform();

        // stacks
        var stackCount = (lines.First().Length + 1) / 4;

        Enumerable.Range(0, stackCount).ToList().ForEach(i =>  p.Stacks.Add(new CrateStack()));

        var stackLinesReversed = lines.Where(line => line.Contains("[")).Reverse();
           
        foreach (string line in stackLinesReversed)
        {
            for (int i = 0; i < stackCount; i++)
            {
                int startIndex = (i * 4) + 1;
                char c = line.Substring(startIndex, 1).Single();

                if(c != ' ')
                    p.AddCrate(i, c);
            }

            Console.WriteLine();
        }

        p.Commands = lines.Where(line => line.Contains("move")).Select(line =>
        {
            var csv =line
                .Replace("move ", "")
                .Replace(" from ", ",")
                .Replace(" to ", ",");

            var parts = csv.Split(',');

            return new Command(
                count: int.Parse(parts[0]),
                from: int.Parse(parts[1]),
                to: int.Parse(parts[2]));

        }).ToList();
            
        return p;
    }
}