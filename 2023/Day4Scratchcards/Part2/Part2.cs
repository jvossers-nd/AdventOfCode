using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part2
{
    public class Line
    {
        private readonly string _text;
        
        public HashSet<int> WinningNumbersSet { get; set; }
        public List<Number> MyNumbers { get; set; }
        public List<Number> WinningNumbers { get; set; }
        public List<int> MyWinningNumbers { get; set; }

        public int CardNumber { get; set; }

        public int GetPoints()
        {
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
            var colonSplit = _text.Replace("  ", " ").Split(':');
            CardNumber = int.Parse(colonSplit[0].Replace("Card ", ""));

            var numberSets = colonSplit[1].Split('|');
            WinningNumbers = numberSets[0].Trim().Split(' ').Select(n => new Number(int.Parse(n))).ToList();
            WinningNumbersSet = new HashSet<int>(WinningNumbers.Select(n => n.Value));
            
            MyNumbers = numberSets[1].Trim().Split(' ').Select(n => new Number(int.Parse(n))).ToList();
            MyWinningNumbers = MyNumbers
                .Where(n => WinningNumbersSet.Contains(n.Value))
                .Select(n => n.Value)
                .ToList();
        }
    }

    public class Number
    {
        public Number(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }

    public class Solution
    {
        public readonly List<Line> _lines;

        public Solution(List<Line> lines)
        {
            _lines = lines;
        }

        public int Solve(ITestOutputHelper output)
        {
            int count = 0;

            Queue<Line> queue = new Queue<Line>();
            
            foreach (var line in _lines)
            {
                queue.Enqueue(line);
                count++;
                output.WriteLine(count.ToString());
            }

            while (queue.Count > 0)
            {
                var line = queue.Dequeue();

                for (int i = 0; i < line.MyWinningNumbers.Count; i++)
                {
                    if (line.CardNumber + i < _lines.Count)
                    {
                        queue.Enqueue(_lines[line.CardNumber+i]);
                        count++;
                        output.WriteLine(count.ToString());
                    }
                }
            }

            return count;
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

            solution.Solve(_output).Should().Be(30);
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

            _output.WriteLine("Answer: " + solution.Solve(_output).ToString()); // 11024379
        }
    }
}