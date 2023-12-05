using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1
{
    public class Line
    {
        public string Text { get; }

        public Line(string text)
        {
            Text = text;
        }
    }

    public class Range
    {
        public long DestinationRangeStart { get; set; }
        public long SourceRangeStart { get; set; }
        public long RangeLength { get; set; }
        public long SourceRangeEnd => SourceRangeStart + RangeLength;
        public long Offset => DestinationRangeStart - SourceRangeStart;
        public bool SourceInRange(long source) => source >= SourceRangeStart && source < SourceRangeEnd;
    }

    public class Map
    {
        private List<Range> _ranges;

        public Map(List<Range> ranges)
        {
            _ranges = ranges;
        }

        public Map() : this(new List<Range>())
        {
            
        }

        public void AddRanges(IEnumerable<Range> ranges)
        {
            _ranges.AddRange(ranges);
        }

        public void AddRange(long destinationRangeStart, long sourceRangeStart, long rangeLength)
        {
            _ranges.Add(new Range()
            {
                DestinationRangeStart = destinationRangeStart, 
                SourceRangeStart = sourceRangeStart, 
                RangeLength = rangeLength
            });
        }

        public long Lookup(long source)
        {
            var mapping = _ranges.SingleOrDefault(m => m.SourceInRange(source));

            if (mapping != null)
            {
                return source + mapping.Offset;
            }

            return source;
        }
    }

    public class Tests
    {
        private readonly ITestOutputHelper _output;

        public Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(79, 81)]
        [InlineData(14, 14)]
        [InlineData(55, 57)]
        [InlineData(13, 13)]
        // lower boundary tests
        [InlineData(49, 49)]
        [InlineData(50, 52)]
        [InlineData(51, 53)]
        // upper boundary tests
        [InlineData(98, 50)]
        [InlineData(99, 51)]
        [InlineData(100, 100)]
        public void RangeLookupTest(long source, long expected)
        {
            var sut = new Map();
            sut.AddRange(50, 98, 2);
            sut.AddRange(52, 50, 48);

            sut.Lookup(source).Should().Be(expected);
        }

        [Fact]
        public void Test()
        {
            var solution = new Solution(new List<Line>()
            {
                new Line("seeds: 79 14 55 13"),
                new Line(""),
                new Line("seed-to-soil map:"),
                new Line("50 98 2"),
                new Line("52 50 48"),
                new Line(""),
                new Line("soil-to-fertilizer map:"),
                new Line("0 15 37"),
                new Line("37 52 2"),
                new Line("39 0 15"),
                new Line(""),
                new Line("fertilizer-to-water map:"),
                new Line("49 53 8"),
                new Line("0 11 42"),
                new Line("42 0 7"),
                new Line("57 7 4"),
                new Line(""),
                new Line("water-to-light map:"),
                new Line("88 18 7"),
                new Line("18 25 70"),
                new Line(""),
                new Line("light-to-temperature map:"),
                new Line("45 77 23"),
                new Line("81 45 19"),
                new Line("68 64 13"),
                new Line(""),
                new Line("temperature-to-humidity map:"),
                new Line("0 69 1"),
                new Line("1 0 69"),
                new Line(""),
                new Line("humidity-to-location map:"),
                new Line("60 56 37"),
                new Line("56 93 4")
            });
            
            solution.Solve().Should().Be(35);

            //var seed79 = solution.Map(79);
            //seed79.Id.Should().Be(79);
            //seed79.Soil.Id.Should().Be(81);
            //seed79.Soil.Fertilizer.Id.Should().Be(81);
            //seed79.Soil.Fertilizer.Water.Id.Should().Be(81);
            //seed79.Soil.Fertilizer.Water.Light.Id.Should().Be(74);
            //seed79.Soil.Fertilizer.Water.Light.Temperature.Id.Should().Be(78);
            //seed79.Soil.Fertilizer.Water.Light.Temperature.Humidity.Id.Should().Be(78);
            //seed79.Soil.Fertilizer.Water.Light.Temperature.Humidity.Location.Id.Should().Be(82);

            //var seed14 = solution.Map(14);
            //seed14.Id.Should().Be(14);
            //seed14.Soil.Id.Should().Be(14);
            //seed14.Soil.Fertilizer.Id.Should().Be(53);
            //seed14.Soil.Fertilizer.Water.Id.Should().Be(49);
            //seed14.Soil.Fertilizer.Water.Light.Id.Should().Be(42);
            //seed14.Soil.Fertilizer.Water.Light.Temperature.Id.Should().Be(42);
            //seed14.Soil.Fertilizer.Water.Light.Temperature.Humidity.Id.Should().Be(43);
            //seed14.Soil.Fertilizer.Water.Light.Temperature.Humidity.Location.Id.Should().Be(43);

            //var seed55 = solution.Map(55);
            //seed55.Id.Should().Be(55);
            //seed55.Soil.Id.Should().Be(57);
            //seed55.Soil.Fertilizer.Id.Should().Be(57);
            //seed55.Soil.Fertilizer.Water.Id.Should().Be(53);
            //seed55.Soil.Fertilizer.Water.Light.Id.Should().Be(46);
            //seed55.Soil.Fertilizer.Water.Light.Temperature.Id.Should().Be(82);
            //seed55.Soil.Fertilizer.Water.Light.Temperature.Humidity.Id.Should().Be(82);
            //seed55.Soil.Fertilizer.Water.Light.Temperature.Humidity.Location.Id.Should().Be(86);

            //var seed13 = solution.Map(13);
            //seed13.Id.Should().Be(13);
            //seed13.Soil.Id.Should().Be(13);
            //seed13.Soil.Fertilizer.Id.Should().Be(52);
            //seed13.Soil.Fertilizer.Water.Id.Should().Be(41);
            //seed13.Soil.Fertilizer.Water.Light.Id.Should().Be(34);
            //seed13.Soil.Fertilizer.Water.Light.Temperature.Id.Should().Be(34);
            //seed13.Soil.Fertilizer.Water.Light.Temperature.Humidity.Id.Should().Be(35);
            //seed13.Soil.Fertilizer.Water.Light.Temperature.Humidity.Location.Id.Should().Be(35);
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

            _output.WriteLine(solution.Solve().ToString()); // 424490994
        }
    }
}