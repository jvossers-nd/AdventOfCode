// See https://aka.ms/new-console-template for more information


using Day3RucksackReorganization.Business;

Console.WriteLine("Hello, World!");

var lines = File.ReadAllLines("input.txt").Select(s => new Line(s)).ToList();

// part 1
var sum = lines.Sum(line => line.Score);
Console.WriteLine(sum);

// part 2
var groups = lines
    .Chunk(3)
    .Select(chunkSet => new LineGroup(chunkSet.ElementAt(0), chunkSet.ElementAt(1), chunkSet.ElementAt(2)))
    .ToList();

var groupSum = groups.Sum(g => g.BadgeScore);
Console.WriteLine(groupSum);

Console.Read();
