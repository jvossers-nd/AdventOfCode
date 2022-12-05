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

            stackPlatform.ToString().Should().Be("N,Z|D,C,M|P");
        }
    }

    public class StackPlatformBuilder
    {
        public StackPlatform Build(string[] lines)
        {
            var p = new StackPlatform();

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

            return p;
        }
    }

    public class StackPlatform
    {
        public List<CrateStack> Stacks { get; }

        public StackPlatform()
        {
            Stacks = new List<CrateStack>();
        }

        public override string ToString()
        {
            return String.Join("|", Stacks.Select(s => s.ToString()));
        }

        public void AddCrate(int stackIndex, char id)
        {
            Stacks[stackIndex].Crates.Push(new Crate(id));
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
            return String.Join(",", Crates.Select(c => c.Id));
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