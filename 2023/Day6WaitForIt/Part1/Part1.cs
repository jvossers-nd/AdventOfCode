using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part1
{
    public class RaceResult
    {
        public int Time { get; set; }
        public int Distance { get; set; }
    }

    public class RaceOption
    {
        public RaceOption(int chargeTime, int raceTime)
        {
            ChargeTime = chargeTime;
            RaceTime = raceTime;
        }

        public int ChargeTime { get; set; }
        public int RaceTime { get; set; }
        public int Speed => ChargeTime;
        public int Distance => Speed * RaceTime;
        public int TotalTime => ChargeTime + RaceTime;

        public override string ToString()
        {
            return $"{Distance}mm in {TotalTime}ms based on {ChargeTime}ms charge time and {RaceTime}ms race time at {Speed}ms speed.";
        }
    }
    
    public class Solution
    {
        public List<RaceResult> HallOfFame { get; set; }

        public List<RaceOption> RaceOptions { get; set; }

        public Solution(List<RaceResult> hallOfFame)
        {
            HallOfFame = hallOfFame;
        }

        public long Solve()
        {
            RaceOptions = CalculateRaceOptions(HallOfFame.Max(r => r.Time));
            
            var allWins = new List<int>();

            foreach (var currentWinner in HallOfFame)
            {
                var wins = RaceOptions
                    .Where(o => o.Distance > currentWinner.Distance && o.TotalTime <= currentWinner.Time)
                    .Select(o => o.ChargeTime)
                    .Distinct()
                    .ToList();
                
                allWins.Add(wins.Count);
            }

            int modValue = allWins.Aggregate(1, (x,y) => x * y);

            return modValue;
            //return 4 * 8 * 9;
        }

        private List<RaceOption> CalculateRaceOptions(int upperLimit)
        {
            List<RaceOption> options = new List<RaceOption>();

            for (int chargeTime = 0; chargeTime < upperLimit; chargeTime++)
            {
                for (int raceTime = 0; raceTime < upperLimit; raceTime++)
                {
                    if (chargeTime + raceTime <= upperLimit)
                    {
                        options.Add(new RaceOption(chargeTime, raceTime));
                    }
                }
            }

            return options;
        }
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
            var solution = new Solution(new List<RaceResult>()
            {
                new RaceResult {Time = 7, Distance = 9},
                new RaceResult {Time = 15, Distance = 40},
                new RaceResult {Time = 30, Distance = 200}
            });

            solution.Solve().Should().Be(4 * 8 * 9);
        }

        [Fact]
        public void Answer()
        {
            var solution = new Solution(new List<RaceResult>()
            {
                new RaceResult {Time = 35, Distance = 212},
                new RaceResult {Time = 93, Distance = 2060},
                new RaceResult {Time = 73, Distance = 1201},
                new RaceResult {Time = 66, Distance = 1044},
            });

            _output.WriteLine(solution.Solve().ToString()); // 114400
        }
    }
}