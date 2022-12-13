using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Xunit;

namespace Day11MonkeyInTheMiddle
{
    public class Tests
    {
        [Fact]
        public void ShouldReadLines()
        {
            File.ReadAllLines("input.txt").Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldReadTestLines()
        {
            File.ReadAllLines("input.test.txt").Should().NotBeEmpty();
        }

        [Fact]
        public async Task SolutionPart1()
        {
            var lines = File.ReadAllLines("input.txt");
            
            var monkeys = new Parser().Parse(lines);

            var game = new Game(monkeys);

            await game.PlayRounds(20);

            var top2 = game.Monkeys
                .Select(m => m.Inspections.Count)
                .OrderByDescending(count => count)
                .ToList();
            
            Console.WriteLine(top2[0] * top2[1]);
        }

        [Fact]
        public async Task ShouldExecuteOperation()
        {
            var op = "old + 6";
            var opLambda = $"(old) => {op}";

            Func<int, int> func = await CSharpScript.EvaluateAsync<Func<int, int>>(opLambda);

            func(10).Should().Be(16);
        }

        [Fact]
        public async Task ShouldApplyOperation()
        {
            var monkey = new Monkey("old + 6", 19, 2, 0, new List<int>() {10});

            await monkey.ApplyOperation(); // add 6 to 10, divide by 3, round down

            monkey.Items[0].Should().Be(5); 
        }

        [Fact]
        public void ShouldParseMonkeys()
        {
            var lines = File.ReadAllLines("input.test.txt");

            var sut = new Parser();

            var monkeys = sut.Parse(lines);

            monkeys.Should().BeEquivalentTo(new List<Monkey>()
            {
                new Monkey("old * 19", 23, 2,3, new List<int>() { 79, 98 }),
                new Monkey("old + 6", 19, 2,0, new List<int>() { 54, 65, 75, 74 }),
                new Monkey("old * old", 13, 1, 3, new List<int>() { 79, 60, 97 }),
                new Monkey("old + 3", 17, 0,1, new List<int>() { 74 }),
            });
        }

        [Fact]
        public async Task ShouldPlayRounds()
        {
            var lines = File.ReadAllLines("input.test.txt");
            
            var monkeys = new Parser().Parse(lines);

            var game = new Game(monkeys);

            await game.PlayRounds(20);

            game.Monkeys[0].Inspections.Count.Should().Be(101);
            game.Monkeys[1].Inspections.Count.Should().Be(95);
            game.Monkeys[2].Inspections.Count.Should().Be(7);
            game.Monkeys[3].Inspections.Count.Should().Be(105);
        }

        [Fact]
        public async Task ShouldPlayRoundsWithoutReducingWorryLevel()
        {
            var lines = File.ReadAllLines("input.test.txt");
            
            var monkeys = new Parser().Parse(lines);

            var game = new Game(monkeys);

            await game.PlayRounds(20, false);

            game.Monkeys[0].Inspections.Count.Should().Be(99);
            game.Monkeys[1].Inspections.Count.Should().Be(97);
            game.Monkeys[2].Inspections.Count.Should().Be(8);
            game.Monkeys[3].Inspections.Count.Should().Be(103);
        }
    }

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

                        await monkey.ApplyOperation(reduceWorryLevel);

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
}