using FluentAssertions;
using Xunit;

namespace Day7NoSpaceLeftOnDevice
{
    public class Files
    {
        public const string InputFile = "input.txt";
        public const string InputFileTest = "input.test.txt";
    }
    
    // https://adventofcode.com/2022/day/7
    // build filestructure hierarchy
    // calc direct folder size
    // add recursion to include child folder sizes in calculation
    // filter where dir.size < 100000
    // sum of the above list of filtered list
    public class Tests
    {
        [Theory]
        [InlineData(Files.InputFile)]
        [InlineData(Files.InputFileTest)]
        public void ShouldReadFile(string fileName)
        {
            File.ReadAllLines(fileName).Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void SolutionPart1()
        {
            var lines = File.ReadAllLines(Files.InputFile);

            var sut = new FileSystemParser();

            var fileSystem = sut.Parse(lines);

            var directories = fileSystem.AllDirectories.Where(d => d.Size <= 100000).ToList();

            var solution = directories.Sum(d => d.Size);
            
            // Your puzzle answer was 1908462.
            solution.Should().Be(1908462); // retrospectively added as a regression test

            Console.WriteLine(solution);
        }

        [Fact]
        public void SolutionPart2()
        {
            var lines = File.ReadAllLines(Files.InputFile);

            var sut = new FileSystemParser();

            var fileSystem = sut.Parse(lines);

            var capacity = 70000000;
            var usedSpace = fileSystem.Root.Size;
            var availableSpace = capacity - usedSpace;
            var requiredFreeSpace = 30000000;
            var requiredSpaceToBeFreed = requiredFreeSpace - availableSpace;
            
            Console.WriteLine(fileSystem.Root.Size + " (current)"); 
            Console.WriteLine(requiredFreeSpace + " (requiredFreeSpace)");
            Console.WriteLine(requiredSpaceToBeFreed + " (requiredSpaceToBeFreed)");

            var candidates = fileSystem.AllDirectories.Where(d => d.Size >= requiredSpaceToBeFreed).ToList();
            
            var solution = candidates.Min(d => d.Size);
            
            Console.WriteLine(solution);
        }

        [Fact]
        public void ShouldParseFileSystem()
        {
            var lines = File.ReadAllLines(Files.InputFileTest);

            var sut = new FileSystemParser();

            var fileSystem = sut.Parse(lines);
            
            // file sizes
            fileSystem.Root.Directories["a"].Directories["e"].Files["i"].Size.Should().Be(584);
            fileSystem.Root.Directories["a"].Files["f"].Size.Should().Be(29116);
            fileSystem.Root.Directories["a"].Files["g"].Size.Should().Be(2557);
            fileSystem.Root.Directories["a"].Files["h.lst"].Size.Should().Be(62596);

            fileSystem.Root.Files["b.txt"].Size.Should().Be(14848514);

            fileSystem.Root.Files["c.dat"].Size.Should().Be(8504156);

            fileSystem.Root.Directories["d"].Files["j"].Size.Should().Be(4060174);
            fileSystem.Root.Directories["d"].Files["d.log"].Size.Should().Be(8033020);
            fileSystem.Root.Directories["d"].Files["d.ext"].Size.Should().Be(5626152);
            fileSystem.Root.Directories["d"].Files["k"].Size.Should().Be(7214296);

            // dir sizes
            fileSystem.Root.Directories["a"].Directories["e"].Size.Should().Be(584);
            fileSystem.Root.Directories["a"].Size.Should().Be(94853);
            fileSystem.Root.Directories["d"].Size.Should().Be(24933642);
            fileSystem.Root.Size.Should().Be(48381165);

            fileSystem.AllDirectories.Count.Should().Be(4);
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "/").Should().NotBeNull();
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "a").Should().NotBeNull();
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "e").Should().NotBeNull();
            fileSystem.AllDirectories.SingleOrDefault(d => d.Name == "d").Should().NotBeNull();

        }
    }
}