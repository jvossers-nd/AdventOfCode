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
        public void ShouldMoveCratesMultiCrateMode()
        {
            var sut = new StackPlatformBuilder();

            var lines = File.ReadAllLines(Files.TestInput1);

            var stackPlatform = sut.Build(lines);

            stackPlatform.ExecuteCommands(multiCrateMode: true);
            stackPlatform.ToString().Should().Be("M|C|DNZP");
        }

        [Fact]
        public void GetAnswerPart1()
        {
            var sut = new StackPlatformBuilder();

            var lines = File.ReadAllLines(Files.RealInput);

            var stackPlatform = sut.Build(lines);

            stackPlatform.ExecuteCommands();

            Console.WriteLine($"TopLayer: {stackPlatform.TopLayer}");
        }

        [Fact]
        public void GetAnswerPart2()
        {
            var sut = new StackPlatformBuilder();

            var lines = File.ReadAllLines(Files.RealInput);

            var stackPlatform = sut.Build(lines);

            stackPlatform.ExecuteCommands(multiCrateMode: true);

            Console.WriteLine($"TopLayer: {stackPlatform.TopLayer}");
        }
    }
}