using FluentAssertions;
using Xunit.Abstractions;
using Xunit;
using System.Text.RegularExpressions;

namespace Part2
{
      public class Line
    {
        private readonly string _text;

        public string Text => _text;

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
            Numbers = LoadNumbers(_lines).ToList();
            Symbols = LoadSymbols(_lines).ToList();
        }

        public List<Number> Numbers { get; set; }
        public List<Symbol> Symbols { get; set; }

        public IEnumerable<Symbol> LoadSymbols(IEnumerable<Line> lines)
        {
            int y = 0;

            var regex = new Regex("[^0-9.]");

            foreach (var line in lines)
            {
                var matches = regex.Matches(line.Text);

                foreach (Match match in matches)
                {
                    yield return new Symbol()
                    {
                        Value = match.Value.Single(),
                        X = match.Index,
                        Y = y
                    };
                }

                y++;
            }
        }

        public IEnumerable<Number> LoadNumbers(IEnumerable<Line> lines)
        {
            int y = 0;

            var regex = new Regex("([0-9])+");

            foreach (var line in lines)
            {
                var matches = regex.Matches(line.Text);

                foreach (Match match in matches)
                {
                    var numberText = match.Value;
                    int numberValue = int.Parse(numberText);
                    yield return new Number()
                    {
                        Value = numberValue,
                        Length = numberText.Length,
                        X = match.Index,
                        Y = y
                    };
                }

                y++;
            }
        }

        public IEnumerable<Number> NumbersAdjacentToGear(Symbol symbol)
        {
            var allDigits = this.Numbers.SelectMany(n => n.Digits).ToList();
            
            int x = symbol.X;
            int y = symbol.Y;

            List<Number> adjacentNumbers = new List<Number>();

            var n = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x && d.Y == y - 1));
            var ne = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x+1 && d.Y == y-1));
            var e = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x+1 && d.Y == y));
            var se = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x+1 && d.Y == y+1));
            var s = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x && d.Y == y+1));
            var sw = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x-1 && d.Y == y+1));
            var w = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x-1 && d.Y == y));
            var nw = this.Numbers.SingleOrDefault(n => n.Digits.Any(d => d.X == x-1 && d.Y == y-1));

            AddMatch(n);
            AddMatch(ne);
            AddMatch(e);
            AddMatch(se);
            AddMatch(s);
            AddMatch(sw);
            AddMatch(w);
            AddMatch(nw);

            void AddMatch(Number? number)
            {
                if(number != null && !adjacentNumbers.Contains(number)) adjacentNumbers.Add(number);
            }

            return adjacentNumbers;
        }

        public bool IsNumberAdjacentToSymbol(Number number)
        {
            int x = number.X;
            int y = number.Y;

            foreach (var _ in number.Digits)
            {
                // N
                if (Symbols.Any(s => s.X == x && s.Y == y-1)) return true;
                // NE
                if (Symbols.Any(s => s.X == x+1 && s.Y == y-1)) return true;
                // E
                if (Symbols.Any(s => s.X == x+1 && s.Y == y)) return true;
                // SE
                if (Symbols.Any(s => s.X == x+1 && s.Y == y+1)) return true;
                // S
                if (Symbols.Any(s => s.X == x && s.Y == y+1)) return true;
                // SW
                if (Symbols.Any(s => s.X == x-1 && s.Y == y+1)) return true;
                // W
                if (Symbols.Any(s => s.X == x-1 && s.Y == y)) return true;
                // NW
                if (Symbols.Any(s => s.X == x-1 && s.Y == y-1)) return true;

                x++;
            }

            return false;
        }
        
        public int Solve()
        {
            int total = 0;

            foreach (var gear in Symbols.Where(s => s.Value.Equals('*')))
            {
                var numbers = NumbersAdjacentToGear(gear);

                if (numbers.Count() == 2)
                {
                    total += numbers.ElementAt(0).Value * numbers.ElementAt(1).Value;
                }
            }

            return total;
        }
    }

    public class Symbol
    {
        public char Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Number
    {
        public int Value { get; set; }
        public int Length { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IEnumerable<Digit> Digits => Value.ToString().Select((c, i) => new Digit()
        {
            Value = int.Parse(c.ToString()),
            X = this.X + i,
            Y = this.Y,
            Number = this
        });
    }

    public class Digit
    {
        public int Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Number Number { get; set; }
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
                    new Line("467..114.."),
                    new Line("...*......"),
                    new Line("..35..633."),
                    new Line("......#..."),
                    new Line("617*......"),
                    new Line(".....+.58."),
                    new Line("..592....."),
                    new Line("......755."),
                    new Line("...$.*...."),
                    new Line(".664.598.."),
                });

            solution.Symbols.Should().HaveCount(6);

            solution.Symbols[0].Value.Should().Be('*');
            solution.Symbols[0].X.Should().Be(3);
            solution.Symbols[0].Y.Should().Be(1);
            solution.NumbersAdjacentToGear(solution.Symbols[0]).Count().Should().Be(2);

            solution.Symbols[1].Value.Should().Be('#');
            solution.Symbols[1].X.Should().Be(6);
            solution.Symbols[1].Y.Should().Be(3);

            solution.Symbols[2].Value.Should().Be('*');
            solution.Symbols[2].X.Should().Be(3);
            solution.Symbols[2].Y.Should().Be(4);

            solution.Symbols[3].Value.Should().Be('+');
            solution.Symbols[3].X.Should().Be(5);
            solution.Symbols[3].Y.Should().Be(5);

            solution.Symbols[4].Value.Should().Be('$');
            solution.Symbols[4].X.Should().Be(3);
            solution.Symbols[4].Y.Should().Be(8);

            solution.Symbols[5].Value.Should().Be('*');
            solution.Symbols[5].X.Should().Be(5);
            solution.Symbols[5].Y.Should().Be(8);

            solution.Numbers.Should().HaveCount(10);

            solution.Numbers[0].Value.Should().Be(467);
            solution.Numbers[0].X.Should().Be(0);
            solution.Numbers[0].Y.Should().Be(0);
            solution.Numbers[0].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[0]).Should().BeTrue();

            solution.Numbers[1].Value.Should().Be(114);
            solution.Numbers[1].X.Should().Be(5);
            solution.Numbers[1].Y.Should().Be(0);
            solution.Numbers[1].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[1]).Should().BeFalse();

            solution.Numbers[2].Value.Should().Be(35);
            solution.Numbers[2].X.Should().Be(2);
            solution.Numbers[2].Y.Should().Be(2);
            solution.Numbers[2].Length.Should().Be(2);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[2]).Should().BeTrue();

            solution.Numbers[3].Value.Should().Be(633);
            solution.Numbers[3].X.Should().Be(6);
            solution.Numbers[3].Y.Should().Be(2);
            solution.Numbers[3].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[3]).Should().BeTrue();

            solution.Numbers[4].Value.Should().Be(617);
            solution.Numbers[4].X.Should().Be(0);
            solution.Numbers[4].Y.Should().Be(4);
            solution.Numbers[4].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[4]).Should().BeTrue();

            solution.Numbers[5].Value.Should().Be(58);
            solution.Numbers[5].X.Should().Be(7);
            solution.Numbers[5].Y.Should().Be(5);
            solution.Numbers[5].Length.Should().Be(2);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[5]).Should().BeFalse();
            
            solution.Numbers[6].Value.Should().Be(592);
            solution.Numbers[6].X.Should().Be(2);
            solution.Numbers[6].Y.Should().Be(6);
            solution.Numbers[6].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[6]).Should().BeTrue();

            solution.Numbers[7].Value.Should().Be(755);
            solution.Numbers[7].X.Should().Be(6);
            solution.Numbers[7].Y.Should().Be(7);
            solution.Numbers[7].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[7]).Should().BeTrue();

            solution.Numbers[8].Value.Should().Be(664);
            solution.Numbers[8].X.Should().Be(1);
            solution.Numbers[8].Y.Should().Be(9);
            solution.Numbers[8].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[8]).Should().BeTrue();

            solution.Numbers[9].Value.Should().Be(598);
            solution.Numbers[9].X.Should().Be(5);
            solution.Numbers[9].Y.Should().Be(9);
            solution.Numbers[9].Length.Should().Be(3);
            solution.IsNumberAdjacentToSymbol(solution.Numbers[9]).Should().BeTrue();
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

            _output.WriteLine(solution.Solve().ToString()); 
            // 20506296 too low
            // 35543582 too low
            // 81709807
        }
    }
}