using Part2.Models;
using Xunit;

namespace Part2;

public class Solution
{
    public readonly List<Line> _lines;

    public Map SeedSoilMap { get; set; }
    public Map SoilFertilizerMap { get; set; }
    public Map FertilizerWaterMap { get; set; }
    public Map WaterLightMap { get; set; }
    public Map LightTemperatureMap { get; set; }
    public Map TemperatureHumidityMap { get; set; }
    public Map HumiditylocationMap { get; set; }
    public IEnumerable<Map> AllMaps => new List<Map>()
    {
        SeedSoilMap,
        SoilFertilizerMap,
        FertilizerWaterMap,
        WaterLightMap,
        LightTemperatureMap,
        TemperatureHumidityMap,
        HumiditylocationMap
    };

    public Solution(List<Line> lines)
    {
        _lines = lines;

        SeedSoilMap = new Map();
        SoilFertilizerMap = new Map();
        FertilizerWaterMap = new Map();
        WaterLightMap = new Map();
        LightTemperatureMap = new Map();
        TemperatureHumidityMap = new Map();
        HumiditylocationMap = new Map();
    }

    private IEnumerable<Range> ParseRanges(int startLineIndex)
    {
        int i = startLineIndex+1;

        while (true)
        {
            if (i < _lines.Count && !String.IsNullOrWhiteSpace(_lines[i].Text) && !_lines[i].Text.Contains("map:"))
            {
                var parts = _lines[i].Text.Split(' ');
                
                yield return new Range()
                {
                    DestinationRangeStart = long.Parse(parts[0]),
                    SourceRangeStart = long.Parse(parts[1]),
                    RangeLength = long.Parse(parts[2])
                };

                i++;
            }
            else
            {
                break;
            }
        }
    }
    
    public long Solve()
    {
        for (int i=0; i<_lines.Count;i++)
        {
            var line = _lines[i];

            if (line.Text == "seed-to-soil map:") SeedSoilMap.AddRanges(ParseRanges(i));
            if (line.Text == "soil-to-fertilizer map:") SoilFertilizerMap.AddRanges(ParseRanges(i));
            if (line.Text == "fertilizer-to-water map:") FertilizerWaterMap.AddRanges(ParseRanges(i));
            if (line.Text == "water-to-light map:") WaterLightMap.AddRanges(ParseRanges(i));
            if (line.Text == "light-to-temperature map:") LightTemperatureMap.AddRanges(ParseRanges(i));
            if (line.Text == "temperature-to-humidity map:") TemperatureHumidityMap.AddRanges(ParseRanges(i));
            if (line.Text == "humidity-to-location map:") HumiditylocationMap.AddRanges(ParseRanges(i));
        }
        
        var parts = _lines.First().Text.Split(':')[1].Trim().Split(' ').Select(s => long.Parse(s)).ToList();
        var seedRanges = SeedRange.FromParts(parts).ToList();
        long candidateLocationId = 0;

        while(true)
        {
            long humidityId = HumiditylocationMap.ReverseLookup(candidateLocationId);
            long temperatureId = TemperatureHumidityMap.ReverseLookup(humidityId);
            long lightId = LightTemperatureMap.ReverseLookup(temperatureId);
            long waterId = WaterLightMap.ReverseLookup(lightId);
            long fertilizerId = FertilizerWaterMap.ReverseLookup(waterId);
            long soilId = SoilFertilizerMap.ReverseLookup(fertilizerId);
            long seedId = SeedSoilMap.ReverseLookup(soilId);

            if (seedRanges.Any(r => r.IsInRange(seedId)))
            {
                return candidateLocationId;
            }

            candidateLocationId++;

            if (candidateLocationId % 1000000 == 0)
            {
                Console.WriteLine(candidateLocationId.ToString());
            }
        }

        throw new Exception("not found");
    }
    

    public Seed Map(long seedId)
    {
        var seed = new Seed() {Id = seedId};
        seed.Soil = new Soil() {Id = SeedSoilMap.Lookup(seed.Id)};
        seed.Soil.Fertilizer = new Fertilizer() {Id = SoilFertilizerMap.Lookup(seed.Soil.Id)};
        seed.Soil.Fertilizer.Water = new Water() {Id = FertilizerWaterMap.Lookup(seed.Soil.Fertilizer.Id)};
        seed.Soil.Fertilizer.Water.Light = new Light() {Id = WaterLightMap.Lookup(seed.Soil.Fertilizer.Water.Id)};
        seed.Soil.Fertilizer.Water.Light.Temperature = new Temperature() {Id = LightTemperatureMap.Lookup(seed.Soil.Fertilizer.Water.Light.Id)};
        seed.Soil.Fertilizer.Water.Light.Temperature.Humidity = new Humidity() {Id = TemperatureHumidityMap.Lookup(seed.Soil.Fertilizer.Water.Light.Temperature.Id)};
        seed.Soil.Fertilizer.Water.Light.Temperature.Humidity.Location = new Location() {Id = HumiditylocationMap.Lookup(seed.Soil.Fertilizer.Water.Light.Temperature.Humidity.Id)};

        return seed;
    }

    public class SeedRange
    {
        public long Start { get; set; }
        public long Length { get; set; }
        public long End => Start + Length;
        public static IEnumerable<SeedRange> FromParts(List<long> parts)
        {
            for (int i = 0; i < parts.Count; i += 2)
            {
                yield return new SeedRange()
                {
                    Start = parts[i],
                    Length = parts[i + 1]
                };
            }
        }

        public bool IsInRange(long seedId)
        {
            return seedId >= Start && seedId < End;
        }
    }
}