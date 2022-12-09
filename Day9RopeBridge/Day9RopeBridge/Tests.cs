using FluentAssertions;
using Xunit;

namespace Day9RopeBridge
{
    public class Tests
    {
        [Fact]
        public void ShouldReadFiles()
        {
            File.ReadAllLines("input.txt").Should().NotBeEmpty();
        }
    }
}