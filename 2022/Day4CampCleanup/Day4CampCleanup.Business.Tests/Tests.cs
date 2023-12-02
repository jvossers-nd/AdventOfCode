using FluentAssertions;
using Xunit;

namespace Day4CampCleanup.Business.Tests
{
    public class Tests
    {
        [Theory]
        [InlineData("2-4,6-8", false, false)]
        [InlineData("2-3,4-5", false, false)]
        [InlineData("5-7,7-9", false, true)]
        [InlineData("2-8,3-7", true, true)]
        [InlineData("6-6,4-6", true, true)]
        [InlineData("2-6,4-8", false, true)]
        public void Test(string textInput, bool expectedFullyContained, bool expectedAnyOverlap)
        {
            var sut = new AssignmentPair(textInput);

            sut.FullyContained.Should().Be(expectedFullyContained);
            sut.AnyOverlap.Should().Be(expectedAnyOverlap);
        }
    }
}