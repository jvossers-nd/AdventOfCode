namespace Day5SupplyStacks;

public class Command
{
    public int From { get; }
    public int To { get; }
    public int Count { get; }
    public Command(int from, int to, int count)
    {
        From = from;
        To = to;
        Count = count;
    }

    public override string ToString()
    {
        return $"move {Count} from {From} to {To}";
    }
}