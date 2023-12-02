using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1
{
    public class Line
    {
        private readonly string _text;

        public Line(string text)
        {
            _text = text;
        }
    }

    public class Solution
    {
        public readonly List<Line> _lines;

        public Solution(List<Line> lines)
        {
            _lines = lines;
        }

        public string Solve()
        {
            return string.Empty;
        }
    }

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

            _output.WriteLine(solution.Solve());
        }
    }
}