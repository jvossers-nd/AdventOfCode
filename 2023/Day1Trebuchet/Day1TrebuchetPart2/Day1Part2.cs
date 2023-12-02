using System.Text.RegularExpressions;
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
            _digits = ExtractDigits(_text).ToList();
        }

        private IEnumerable<int> ExtractDigits(string text)
        {
            string pattern = "(?=(one|two|three|four|five|six|seven|eight|nine|1|2|3|4|5|6|7|8|9)).";
            
            var matches = Regex.Matches(text, pattern);

            foreach (Match match in matches)
            {
                string value = match.Groups[1].Value;

                if (value == "one" || value == "1") yield return 1;
                else if (value == "two" || value == "2") yield return 2;
                else if (value == "three" || value == "3") yield return 3;
                else if (value == "four" || value == "4") yield return 4;
                else if (value == "five" || value == "5") yield return 5;
                else if (value == "six" || value == "6") yield return 6;
                else if (value == "seven" || value == "7") yield return 7;
                else if (value == "eight" || value == "8") yield return 8;
                else if (value == "nine" || value == "9") yield return 9;
                else throw new Exception("oops " + value);
            }
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
        public void OverlappingLineTest()
        {
            var line = new Line("blaeighthree");

            line.FirstAndLastDigitCombined.Should().Be(83);
        }

        [Fact]
        public void LineTest()
        {
            var lines = new List<Line>()
            {
                new Line("two1nine"),
                new Line("eightwothree"),
                new Line("abcone2threexyz"),
                new Line("xtwone3four"),
                new Line("4nineeightseven2"),
                new Line("zoneight234"),
                new Line("7pqrstsixteen")
            };

            lines[0].FirstAndLastDigitCombined.Should().Be(29);
            lines[1].FirstAndLastDigitCombined.Should().Be(83);
            lines[2].FirstAndLastDigitCombined.Should().Be(13);
            lines[3].FirstAndLastDigitCombined.Should().Be(24);
            lines[4].FirstAndLastDigitCombined.Should().Be(42);
            lines[5].FirstAndLastDigitCombined.Should().Be(14);
            lines[6].FirstAndLastDigitCombined.Should().Be(76);
            lines.Select(line => line.FirstAndLastDigitCombined).Sum().Should().Be(281);
        }

        [Fact]
        public void Answer()
        {
            var answer = File.ReadAllLines("input.txt")
                .Select(line => new Line(line).FirstAndLastDigitCombined)
                .Sum();

            _output.WriteLine(answer.ToString());

            // 54868 too low
            // 54875
        }
    }
}