using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1
{
    public class Line
    {
        private readonly string _text;

        public List<Number> MyNumbers { get; set; }
        public List<Number> WinningNumbers { get; set; }

        public int GetPoints()
        {
            var MyWinningNumbers = MyNumbers.Select(n => n.Value).Intersect(WinningNumbers.Select(n => n.Value)).ToList();

            int points = (int)Math.Pow((double)2, (double)MyWinningNumbers.Count-1);

            return points;
        }

        public Line(string text)
        {
            _text = text;
            ParseNumbers();
        }

        private void ParseNumbers()
        {
            var numberSets = _text.Replace("  ", " ").Split(':')[1].Split('|');
            WinningNumbers = numberSets[0].Trim().Split(' ').Select(n => new Number(int.Parse(n))).ToList();
            MyNumbers = numberSets[1].Trim().Split(' ').Select(n => new Number(int.Parse(n))).ToList();
        }
    }

    public class Number
    {
        public Number(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            return Value.Equals(((Number)obj).Value);
        }
    }

    public class Solution
    {
        public readonly List<Line> _lines;

        public Solution(List<Line> lines)
        {
            _lines = lines;
        }

        public int Solve()
        {
            return _lines.Sum(line => line.GetPoints());
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
                    new Line("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53"),
                    new Line("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19"),
                    new Line("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1"),
                    new Line("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83"),
                    new Line("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36"),
                    new Line("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"),
                });

            solution.Solve().Should().Be(13);
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

            _output.WriteLine(solution.Solve().ToString()); // 21485
        }
    }
}