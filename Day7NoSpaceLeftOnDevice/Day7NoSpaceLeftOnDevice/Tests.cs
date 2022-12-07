using System.Globalization;
using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace Day7NoSpaceLeftOnDevice
{
    public class Files
    {
        public const string InputFile = "input.txt";
        public const string InputFileTest = "input.test.txt";
    }

    public class Tests
    {
        [Theory]
        [InlineData(Files.InputFile)]
        [InlineData(Files.InputFileTest)]
        public void ShouldReadFile(string fileName)
        {
            File.ReadAllLines(fileName).Length.Should().BeGreaterThan(0);
        }

        // build filestructure hierarchy
        // calc direct folder size
        // add recursion to include child folder sizes in calculation
        // filter where dir.size < 100000
        // sum of the above list of filtered list
    }
}