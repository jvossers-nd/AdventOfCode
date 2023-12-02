using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Day11MonkeyInTheMiddle;

public class Monkey
{
    private Func<long, long>? _operation;
    public string OperationText { get; }
    public int DivisibleBy { get; }
    public int TrueMonkeyIndex { get; }
    public int FalseMonkeyIndex { get; }
    public List<long> Items { get; }
    public List<Inspection> Inspections { get; }

    public Monkey(string operationText, int divisibleBy, int trueMonkeyIndex, int falseMonkeyIndex, List<long> items)
    {
        OperationText = operationText;
        Items = items;
        TrueMonkeyIndex = trueMonkeyIndex;
        FalseMonkeyIndex = falseMonkeyIndex;
        DivisibleBy = divisibleBy;
        Inspections = new List<Inspection>();
    }

    private async Task<Func<long, long>> GetOperation()
    {
        var opLambda = $"(old) => {OperationText}";

        if (_operation == null)
        {
            _operation = await CSharpScript.EvaluateAsync<Func<long, long>>(opLambda);
        }
            
        return _operation;
    }

    public async Task ApplyOperation()
    {
        var op = await GetOperation();

        Items[0] = op(Items[0]);
        Items[0] = (long) Math.Floor((double) Items[0] / 3);
    }

    public async Task ApplyOperationSupermodulo(long supermodulo)
    {
        var op = await GetOperation();
        Items[0] = op(Items[0]);
        Items[0] = Items[0] % supermodulo;
    }
}

public class Inspection
{
}