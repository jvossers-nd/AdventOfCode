using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1;

public class Tests
{
    private readonly ITestOutputHelper _output;

    public Tests(ITestOutputHelper output)
    {
        _output = output;
    }
    

    [Fact]
    public void Test2Steps()
    {
        var solution = new Solution(File.ReadAllLines("input.2steps.test.txt").Select(line => new Line(line)).ToList());

        solution.StepPattern.Count.Should().Be(2);
        solution.StepPattern[0].Direction.Should().Be(Direction.Right);
        solution.StepPattern[1].Direction.Should().Be(Direction.Left);

        solution.Network["AAA"][Direction.Left].Id.Should().Be("BBB");
        solution.Network["AAA"][Direction.Right].Id.Should().Be("CCC");

        solution.Network["BBB"][Direction.Left].Id.Should().Be("DDD");
        solution.Network["BBB"][Direction.Right].Id.Should().Be("EEE");

        solution.Network["CCC"][Direction.Left].Id.Should().Be("ZZZ");
        solution.Network["CCC"][Direction.Right].Id.Should().Be("GGG");

        solution.Network["DDD"][Direction.Left].Id.Should().Be("DDD");
        solution.Network["DDD"][Direction.Right].Id.Should().Be("DDD");

        solution.Network["EEE"][Direction.Left].Id.Should().Be("EEE");
        solution.Network["EEE"][Direction.Right].Id.Should().Be("EEE");

        solution.Network["GGG"][Direction.Left].Id.Should().Be("GGG");
        solution.Network["GGG"][Direction.Right].Id.Should().Be("GGG");

        solution.Network["ZZZ"][Direction.Left].Id.Should().Be("ZZZ");
        solution.Network["ZZZ"][Direction.Right].Id.Should().Be("ZZZ");

        solution.Network["AAA"][Direction.Right][Direction.Left].Id.Should().Be("ZZZ");

        solution.Solve().Should().Be(2);
    }

    [Fact]
    public void Test6Steps()
    {
        var solution = new Solution(File.ReadAllLines("input.6steps.test.txt").Select(line => new Line(line)).ToList());

        solution.StepPattern.Count.Should().Be(3);
        solution.StepPattern[0].Direction.Should().Be(Direction.Left);
        solution.StepPattern[1].Direction.Should().Be(Direction.Left);
        solution.StepPattern[2].Direction.Should().Be(Direction.Right);

        solution.Network["AAA"][Direction.Left].Id.Should().Be("BBB");
        solution.Network["AAA"][Direction.Right].Id.Should().Be("BBB");

        solution.Network["BBB"][Direction.Left].Id.Should().Be("AAA");
        solution.Network["BBB"][Direction.Right].Id.Should().Be("ZZZ");

        solution.Network["ZZZ"][Direction.Left].Id.Should().Be("ZZZ");
        solution.Network["ZZZ"][Direction.Right].Id.Should().Be("ZZZ");

        solution.Network["AAA"]
            [Direction.Left]
            [Direction.Left]
            [Direction.Right]
            [Direction.Left]
            [Direction.Left]
            [Direction.Right]
            .Id.Should().Be("ZZZ");

        solution.Solve().Should().Be(6);
    }

    [Fact]
    public void CanReadAllLines()
    {
        File.ReadAllLines("input.txt").Select(line => new Line(line)).Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public void Answer()
    {
        var solution = new Solution(File.ReadAllLines("input.txt").Select(line => new Line(line)).ToList());

        _output.WriteLine(solution.Solve().ToString()); // 13207
    }
}