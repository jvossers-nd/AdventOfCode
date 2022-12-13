using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Xunit;
using Xunit.Abstractions;

namespace Day11MonkeyInTheMiddle
{
    public class Tests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Tests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

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
            
            _testOutputHelper.WriteLine((top2[0] * top2[1]).ToString());
        }

        [Fact]
        public async Task SolutionPart2()
        {
            var lines = File.ReadAllLines("input.txt");

            var monkeys = new Parser().Parse(lines);

            var game = new Game(monkeys);

            await game.PlayRounds(10000, reduceWorryLevel: false);

            var top2 = game.Monkeys
                .Select(m => (long)m.Inspections.Count)
                .OrderByDescending(count => count)
                .ToList();

            _testOutputHelper.WriteLine((top2[0] * top2[1]).ToString());
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
            var monkey = new Monkey("old + 6", 19, 2, 0, new List<long>() {10});

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
                new Monkey("old * 19", 23, 2,3, new List<long>() { 79, 98 }),
                new Monkey("old + 6", 19, 2,0, new List<long>() { 54, 65, 75, 74 }),
                new Monkey("old * old", 13, 1, 3, new List<long>() { 79, 60, 97 }),
                new Monkey("old + 3", 17, 0,1, new List<long>() { 74 }),
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
        public async Task ShouldPlayRoundsWithoutReducingWorryLevel20Rounds()
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

        [Fact]
        public async Task ShouldPlayRoundsWithoutReducingWorryLevel1000rounds()
        {
            var lines = File.ReadAllLines("input.test.txt");

            var monkeys = new Parser().Parse(lines);

            var game = new Game(monkeys);

            await game.PlayRounds(1000, false);

            game.Monkeys[0].Inspections.Count.Should().Be(5204);
            game.Monkeys[1].Inspections.Count.Should().Be(4792);
            game.Monkeys[2].Inspections.Count.Should().Be(199);
            game.Monkeys[3].Inspections.Count.Should().Be(5192);
        }

        [Fact]
        public async Task ShouldPlayRoundsWithoutReducingWorryLevel10000rounds()
        {
            var lines = File.ReadAllLines("input.test.txt");

            var monkeys = new Parser().Parse(lines);

            var game = new Game(monkeys);

            await game.PlayRounds(10000, false);

            game.Monkeys[0].Inspections.Count.Should().Be(52166);
            game.Monkeys[1].Inspections.Count.Should().Be(47830);
            game.Monkeys[2].Inspections.Count.Should().Be(1938);
            game.Monkeys[3].Inspections.Count.Should().Be(52013);
        }
    }
}