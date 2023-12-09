using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Part2;

public class Tests
{
    private readonly ITestOutputHelper _output;

    public Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test()
    {
        var solution = new Solution(File.ReadAllLines("input.test.txt").Select(line => new Line(line)).ToList());

        var answer = solution.Solve();
        solution.Navigators.Count.Should().Be(2);
        solution.Navigators[0].LoopSize.Should().Be(2);
        solution.Navigators[1].LoopSize.Should().Be(3);

        answer.Should().Be(6);
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

        var answer = solution.Solve();

        solution.Navigators.Count.Should().Be(6);

        foreach (var nav in solution.Navigators)
        {
            _output.WriteLine(nav.StartNode.Id + ": " + nav.LoopSize);
        }

        _output.WriteLine("================");
        _output.WriteLine(answer.ToString()); 
        // 1233611912 too low
        // 1233611913 too low
        // 12324145107121 correct
    }
}