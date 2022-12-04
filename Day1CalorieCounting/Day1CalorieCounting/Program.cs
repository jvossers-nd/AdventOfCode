// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt");

var elves = new List<Elf>();

var elf = new Elf();

int i = 0;

foreach (var line in lines)
{
    i++;

    if (int.TryParse(line, out int calories))
    {
        elf.FoodItems.Add(new FoodItem() { Calories = calories });

        if (i == lines.Length)
        {
            AddElf(i, elf);
        }
    }
    else
    {
        AddElf(i, elf);
        elf = new Elf(); // prep for next one
    }
}

// part 1
var solutionElf = elves.MaxBy(e => e.TotalCalories);
Console.WriteLine(solutionElf.TotalCalories);

// part 2
var orderedElves = elves.OrderByDescending(e => e.TotalCalories);
var top3Elves = orderedElves.Take(3);
var top3TotalCalories = top3Elves.Sum(e => e.TotalCalories);
Console.WriteLine(top3TotalCalories);

Console.Read();

void AddElf(int i, Elf elfToAdd)
{
    Console.WriteLine(i);
    if (elf.FoodItems.Any())
    {
        Console.WriteLine("elf added");
        elves.Add(elfToAdd);
    }
}

public class Elf
{
    public List<FoodItem> FoodItems { get; set; }

    public int TotalCalories => FoodItems.Sum(fi => fi.Calories);

    public Elf()
    {
        FoodItems = new List<FoodItem>();
    }
}

public class FoodItem
{
    public int Calories { get; set; }
}