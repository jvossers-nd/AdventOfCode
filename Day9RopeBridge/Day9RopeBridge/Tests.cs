using FluentAssertions;
using Xunit;

namespace Day9RopeBridge
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
        public void ShouldCreateCommands()
        {
            var lines = File.ReadAllLines("input.test.txt");

            var sut = new CommandFactory();

            var commands = sut.Create(lines);

            commands.Should().BeEquivalentTo(new List<Command>
            {
                new() {Direction = Direction.Right, Steps = 4},
                new() {Direction = Direction.Up, Steps = 4},
                new() {Direction = Direction.Left, Steps = 3},
                new() {Direction = Direction.Down, Steps = 1},
                new() {Direction = Direction.Right, Steps = 4},
                new() {Direction = Direction.Down, Steps = 1},
                new() {Direction = Direction.Left, Steps = 5},
                new() {Direction = Direction.Right, Steps = 2}
            });
        }

        [Fact]
        public void SolutionPart1()
        {
            var commands = new CommandFactory().Create(File.ReadAllLines("input.txt"));

            var sut = new Rope(2);

            sut.ExecuteCommands(commands);

            Console.WriteLine(sut.Solution); // 
            sut.Solution.Should().Be(6057); // regression test added after finding solution
        }

        [Fact]
        public void SolutionPart2()
        {
            var commands = new CommandFactory().Create(File.ReadAllLines("input.txt"));

            var sut = new Rope(10);

            sut.ExecuteCommands(commands);

            Console.WriteLine(sut.Solution);
        }


        [Fact]
        public void ShouldExecuteCommands()
        {
            var commands = new CommandFactory().Create(File.ReadAllLines("input.test.txt"));

            var sut = new Rope(2);

            sut.ExecuteCommands(commands);

            sut.Knots[0].VisitHistory.Should().BeEquivalentTo(new List<Visit>()
            {
                // starting position
                new Visit(0, 0),
                // R 4
                new Visit(1, 0),
                new Visit(2, 0),
                new Visit(3, 0),
                new Visit(4, 0),
                // U 4,
                new Visit(4, 1),
                new Visit(4, 2),
                new Visit(4, 3),
                new Visit(4, 4),
                // L 3
                new Visit(3, 4),
                new Visit(2, 4),
                new Visit(1, 4),
                // D 1
                new Visit(1, 3),
                // R 4
                new Visit(2, 3),
                new Visit(3, 3),
                new Visit(4, 3),
                new Visit(5, 3),
                // D 1
                new Visit(5, 2),
                // L 5
                new Visit(4, 2),
                new Visit(3, 2),
                new Visit(2, 2),
                new Visit(1, 2),
                new Visit(0, 2),
                // R 2
                new Visit(1, 2),
                new Visit(2, 2)
            });

            sut.Knots[1].VisitHistory.Should().BeEquivalentTo(new List<Visit>()
            {
                // starting position
                new Visit(0, 0),
                // R 4
                new Visit(1, 0),
                new Visit(2, 0),
                new Visit(3, 0),
                // U 4,
                new Visit(4, 1),
                new Visit(4, 2),
                new Visit(4, 3),
                // L 3
                new Visit(3, 4),
                new Visit(2, 4),
                // D 1
                // R 4
                new Visit(3, 3),
                new Visit(4, 3),
                // D 1
                // L 5
                new Visit(3, 2),
                new Visit(2, 2),
                new Visit(1, 2),
                // R 2
            });

            sut.Solution.Should().Be(13);
        }
    }
}