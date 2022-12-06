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
    }
}