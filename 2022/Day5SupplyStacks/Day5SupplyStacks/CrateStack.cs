namespace Day5SupplyStacks;

public class CrateStack
{
    public Stack<Crate> Crates { get; }

    public CrateStack()
    {
        Crates = new Stack<Crate>();
    }
    public override string ToString()
    {
        return String.Join("", Crates.Select(c => c.Id));
    }
}