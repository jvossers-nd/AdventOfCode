using FluentAssertions;
using Xunit;

namespace Day11MonkeyInTheMiddle
{
    public class Tests
    {
        [Fact]
        public void ShouldReadLines()
        {
            File.ReadAllLines("input.txt").Should().NotBeEmpty();
        }
    }
}