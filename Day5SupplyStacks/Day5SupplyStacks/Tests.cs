using System.Net.Http.Headers;
using FluentAssertions;
using Xunit;

namespace Day5SupplyStacks
{
    public class Files
    {
        public const string RealInput = "input.txt";
        public const string TestInput1 = "input.test.txt";
    }

    public class Tests
    {
        [Theory]
        [InlineData(Files.RealInput)]
        [InlineData(Files.TestInput1)]
        public void FileShouldContainLines(string file)
        {
            File.ReadAllLines(file).Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ShouldBuildStackPlatform()
        {
            var sut = new StackPlatformBuilder();

            var lines = File.ReadAllLines(Files.TestInput1);

            var stackPlatform = sut.Build(lines);

            stackPlatform.ToString().Should().Be("NZ|DCM|P");
            stackPlatform.Commands.Should().BeEquivalentTo(new List<Command>
            {
                new(count:1, from: 2, to: 1),
                new(count:3, from: 1, to: 3),
                new(count:2, from: 2, to: 1),
                new(count:1, from: 1, to: 2),
            });
        }

        [Fact]
        public void ShouldMoveCrates()
        {
            var sut = new StackPlatformBuilder();

            var lines = File.ReadAllLines(Files.TestInput1);

            var stackPlatform = sut.Build(lines);

            stackPlatform.ExecuteCommands();
            stackPlatform.ToString().Should().Be("C|M|ZNDP");
            
        }

        [Fact]
        public void GetAnswer()
        {
            var sut = new StackPlatformBuilder();

            var lines = File.ReadAllLines(Files.RealInput);

            var stackPlatform = sut.Build(lines);

            stackPlatform.ExecuteCommands();

            Console.WriteLine($"TopLayer: {stackPlatform.TopLayer}");
        }

    }

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

    public class StackPlatform
    {
        public List<CrateStack> Stacks { get; }
        public List<Command> Commands { get; set; }

        public StackPlatform()
        {
            Stacks = new List<CrateStack>();
        }

        public string TopLayer
        {
            get => String.Join("", Stacks.Select(s => s.Crates.Peek().Id));
        }

        public override string ToString()
        {
            return String.Join("|", Stacks.Select(s => s.ToString()));
        }

        public void AddCrate(int stackIndex, char id)
        {
            Stacks[stackIndex].Crates.Push(new Crate(id));
        }

        public void ExecuteCommands()
        {
            foreach (var command in Commands)
            {
                for (int i = 0; i < command.Count; i++)
                {
                    Console.WriteLine(command);

                    var crate = Stacks[command.From-1].Crates.Pop();
                    Stacks[command.To-1].Crates.Push(crate);
                }
            }
        }
    }

    public class Command
    {
        public int From { get; }
        public int To { get; }
        public int Count { get; }
        public Command(int from, int to, int count)
        {
            From = from;
            To = to;
            Count = count;
        }

        public override string ToString()
        {
            return $"move {Count} from {From} to {To}";
        }
    }

    public class CrateStack
    {
        public Stack<Crate> Crates { get; }

        public CrateStack()
        {
            Crates = new Stack<Crate>();
        }
        public override string ToString()
        {
            return String.Join("", Crates.Select(c => c.Id));
        }
    }

    public class Crate
    {
        public char Id { get; }

        public Crate(char id)
        {
            Id = id;
        }
    }
}