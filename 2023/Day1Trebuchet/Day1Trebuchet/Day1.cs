using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Day1Trebuchet
{
    public class Line
    {
        private readonly string _text;
        private readonly List<int> _digits;

        public int FirstDigit => _digits.First();
        public int LastDigit => _digits.Last();
        public int FirstAndLastDigitCombined => int.Parse($"{FirstDigit}{LastDigit}");

        public Line(string text)
        {
            _text = text;
            _digits = _text
                .Where(c => int.TryParse(c.ToString(), out var digit))
                .Select(c => int.Parse(c.ToString()))
                .ToList();
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
        public void LineTest()
        {
            var lines = new List<Line>()
            {
                new Line("1abc2"),
                new Line("pqr3stu8vwx"),
                new Line("a1b2c3d4e5f"),
                new Line("treb7uchet")
            };

            lines[0].FirstAndLastDigitCombined.Should().Be(12);
            lines[1].FirstAndLastDigitCombined.Should().Be(38);
            lines[2].FirstAndLastDigitCombined.Should().Be(15);
            lines[3].FirstAndLastDigitCombined.Should().Be(77);
            lines.Select(line => line.FirstAndLastDigitCombined).Sum().Should().Be(142);
        }

        [Fact]
        public void Answer()
        {
            var answer = File.ReadAllLines("input.txt")
                .Select(line => new Line(line).FirstAndLastDigitCombined)
                .Sum();

            _output.WriteLine(answer.ToString());
        }
    }
}