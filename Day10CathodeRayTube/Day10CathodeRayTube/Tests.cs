using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Day10CathodeRayTube
{
    public class Tests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Tests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldReadLines()
        {
            File.ReadAllLines("input.txt").Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldReadTestLines()
        {
            File.ReadAllLines("input.test.txt").Should().NotBeEmpty();
        }

        [Fact]
        public void ShouldProcessInstructions()
        {
            var instructions = new Parser().Parse(File.ReadAllLines("input.test.txt")).ToList();

            var sut = new Cpu();
            sut.ProcessInstructions(instructions);

            foreach (var cycle in sut.CycleHistory)
            {
                _testOutputHelper.WriteLine(cycle.ToString());
            }
            
            var cycle20 = sut.CycleHistory[19];
            cycle20.CycleNumber.Should().Be(20);
            cycle20.BeforeV.Should().Be(21);
            cycle20.SignalStrenght.Should().Be(420);

            var cycle60 = sut.CycleHistory[59];
            cycle60.CycleNumber.Should().Be(60);
            cycle60.BeforeV.Should().Be(19);
            cycle60.SignalStrenght.Should().Be(1140);

            var cycle100 = sut.CycleHistory[99];
            cycle100.CycleNumber.Should().Be(100);
            cycle100.BeforeV.Should().Be(18);
            cycle100.SignalStrenght.Should().Be(1800);

            var cycle140 = sut.CycleHistory[139];
            cycle140.CycleNumber.Should().Be(140);
            cycle140.BeforeV.Should().Be(21);
            cycle140.SignalStrenght.Should().Be(2940);

            var cycle180 = sut.CycleHistory[179];
            cycle180.CycleNumber.Should().Be(180);
            cycle180.BeforeV.Should().Be(16);
            cycle180.SignalStrenght.Should().Be(2880);

            var cycle220 = sut.CycleHistory[219];
            cycle220.CycleNumber.Should().Be(220);
            cycle220.BeforeV.Should().Be(18);
            cycle220.SignalStrenght.Should().Be(3960);
        }

        [Fact]
        public void SolutionPart1()
        {
            var instructions = new Parser().Parse(File.ReadAllLines("input.txt")).ToList();

            var sut = new Cpu();
            sut.ProcessInstructions(instructions);

            var cycles = new List<Cycle>()
            {
                sut.CycleHistory[19],
                sut.CycleHistory[59],
                sut.CycleHistory[99],
                sut.CycleHistory[139],
                sut.CycleHistory[179],
                sut.CycleHistory[219]
            };

            var sum = cycles.Sum(c => c.SignalStrenght);

            _testOutputHelper.WriteLine(sum.ToString());
        }
    }

    public class Parser
    {
        public IEnumerable<IInstruction> Parse(IEnumerable<string> lines)
        {
            return lines.Select<string, IInstruction>((line, index) =>
            {
                if (line == "noop")
                {
                    return new Noop(line, index);
                }

                if(line.StartsWith("addx"))
                {
                    return new AddX(line, index, int.Parse(line.Replace("addx ", string.Empty)));
                }

                throw new Exception("unsupported line");
            });
        }
    }

    public class Cpu
    {
        public List<Cycle> CycleHistory { get; }
        public int V => CycleHistory.LastOrDefault()?.AfterV ?? 1;
        public void AddCycle(string instruction, int instructionIndex, int beforeV, int afterV)
        {
            CycleHistory.Add(new Cycle(instruction,instructionIndex, CycleHistory.Count+1, beforeV, afterV));
        }

        public Cpu()
        {
            CycleHistory = new List<Cycle>();
        }

        public void ProcessInstructions(IEnumerable<IInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                instruction.Execute(this);
            }
        }
    }

    public class Cycle
    {
        public string Instruction { get; }
        public int InstructionIndex { get; }
        public int BeforeV { get; }
        public int AfterV { get; }
        public int CycleNumber { get; }
        public int SignalStrenght => BeforeV * CycleNumber;
        public Cycle(string instruction, int instructionIndex, int cycleNumber, int beforeV, int afterV)
        {
            Instruction = instruction;
            InstructionIndex = instructionIndex;
            CycleNumber = cycleNumber;
            BeforeV = beforeV;
            AfterV = afterV;
        }

        public override string ToString()
        {
            return $"{CycleNumber}. [{Instruction}] {BeforeV}-->{AfterV}";
        }
    }

    public interface IInstruction
    {
        public void Execute(Cpu cpu);
    }

    public class Noop : IInstruction
    {
        public string Instruction { get; }
        public int InstructionIndex { get; }
        public Noop(string instruction, int instructionIndex)
        {
            Instruction = instruction;
            InstructionIndex = instructionIndex;
        }

        public void Execute(Cpu cpu)
        {
            cpu.AddCycle(Instruction, InstructionIndex, cpu.V, cpu.V);
        }
    }
    public class AddX : IInstruction
    {
        public string Instruction { get; }
        public int InstructionIndex { get; }
        public int V { get; }
        public AddX(string instruction, int instructionIndex, int v)
        {
            Instruction = instruction;
            InstructionIndex = instructionIndex;
            V = v;
        }

        public void Execute(Cpu cpu)
        {
            cpu.AddCycle(Instruction, InstructionIndex,cpu.V, cpu.V);
            cpu.AddCycle(Instruction, InstructionIndex, cpu.V, cpu.V+V);
        }
    }

}