using System.Text;
using FluentAssertions;
using Xunit;

namespace Day6TuningTrouble
{
    public class Tests
    {
        [Fact]
        public void ShouldReadLines()
        {
            File.ReadAllLines("input.txt").Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void FindSolutionPart1()
        {
            var sut = new Scanner();

            int position = sut.Scan(File.ReadAllLines("input.txt").First());

            Console.WriteLine(position);
        }

        [Theory]
        [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        public void ShouldFindMarkerPosition(string subject, int expectedPosition)
        {
            var sut = new Scanner();

            int position = sut.Scan(subject);

            position.Should().Be(expectedPosition);
        }
    }

    public class Scanner
    {
        public int Scan(string subject)
        {
            int counter = 0;
            var buffer = new List<char>();
            
            foreach (var c in subject)
            {
                buffer.Add(c);
                counter++;

                if (buffer.Distinct().Count() < buffer.Count)
                {
                    buffer.RemoveAt(0);
                }

                if (buffer.Count > 3)
                {
                    return counter;
                }
            }

            throw new InvalidOperationException();
        }
    }

}