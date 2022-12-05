namespace Day5SupplyStacks;

public class StackPlatform
{
    public List<CrateStack> Stacks { get; }
    public List<Command> Commands { get; set; }

    public StackPlatform()
    {
        Stacks = new List<CrateStack>();
    }

    public string TopLayer
    {
        get => String.Join("", Stacks.Select(s => s.Crates.Peek().Id));
    }

    public override string ToString()
    {
        return String.Join("|", Stacks.Select(s => s.ToString()));
    }

    public void AddCrate(int stackIndex, char id)
    {
        Stacks[stackIndex].Crates.Push(new Crate(id));
    }

    public void ExecuteCommands(bool multiCrateMode = false)
    {
        foreach (var command in Commands)
        {
            var tempStack = new Stack<Crate>();

            for (int i = 0; i < command.Count; i++)
            {
                var crate = Stacks[command.From-1].Crates.Pop();
                tempStack.Push(crate);
            }

            if (multiCrateMode)
            {
                tempStack.ToList().ForEach(c => Stacks[command.To-1].Crates.Push(c));
            }
            else
            {
                tempStack.Reverse().ToList().ForEach(c => Stacks[command.To-1].Crates.Push(c));
            }
        }
    }
}