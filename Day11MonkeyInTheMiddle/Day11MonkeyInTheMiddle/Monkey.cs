using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Day11MonkeyInTheMiddle;

public class Monkey
{
    private Func<int, int>? _operation;
    public string OperationText { get; }
    public int DivisibleBy { get; }
    public int TrueMonkeyIndex { get; }
    public int FalseMonkeyIndex { get; }
    public List<int> Items { get; }
    public List<Inspection> Inspections { get; }

    public Monkey(string operationText, int divisibleBy, int trueMonkeyIndex, int falseMonkeyIndex, List<int> items)
    {
        OperationText = operationText;
        Items = items;
        TrueMonkeyIndex = trueMonkeyIndex;
        FalseMonkeyIndex = falseMonkeyIndex;
        DivisibleBy = divisibleBy;

        Inspections = new List<Inspection>();
    }

    private async Task<Func<int, int>> GetOperation()
    {
        var opLambda = $"(old) => {OperationText}";

        if (_operation == null)
        {
            _operation = await CSharpScript.EvaluateAsync<Func<int, int>>(opLambda);
        }
            
        return _operation;
    }

    public async Task ApplyOperation()
    {
        var op = await GetOperation();

        Items[0] = op(Items[0]);
        Items[0] = (int)Math.Floor((double)Items[0] / 3);
    }
}

public class Inspection
{
}