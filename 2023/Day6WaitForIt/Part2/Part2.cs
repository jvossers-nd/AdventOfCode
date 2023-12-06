using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Part2
{
    public class RaceResult
    {
        public int Time { get; set; }
        public long Distance { get; set; }
    }

    public class RaceOption
    {
        public RaceOption(long chargeTime, long raceTime)
        {
            ChargeTime = chargeTime;
            RaceTime = raceTime;
        }

        public long ChargeTime { get; set; }
        public long RaceTime { get; set; }
        public long Speed => ChargeTime;
        public long Distance => Speed * RaceTime;
        public long TotalTime => ChargeTime + RaceTime;

        public override string ToString()
        {
            return $"{Distance}mm in {TotalTime}ms based on {ChargeTime}ms charge time and {RaceTime}ms race time at {Speed}ms speed.";
        }
    }
    
    public class Solution
    {
        public RaceResult CurrentWinner { get; set; }
        
        public Solution(RaceResult currentWinner)
        {
            CurrentWinner = currentWinner;
        }

        public long Solve()
        {
            // find first win, assume any longer charging times are automatically wins too
            long candidateChargeTime = 0;

            while (true)
            {
                var candidateOption = new RaceOption(candidateChargeTime, CurrentWinner.Time - candidateChargeTime);

                if (candidateOption.Distance > CurrentWinner.Distance)
                {
                    break;
                }

                candidateChargeTime++;
            }

            return CurrentWinner.Time-(2*candidateChargeTime)+1;
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
            var solution = new Solution(new RaceResult {Time = 71530, Distance = 940200});

            solution.Solve().Should().Be(71503);
        }

        [Fact]
        public void Answer()
        {
            //Time:        35937366
            //Distance:   212206012011044

            var solution = new Solution(new RaceResult { Time = 35937366, Distance = 212206012011044 });

            _output.WriteLine(solution.Solve().ToString()); // 21039729
        }
    }
}