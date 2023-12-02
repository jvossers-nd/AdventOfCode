namespace Day11MonkeyInTheMiddle;

public class Game
{
    public readonly List<Monkey> Monkeys;

    public Game(List<Monkey> monkeys)
    {
        Monkeys = monkeys;
    }

    public async Task PlayRounds(int roundCount, bool reduceWorryLevel = true)
    {
        for (int i = 0; i < roundCount; i++)
        {
            Console.WriteLine($"===== Round {i+1} =====");

            int monkeyIndex = 0;

            var supermodulo = Monkeys.Select(m => m.DivisibleBy).Aggregate(1, (x,y) => x * y);

            foreach (var monkey in Monkeys)
            {
                if (!monkey.Items.Any())
                {
                    Console.WriteLine($"[{monkeyIndex}] does not have any items.");
                }

                while (monkey.Items.Any())
                {
                    monkey.Inspections.Add(new Inspection());
                    string inspectionsMessage = $"inspection count increased to {monkey.Inspections.Count}";

                    if (reduceWorryLevel)
                    {
                        await monkey.ApplyOperation();
                    }
                    else
                    {
                        await monkey.ApplyOperationSupermodulo(supermodulo); // (23*19*13*17);
                    }

                    if (monkey.Items[0] % monkey.DivisibleBy == 0)
                    {
                        Console.WriteLine($"[{monkeyIndex}] passing new value {monkey.Items[0]} to [{monkey.TrueMonkeyIndex}] because {monkey.Items[0]} is divisible by {monkey.DivisibleBy} ({inspectionsMessage}).");
                        Monkeys[monkey.TrueMonkeyIndex].Items.Add(monkey.Items[0]);
                    }
                    else
                    {
                        Console.WriteLine($"[{monkeyIndex}] passing new value {monkey.Items[0]} to [{monkey.TrueMonkeyIndex}] because {monkey.Items[0]} is NOT divisible by {monkey.DivisibleBy} ({inspectionsMessage}).");
                        Monkeys[monkey.FalseMonkeyIndex].Items.Add(monkey.Items[0]);
                    }

                    monkey.Items.RemoveAt(0);
                }

                Console.WriteLine();
                monkeyIndex++;
            }
        }
    }
}