namespace Day11MonkeyInTheMiddle;

public class Parser
{
    public List<Monkey> Parse(IEnumerable<string> lines)
    {
        var monkeys = new List<Monkey>();

        var monkeyChunks = lines.Chunk(7);

        foreach (var monkeyChunk in monkeyChunks)
        {
            var items = monkeyChunk[1]
                .Replace("  Starting items: ", string.Empty)
                .Split(", ")
                .Select(item => long.Parse(item))
                .ToList();

            var operation = monkeyChunk[2]
                .Replace("  Operation: new = ", string.Empty);

            var divisibleBy = int.Parse(monkeyChunk[3].Replace("  Test: divisible by ", string.Empty));

            var trueMonkeyIndex = int.Parse(monkeyChunk[4].Replace("    If true: throw to monkey ", string.Empty));

            var falseMonkeyIndex = int.Parse(monkeyChunk[5].Replace("    If false: throw to monkey ", string.Empty));

            var monkey = new Monkey(operation, divisibleBy, trueMonkeyIndex, falseMonkeyIndex, items);

            monkeys.Add(monkey);
        }

        return monkeys;
    }
}