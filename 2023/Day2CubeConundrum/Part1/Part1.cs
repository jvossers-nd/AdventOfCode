using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1
{
    public class Line
    {
        private readonly string _text;

        private readonly List<Draw> _draws;
        public int GameId { get; }

        public bool IsPossible(int red, int green, int blue)
        {
            return !_draws.Any(d => d.Red > red || d.Green > green || d.Blue > blue);
        }

        public Line(string text)
        {
            _text = text;
            _draws = ExtractDraws(_text).ToList();
            GameId = ExtractGameId(_text);
        }

        private int ExtractGameId(string text) => int.Parse(text.Split(':')[0].Replace("Game ", string.Empty));

        private IEnumerable<Draw> ExtractDraws(string text)
        {
            //  3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            foreach (var drawText in text.Split(':')[1].Split(';'))
            {
                var draw = new Draw();

                // 1 red, 2 green, 6 blue
                foreach (var colourText in drawText.Split(','))
                {
                    int count = int.Parse(colourText.Trim().Split(' ')[0]);

                    if (colourText.Contains("red"))
                        draw.Red = count;
                    if (colourText.Contains("green"))
                        draw.Green = count;
                    if (colourText.Contains("blue"))
                        draw.Blue = count;
                }

                yield return draw;
            }
        }
    }

    public class Draw
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }

    public class Solution
    {
        public readonly List<Line> _lines;

        public Solution(List<Line> lines)
        {
            _lines = lines;
        }

        public int Solve(int red, int green, int blue)
        {
            return _lines
                .Where(line => line.IsPossible(red, green, blue))
                .Select(line => line.GameId)
                .Sum();
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
            var solution = new Solution(
                new List<Line>()
                {
                    new Line("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"), // 1 
                    new Line("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue"), // 2
                    new Line("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red"),
                    new Line("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red"),
                    new Line("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green") // 5
                });

            solution.Solve(12, 13, 14).Should().Be(8);
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

            _output.WriteLine(solution.Solve(12, 13, 14).ToString());
        }
    }
}