using FluentAssertions;
using Xunit;

namespace Day3RucksackReorganization.Business.Tests
{
    public class Tests
    {
        [Theory]
        [InlineData("vJrwpWtwJgWrhcsFMMfFFhFp", 'p', 16)]
        [InlineData("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", 'L', 38)]
        [InlineData("PmmdzqPrVvPwwTWBwg", 'P', 42)]
        [InlineData("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", 'v', 22)]
        [InlineData("ttgJtRGJQctTZtZT", 't', 20)]
        [InlineData("CrZsJsPPZsGzwwsLwLmpwMDw", 's', 19)]
        public void TestLine(string inputString, char expectedDuplicateChar, int expectedScore)
        {
            var sut = new Line(inputString);

            sut.DuplicateChar.Should().Be(expectedDuplicateChar);
            sut.Score.Should().Be(expectedScore);
            sut.Left.Length.Should().Be(sut.Right.Length);
        }

        [Theory]
        [InlineData("vJrwpWtwJgWrhcsFMMfFFhFp", "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", "PmmdzqPrVvPwwTWBwg", 'r', 18)]
        [InlineData("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", "ttgJtRGJQctTZtZT", "CrZsJsPPZsGzwwsLwLmpwMDw", 'Z', 52)]
        public void TestGroup(string line1, string line2, string line3, char expectedBadgeChar, int expectedBadgeScore)
        {
            var sut = new LineGroup(
                new Line(line1),
                new Line(line2),
                new Line(line3));

            sut.BadgeChar.Should().Be(expectedBadgeChar);
            sut.BadgeScore.Should().Be(expectedBadgeScore);
        }
    }
}