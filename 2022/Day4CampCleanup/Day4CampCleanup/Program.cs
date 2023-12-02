using Day4CampCleanup.Business;

var list1 = File.ReadAllLines("input.txt")
    .Select(line => new AssignmentPair(line))
    .Where(p => p.FullyContained)
    .ToList();

Console.WriteLine(list1.Count);

var list2 = File.ReadAllLines("input.txt")
    .Select(line => new AssignmentPair(line))
    .Where(p => p.AnyOverlap)
    .ToList();

Console.WriteLine(list2.Count);

Console.Read();